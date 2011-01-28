using System;
using System.IO;

namespace SuperImageEvolver {
    public interface IModule : ICloneable {
        void ReadSettings( BinaryReader stream, int settingsLength );

        void WriteSettings( BinaryWriter stream );
    }


    public interface IModuleFactory {
        Type ModuleType { get; }

        ModuleFunction Function { get; }

        string ID { get; }

        ModulePreset[] Presets { get; }

        IModule GetInstance();
    }


    public class ModulePreset {
        public ModulePreset( string _name, ModuleInstanceCallback _callback ) {
            Name = _name;
            callback = _callback;
        }

        public string Name { get; private set; }

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
}