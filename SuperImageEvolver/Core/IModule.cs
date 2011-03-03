using System;
using System.IO;

namespace SuperImageEvolver {
    public interface IModule : ICloneable {
        void ReadSettings( NBTag tag );

        void WriteSettings( NBTag tag );
    }


    public interface IModuleFactory {
        Type ModuleType { get; }

        ModuleFunction Function { get; }

        string ID { get; }

        ModulePreset[] Presets { get; }

        IModule GetInstance();
    }


    public class ModulePreset {
        public ModulePreset( string _name, ModuleInstanceCallback _callback, IModuleFactory _factory ) {
            Name = _name;
            callback = _callback;
            Factory = _factory;
        }

        public string Name { get; private set; }
        public IModuleFactory Factory { get; private set; }

        ModuleInstanceCallback callback;

        public IModule GetInstance() {
            return callback();
        }
    }

    public delegate IModule ModuleInstanceCallback();

    public enum ModuleFunction {
        Evaluator,
        Initializer,
        Mutator
    }

    public class DisableAutoSerializationAttribute : Attribute { }
}