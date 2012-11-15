using System;
using System.Windows.Forms;

namespace SuperImageEvolver {
    public partial class ModuleSettingsDisplay<T> : Form where T : class, ICloneable {
        public T Module { get; private set; }


        public ModuleSettingsDisplay( T module ) {
            if( module == null ) throw new ArgumentNullException( "module" );
            InitializeComponent();
            Text = module.GetType().Name;
            Module = (T)module.Clone();
            pgGrid.SelectedObject = Module;
        }
    }
}