using System;
using System.Windows.Forms;

namespace SuperImageEvolver {
    public sealed partial class ModuleSettingsDisplay : Form {
        public ICloneable Module {
            get;
            private set;
        }

        public ModuleSettingsDisplay( ICloneable module ) {
            if( module == null ) throw new ArgumentNullException( "module" );
            InitializeComponent();
            Text = module.GetType().Name;
            Module = (ICloneable)module.Clone();
            pgGrid.SelectedObject = Module;
        }
    }
}