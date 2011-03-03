namespace SuperImageEvolver {
    partial class DNAImportWindow {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing ) {
            if( disposing && (components != null) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.tPasteArea = new System.Windows.Forms.TextBox();
            this.bImport = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tPasteArea
            // 
            this.tPasteArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tPasteArea.Location = new System.Drawing.Point( 12, 12 );
            this.tPasteArea.Multiline = true;
            this.tPasteArea.Name = "tPasteArea";
            this.tPasteArea.Size = new System.Drawing.Size( 206, 112 );
            this.tPasteArea.TabIndex = 0;
            this.tPasteArea.TextChanged += new System.EventHandler( this.tPasteArea_TextChanged );
            // 
            // bImport
            // 
            this.bImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bImport.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bImport.Enabled = false;
            this.bImport.Location = new System.Drawing.Point( 12, 188 );
            this.bImport.Name = "bImport";
            this.bImport.Size = new System.Drawing.Size( 100, 23 );
            this.bImport.TabIndex = 1;
            this.bImport.Text = "Import";
            this.bImport.UseVisualStyleBackColor = true;
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point( 118, 188 );
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size( 100, 23 );
            this.bCancel.TabIndex = 2;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 12, 137 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 201, 39 );
            this.label1.TabIndex = 3;
            this.label1.Text = "WARNING: You are about to replace the\r\ncurrent DNA with this. The only way to\r\nun" +
                "do this will be to load an earlier save.";
            // 
            // DNAImportWindow
            // 
            this.AcceptButton = this.bImport;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size( 230, 223 );
            this.Controls.Add( this.label1 );
            this.Controls.Add( this.bCancel );
            this.Controls.Add( this.bImport );
            this.Controls.Add( this.tPasteArea );
            this.Name = "DNAImportWindow";
            this.Text = "Importing DNA";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tPasteArea;
        private System.Windows.Forms.Button bImport;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Label label1;
    }
}