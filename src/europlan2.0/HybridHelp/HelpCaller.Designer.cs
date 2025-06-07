
namespace HybridHelp
{
    partial class HelpCaller
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.picQrCode = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picQrCode)).BeginInit();
            this.SuspendLayout();
            // 
            // picQrCode
            // 
            this.picQrCode.Cursor = System.Windows.Forms.Cursors.Help;
            this.picQrCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picQrCode.Location = new System.Drawing.Point(2, 2);
            this.picQrCode.Name = "picQrCode";
            this.picQrCode.Size = new System.Drawing.Size(58, 58);
            this.picQrCode.TabIndex = 0;
            this.picQrCode.TabStop = false;
            this.picQrCode.Click += new System.EventHandler(this.picQrCode_Click);
            // 
            // HelpCaller
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.picQrCode);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "HelpCaller";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(62, 62);
            ((System.ComponentModel.ISupportInitialize)(this.picQrCode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picQrCode;
    }
}
