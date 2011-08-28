using System;
using System.Windows.Forms;

namespace SuperImageEvolver {
    public sealed partial class DNAImportWindow : Form {
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
