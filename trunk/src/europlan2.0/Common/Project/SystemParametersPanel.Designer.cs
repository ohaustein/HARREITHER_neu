namespace Europlan.Common {
	partial class SystemParametersPanel {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.label1 = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabEuroval = new System.Windows.Forms.TabPage();
			this.tabHitherm = new System.Windows.Forms.TabPage();
			this.tabModulBoden = new System.Windows.Forms.TabPage();
			this.tabModulDecke = new System.Windows.Forms.TabPage();
			this.tabControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(171, 24);
			this.label1.TabIndex = 3;
			this.label1.Text = "Systemparameter";
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabEuroval);
			this.tabControl1.Controls.Add(this.tabHitherm);
			this.tabControl1.Controls.Add(this.tabModulBoden);
			this.tabControl1.Controls.Add(this.tabModulDecke);
			this.tabControl1.Location = new System.Drawing.Point(0, 27);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(719, 392);
			this.tabControl1.TabIndex = 4;
			// 
			// tabEuroval
			// 
			this.tabEuroval.Location = new System.Drawing.Point(4, 22);
			this.tabEuroval.Name = "tabEuroval";
			this.tabEuroval.Padding = new System.Windows.Forms.Padding(3);
			this.tabEuroval.Size = new System.Drawing.Size(711, 366);
			this.tabEuroval.TabIndex = 0;
			this.tabEuroval.Text = "Euroval®";
			this.tabEuroval.UseVisualStyleBackColor = true;
			// 
			// tabHitherm
			// 
			this.tabHitherm.Location = new System.Drawing.Point(4, 22);
			this.tabHitherm.Name = "tabHitherm";
			this.tabHitherm.Size = new System.Drawing.Size(711, 366);
			this.tabHitherm.TabIndex = 1;
			this.tabHitherm.Text = "Hitherm®";
			this.tabHitherm.UseVisualStyleBackColor = true;
			// 
			// tabModulBoden
			// 
			this.tabModulBoden.Location = new System.Drawing.Point(4, 22);
			this.tabModulBoden.Name = "tabModulBoden";
			this.tabModulBoden.Size = new System.Drawing.Size(711, 366);
			this.tabModulBoden.TabIndex = 2;
			this.tabModulBoden.Text = "Modul Klima-Boden";
			this.tabModulBoden.UseVisualStyleBackColor = true;
			// 
			// tabModulDecke
			// 
			this.tabModulDecke.Location = new System.Drawing.Point(4, 22);
			this.tabModulDecke.Name = "tabModulDecke";
			this.tabModulDecke.Size = new System.Drawing.Size(711, 366);
			this.tabModulDecke.TabIndex = 3;
			this.tabModulDecke.Text = "Modul Klima-Decke";
			this.tabModulDecke.UseVisualStyleBackColor = true;
			// 
			// SystemParametersPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.label1);
			this.Name = "SystemParametersPanel";
			this.Size = new System.Drawing.Size(719, 422);
			this.tabControl1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabEuroval;
		private System.Windows.Forms.TabPage tabHitherm;
		private System.Windows.Forms.TabPage tabModulBoden;
		private System.Windows.Forms.TabPage tabModulDecke;
	}
}
