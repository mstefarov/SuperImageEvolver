using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;

namespace SuperImageEvolver {
    static class WorkServer {
        private static ClientConnection[] connections;
        public static int NumClients { get; private set; }

        public static void Start() {
            try {
                NumClients = Environment.ProcessorCount - 1;
                connections = new ClientConnection[NumClients];
                for (int i = 0; i < NumClients; i++) {
                    string pipeName = "SuperImageEvolver." + Process.GetCurrentProcess().Id + "." + i;
                    var process = Process.Start(Process.GetCurrentProcess().MainModule.FileName, "pipe=" + pipeName);
                    var pipe = new NamedPipeServerStream(pipeName, PipeDirection.InOut);
                    connections[i] = new ClientConnection(process, pipe);
                }
                for (int i = 0; i < NumClients; i++) {
                    connections[i].Pipe.WaitForConnection();
                }
            } catch {
                Shutdown();
            }
        }


        public static void Shutdown() {
            foreach (var c in connections)
                c.Process.Kill();
        }

        private static void Broadcast(NBTCompound tag) {
            foreach (var c in connections) {
                c.WriteTag(tag);
            }
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
                var tag = cc.ReadTag();
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
            return reports;
        }

        private class ClientConnection {
            public Process Process { get; }
            public NamedPipeServerStream Pipe { get; }
            private BinaryWriter writer;
            private BinaryReader reader;

            public ClientConnection(Process process, NamedPipeServerStream pipe) {
                Process = process;
                Pipe = pipe;
                reader = new BinaryReader(pipe);
                writer = new BinaryWriter(pipe);
            }

            public NBTag ReadTag() {
                return (NBTCompound)NBTag.ReadTag(reader, (NBTType)reader.ReadByte(), null, null);
            }

            public void WriteTag(NBTag tag) {
                tag.WriteTag(writer);
                writer.Flush();
            }
        }
    }
}
