using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace SuperImageEvolver {
    public static class ModuleManager {
        static readonly Dictionary<string, IModuleFactory> FactoriesById = new Dictionary<string, IModuleFactory>();
        static readonly Dictionary<string, ModulePreset> Presets = new Dictionary<string, ModulePreset>();
        static readonly Dictionary<Type, IModuleFactory> FactoriesByType = new Dictionary<Type, IModuleFactory>();


        public static Dictionary<string, ModulePreset> GetPresets( ModuleFunction function ) {
            return Presets.Where( p => p.Value.Factory.Function == function ).ToDictionary( k => k.Key, v => v.Value );
        }


        public static void LoadAllPluginAssemblies( string path ) {
            foreach( string file in Directory.GetFiles( path, "*.SIE.dll" ) ) {
                LoadFactories( Assembly.LoadFile( file ) );
            }
        }


        static int factoriesLoadedFlag = 0;
        public static void LoadFactories( Assembly assembly ) {
            if (Interlocked.CompareExchange(ref factoriesLoadedFlag, 1, 0) != 0)
                return;

            foreach ( Type type in assembly.GetTypes() ) {
                if( type.GetInterfaces().Contains( typeof( IModuleFactory ) ) ) {
                    object newFactory = type.GetConstructor( Type.EmptyTypes ).Invoke( new object[0] );
                    AddModule( newFactory as IModuleFactory );
                }
            }
        }


        public static void AddModule( IModuleFactory factory ) {
            foreach( ModulePreset preset in factory.Presets ) {
                Presets.Add( preset.Name, preset );
            }
            FactoriesByType.Add( factory.ModuleType, factory );
            FactoriesById.Add( factory.ID, factory );
        }


        public static IModuleFactory GetFactoryByID( string id ) {
            return FactoriesById[id];
        }


        public static IModuleFactory GetFactoryByType( Type type ) {
            return FactoriesByType[type];
        }


        public static IModule GetPresetByName( string id ) {
            return Presets[id].GetInstance();
        }


        public static IModuleFactory[] ListAllModules() {
            return FactoriesById.Values.ToArray();
        }

        public static IModule ReadModule( NBTag tag) {
            string moduleID = tag["ID"].GetString();
            if( !FactoriesById.ContainsKey( moduleID ) ) {
                return null;
            }
            IModuleFactory factory = GetFactoryByID( moduleID );
            IModule module = factory.GetInstance();

            if( tag.Contains( "Properties" ) ) {}

            module.ReadSettings( tag["Settings"] );

            return module;
        }

        public static NBTag WriteModule( string tagName, IModule module ) {
            NBTCompound root = new NBTCompound( tagName );
            IModuleFactory factory = GetFactoryByType( module.GetType() );
            root.Append( "ID", factory.ID );

            NBTag customSettings = new NBTCompound( "Settings" );
            module.WriteSettings( customSettings );
            root.Append( customSettings );
            return root;
        }
    }
}
