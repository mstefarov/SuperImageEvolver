using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SuperImageEvolver {
    class WorkClient {
        private static BinaryWriter writer;

        public static async Task Run(string pipeName) {
            ModuleManager.LoadFactories(Assembly.GetExecutingAssembly());

            try {
                var pipe = new NamedPipeClientStream(".", pipeName);
                pipe.Connect();
                var reader = new BinaryReader(pipe);
                writer = new BinaryWriter(pipe);
                CancellationTokenSource cts = null;

                TaskState state = null;
                StateRunner runner = null;
                Task runnerTask = null;
                while (pipe.IsConnected) {
                    var tag = (NBTCompound)NBTag.ReadTag(reader, (NBTType)reader.ReadByte(), null, null);
                    switch (tag.Name) {
                        case "load":
                            cts?.Cancel();
                            if (runnerTask != null) await runnerTask;
                            state = new TaskState(tag["fullState"]);
                            state.Stats.Reset();
                            state.SetOriginalImage(state.OriginalImage);
                            state.SetEvaluator(state.Evaluator);
                            runner = new StateRunner(state);
                            break;

                        case "report":
                            var msg = new NBTCompound("workUpdate");
                            var statsTag = new NBTCompound("stats");
                            lock (state.ImprovementLock) {
                                state.Stats.Store(statsTag);
                                state.Stats.Reset();
                                msg.Tags.Add("bestMatch", state.BestMatch.SerializeNBT("bestMatch"));
                            }
                            msg.Tags.Add("stats", statsTag);
                            msg.WriteTag(writer);
                            writer.Flush();
                            pipe.WaitForPipeDrain();
                            break;

                        case "updateConfig":
                            runner.Pause();
                            lock (state.ImprovementLock) {
                                state.ReadCoreConfig(tag["stateChanges"]);
                                state.SetEvaluator(state.Evaluator);
                            }
                            break;

                        case "pause":
                            cts?.Cancel();
                            if (runnerTask != null) await runnerTask;
                            runnerTask=null;
                            break;

                        case "resume":
                            if (runner == null) {
                                runner = new StateRunner(state);
                            } else if (runnerTask == null) {
                                cts = new CancellationTokenSource();
                                runnerTask = Task.Run(() => runner.RunAsync(cts.Token), cts.Token);
                            } else {
                                runner.Resume();
                            }
                            break;

                        case "exit":
                            cts?.Cancel();
                            if (runnerTask != null) await runnerTask;
                            return;
                    }
                }
            } catch (IOException) {
                // expected if server disconnects
            }
        }
    }
}
