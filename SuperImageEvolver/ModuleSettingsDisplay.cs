using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SuperImageEvolver {
    public partial class ModuleSettingsDisplay : Form {
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