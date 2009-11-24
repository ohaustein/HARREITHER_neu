namespace Europlan.Common {
	partial class RequiredMaterialPanel {
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
			this.tabSystemParameters = new System.Windows.Forms.TabControl();
			this.tabFloor = new System.Windows.Forms.TabPage();
			this.tabWall = new System.Windows.Forms.TabPage();
			this.tabCeiling = new System.Windows.Forms.TabPage();
			this.tabDistributor = new System.Windows.Forms.TabPage();
			this.tabInsulation = new System.Windows.Forms.TabPage();
			this.tabGeneral = new System.Windows.Forms.TabPage();
			this.tabSystemParameters.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(142, 24);
			this.label1.TabIndex = 4;
			this.label1.Text = "Materialbedarf";
			// 
			// tabSystemParameters
			// 
			this.tabSystemParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabSystemParameters.Controls.Add(this.tabFloor);
			this.tabSystemParameters.Controls.Add(this.tabWall);
			this.tabSystemParameters.Controls.Add(this.tabCeiling);
			this.tabSystemParameters.Controls.Add(this.tabDistributor);
			this.tabSystemParameters.Controls.Add(this.tabInsulation);
			this.tabSystemParameters.Controls.Add(this.tabGeneral);
			this.tabSystemParameters.Location = new System.Drawing.Point(0, 27);
			this.tabSystemParameters.Name = "tabSystemParameters";
			this.tabSystemParameters.SelectedIndex = 0;
			this.tabSystemParameters.Size = new System.Drawing.Size(815, 489);
			this.tabSystemParameters.TabIndex = 5;
			// 
			// tabFloor
			// 
			this.tabFloor.Location = new System.Drawing.Point(4, 22);
			this.tabFloor.Name = "tabFloor";
			this.tabFloor.Size = new System.Drawing.Size(807, 463);
			this.tabFloor.TabIndex = 0;
			this.tabFloor.Text = "Fuﬂboden";
			this.tabFloor.UseVisualStyleBackColor = true;
			// 
			// tabWall
			// 
			this.tabWall.Location = new System.Drawing.Point(4, 22);
			this.tabWall.Name = "tabWall";
			this.tabWall.Size = new System.Drawing.Size(807, 463);
			this.tabWall.TabIndex = 1;
			this.tabWall.Text = "Wand";
			this.tabWall.UseVisualStyleBackColor = true;
			// 
			// tabCeiling
			// 
			this.tabCeiling.Location = new System.Drawing.Point(4, 22);
			this.tabCeiling.Name = "tabCeiling";
			this.tabCeiling.Size = new System.Drawing.Size(807, 463);
			this.tabCeiling.TabIndex = 2;
			this.tabCeiling.Text = "Decke";
			this.tabCeiling.UseVisualStyleBackColor = true;
			// 
			// tabDistributor
			// 
			this.tabDistributor.Location = new System.Drawing.Point(4, 22);
			this.tabDistributor.Name = "tabDistributor";
			this.tabDistributor.Size = new System.Drawing.Size(807, 463);
			this.tabDistributor.TabIndex = 3;
			this.tabDistributor.Text = "Verteiler";
			this.tabDistributor.UseVisualStyleBackColor = true;
			// 
			// tabInsulation
			// 
			this.tabInsulation.Location = new System.Drawing.Point(4, 22);
			this.tabInsulation.Name = "tabInsulation";
			this.tabInsulation.Size = new System.Drawing.Size(807, 463);
			this.tabInsulation.TabIndex = 4;
			this.tabInsulation.Text = "D‰mmung";
			this.tabInsulation.UseVisualStyleBackColor = true;
			// 
			// tabGeneral
			// 
			this.tabGeneral.Location = new System.Drawing.Point(4, 22);
			this.tabGeneral.Name = "tabGeneral";
			this.tabGeneral.Size = new System.Drawing.Size(807, 463);
			this.tabGeneral.TabIndex = 5;
			this.tabGeneral.Text = "Allgemein";
			this.tabGeneral.UseVisualStyleBackColor = true;
			// 
			// RequiredMaterialPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabSystemParameters);
			this.Controls.Add(this.label1);
			this.Name = "RequiredMaterialPanel";
			this.Size = new System.Drawing.Size(815, 516);
			this.tabSystemParameters.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabControl tabSystemParameters;
		private System.Windows.Forms.TabPage tabFloor;
		private System.Windows.Forms.TabPage tabWall;
		private System.Windows.Forms.TabPage tabCeiling;
		private System.Windows.Forms.TabPage tabDistributor;
		private System.Windows.Forms.TabPage tabInsulation;
		private System.Windows.Forms.TabPage tabGeneral;
	}
}
