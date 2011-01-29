using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;


namespace SuperImageEvolver {
    public static class ModuleManager {

        static Dictionary<string, IModuleFactory> factoriesByID = new Dictionary<string, IModuleFactory>();
        static Dictionary<string, ModulePreset> presets = new Dictionary<string, ModulePreset>();
        static Dictionary<Type, IModuleFactory> factoriesByType = new Dictionary<Type, IModuleFactory>();

        public static void LoadAllPluginAssemblies( string path ) {
            foreach( string file in Directory.GetFiles( path, "*.SIE.dll" ) ) {
                LoadFactories( Assembly.LoadFile( file ) );
            }
        }


        public static void LoadFactories( Assembly assembly ) {
            foreach( Type type in assembly.GetTypes() ) {
                if( type.GetInterfaces().Contains( typeof( IModuleFactory ) ) ) {
                    object newFactory = type.GetConstructor( Type.EmptyTypes ).Invoke( new object[0] );
                    AddModule( newFactory as IModuleFactory );
                }
            }
        }


        public static void AddModule( IModuleFactory factory ) {
            foreach( ModulePreset preset in factory.Presets ) {
                presets.Add( preset.Name, preset );
            }
            factoriesByType.Add( factory.ModuleType, factory );
            factoriesByID.Add( factory.ID, factory );
        }


        public static IModuleFactory GetFactoryByID( string ID ) {
            return factoriesByID[ID];
        }


        public static IModuleFactory GetFactoryByType( Type type ) {
            return factoriesByType[type];
        }


        public static IModule GetPresetByName( string ID ) {
            return presets[ID].GetInstance();
        }


        public static IModule ReadModule( Stream stream ) {
            BinaryReader reader = new BinaryReader(stream);
            string moduleID = reader.ReadString();
            int settingsLength = reader.ReadInt32();
            if( factoriesByID.ContainsKey( moduleID ) ) {
                IModuleFactory factory = GetFactoryByID( moduleID );
                IModule module = factory.GetInstance();
                module.ReadSettings( reader, settingsLength );
                return module;
            } else {
                stream.Seek( settingsLength, SeekOrigin.Current );
                return null;
            }
        }


        public static void WriteModule( IModule module, Stream stream ) {
            BinaryWriter writer = new BinaryWriter( stream );
            IModuleFactory factory = GetFactoryByType(module.GetType());
            writer.Write( factory.ID );
            module.WriteSettings( writer );
        }
    }
}