using System.Windows.Forms;

namespace SuperImageEvolver {
    public sealed partial class ModuleSettingsDisplay : Form {
        public IModule Module {
            get;
            private set;
        }

        public ModuleSettingsDisplay( IModule module ) {
            InitializeComponent();
            Text = module.GetType().Name;
            Module = (IModule)module.Clone();
            pgGrid.SelectedObject = Module;
        }
    }
}