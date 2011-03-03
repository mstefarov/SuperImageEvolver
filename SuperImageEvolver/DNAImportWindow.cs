using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SuperImageEvolver {
    public partial class DNAImportWindow : Form {
        public string DNA {
            get {
                return tPasteArea.Text;
            }
        }

        public DNAImportWindow() {
            InitializeComponent();
            tPasteArea.Text = Clipboard.GetText( TextDataFormat.Text );
        }

        private void tPasteArea_TextChanged( object sender, EventArgs e ) {
            bImport.Enabled = tPasteArea.Text.Length > 0;
        }
    }
}
