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
                Debug.WriteLine("Connecting...");
                var pipe = new NamedPipeClientStream(".", pipeName);
                pipe.Connect();
                Debug.WriteLine("Connected!");
                var reader = new BinaryReader(pipe);
                writer = new BinaryWriter(pipe);
                CancellationTokenSource cts = null;

                TaskState state = null;
                StateRunner runner = null;
                Task runnerTask = null;
                while (pipe.IsConnected) {
                    Debug.WriteLine("Waiting for next tag...");
                    var tag = (NBTCompound)NBTag.ReadTag(reader, (NBTType)reader.ReadByte(), null, null);
                    Debug.WriteLine("> " + tag.Name);
                    switch (tag.Name) {
                        case "report":
                            Debug.WriteLine("Sending [workUpdate]");
                            var msg = new NBTCompound("workUpdate");
                            msg.Tags.Add("bestMatch", state.BestMatch.SerializeNBT("bestMatch"));
                            var statsTag = new NBTCompound("stats");
                            lock (state.ImprovementLock) {
                                state.Stats.Store(statsTag);
                                state.Stats.Reset();
                            }
                            msg.Tags.Add("stats", statsTag);
                            msg.WriteTag(writer);
                            writer.Flush();
                            pipe.WaitForPipeDrain();
                            break;

                        case "load":
                            cts?.Cancel();
                            if (runnerTask != null) await runnerTask;
                            state = new TaskState(tag["fullState"]);
                            state.SetOriginalImage(state.OriginalImage);
                            state.SetEvaluator(state.Evaluator);
                            runner = new StateRunner(state);
                            break;

                        case "updateConfig":
                            cts?.Cancel();
                            if (runnerTask != null) await runnerTask;
                            lock (state.ImprovementLock) {
                                state.ReadCoreConfig(tag["stateChanges"]);
                                state.SetEvaluator(state.Evaluator);
                            }
                            break;

                        case "resume":
                            cts?.Cancel();
                            if (runnerTask != null) await runnerTask;
                            cts = new CancellationTokenSource();
                            runnerTask = Task.Run(() => runner.RunAsync(cts.Token));
                            break;

                        case "pause":
                            cts?.Cancel();
                            if (runnerTask != null) await runnerTask;
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
