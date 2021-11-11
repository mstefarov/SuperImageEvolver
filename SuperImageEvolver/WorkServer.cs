using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading.Tasks;

namespace SuperImageEvolver {
    static class WorkServer {
        private static ClientConnection[] connections;
        public static int NumClients { get; private set; }

        public static void Start() {
            try {
                NumClients = Environment.ProcessorCount - 1;
                if (Debugger.IsAttached){
                    NumClients = 1;
                }

                connections = new ClientConnection[NumClients];
                for (int i = 0; i < NumClients; i++) {
                    connections[i] = Connect(i);
                }

                for (int i = 0; i < NumClients; i++) {
                    connections[i].Pipe.WaitForConnection();
                }
                
            } catch {
                Shutdown();
            }
        }

        private static ClientConnection Connect(int i) {
            string pipeName = "SuperImageEvolver." + Process.GetCurrentProcess().Id + "." + i;
            Process process;
            if (Debugger.IsAttached) {
                process = null;
                Task.Run(() => WorkClient.Run(pipeName));
            } else {
                process = Process.Start(Process.GetCurrentProcess().MainModule.FileName, "pipe=" + pipeName);
                process.PriorityClass = ProcessPriorityClass.BelowNormal;
            }
            var pipe = new NamedPipeServerStream(pipeName, PipeDirection.InOut);
            return new ClientConnection(process, pipe, i);
        }

        private static void CheckForDeadConnections() {
            foreach (var c in connections.Where(c => c.IsBroken))
                Restart(c);
        }

        private static ClientConnection Restart(ClientConnection cc) {
            try {
                cc.Process?.Kill();
            } catch { }

            int newId = connections.Max(c => c.Id) + 1;
            var oldIndex = Array.IndexOf(connections, cc);
            var newConnection = Connect(newId);
            connections[oldIndex] = newConnection;
            return newConnection;
        }

        public static void Shutdown() {
            foreach (var c in connections)
                c.Process?.Kill();
        }

        private static void Broadcast(NBTCompound tag) {
            foreach (var c in connections) {
                c.WriteTag(tag);
            }
            CheckForDeadConnections();
        }

        public static void SendLoad(TaskState state) {
            var msg = new NBTCompound("load");
            lock (state.ImprovementLock) {
                msg.Tags.Add("fullState", state.SerializeNBT("fullState"));
            }
            Broadcast(msg);
        }

        public static void SendUpdateConfig(TaskState state) {
            var msg = new NBTCompound("updateConfig");
            var update = new NBTCompound("stateChanges");
            lock (state.ImprovementLock) {
                state.StoreCoreConfig(update);
            }
            msg.Tags.Add("stateChanges", update);
            Broadcast(msg);
        }

        public static void SendResume() {
            Broadcast(new NBTCompound("resume"));
        }

        public static void SendPause() {
            Broadcast(new NBTCompound("pause"));
        }

        public static void SendExit() {
            Broadcast(new NBTCompound("exit"));
        }

        public static void RequestReports() {
            Broadcast(new NBTCompound("report"));
        }

        public static List<DNA> ReadWorkUpdates(TaskState state) {
            var reports = new List<DNA>(NumClients);
            for (int i = 0; i < NumClients; i++) {
                var cc = connections[i];
                NBTag tag;
                try {
                    tag = cc.ReadTag();
                } catch (IOException) {
                    continue;
                }

                switch (tag.Name) {
                    case "workUpdate":
                        if (tag["configVersion"].GetInt() != state.ConfigVersion)
                            continue; // Skip stale matches
                        lock (state.ImprovementLock) {
                            state.Stats.Merge(tag["stats"]);
                        }
                        reports.Add(new DNA(tag["bestMatch"]));
                        break;
                    default:
                        throw new InvalidDataException();
                }
            }
            CheckForDeadConnections();
            return reports;
        }

        private class ClientConnection {
            public int Id { get; }
            public Process Process { get; }
            public NamedPipeServerStream Pipe { get; }
            private BinaryWriter writer;
            private BinaryReader reader;
            public bool IsBroken { get; private set; }

            public ClientConnection(Process process, NamedPipeServerStream pipe, int id) {
                Process = process;
                Pipe = pipe;
                reader = new BinaryReader(pipe);
                writer = new BinaryWriter(pipe);
            }

            public NBTag ReadTag() {
                try {
                    return (NBTCompound)NBTag.ReadTag(reader, (NBTType)reader.ReadByte(), null, null);
                } catch {
                    IsBroken = true;
                    throw;
                }
            }

            public void WriteTag(NBTag tag) {
                try {
                    tag.WriteTag(writer);
                    writer.Flush();
                } catch (IOException ex) {
                    Debug.WriteLine(ex);
                    IsBroken = true;
                }
            }
        }
    }
}
