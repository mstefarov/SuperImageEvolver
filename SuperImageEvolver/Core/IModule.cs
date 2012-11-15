using System;

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


    public sealed class ModulePreset {
        public ModulePreset( string name, ModuleInstanceCallback _callback, IModuleFactory factory ) {
            Name = name;
            callback = _callback;
            Factory = factory;
        }


        readonly ModuleInstanceCallback callback;
        public string Name { get; private set; }
        public IModuleFactory Factory { get; private set; }


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


    public sealed class DisableAutoSerializationAttribute : Attribute {}
}