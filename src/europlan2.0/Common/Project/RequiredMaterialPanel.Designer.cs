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
			this.requiredMaterialGridFloor = new Europlan.Common.RequiredMaterialGrid();
			this.tabWall = new System.Windows.Forms.TabPage();
			this.requiredMaterialGridWall = new Europlan.Common.RequiredMaterialGrid();
			this.tabCeiling = new System.Windows.Forms.TabPage();
			this.requiredMaterialGridCeiling = new Europlan.Common.RequiredMaterialGrid();
			this.tabDistributor = new System.Windows.Forms.TabPage();
			this.requiredMaterialGridDistributor = new Europlan.Common.RequiredMaterialGrid();
			this.tabInsulation = new System.Windows.Forms.TabPage();
			this.requiredMaterialGridInsulation = new Europlan.Common.RequiredMaterialGrid();
			this.tabGeneral = new System.Windows.Forms.TabPage();
			this.requiredMaterialGridGeneral = new Europlan.Common.RequiredMaterialGrid();
			this.helpProvider = new System.Windows.Forms.HelpProvider();
			this.tabSystemParameters.SuspendLayout();
			this.tabFloor.SuspendLayout();
			this.tabWall.SuspendLayout();
			this.tabCeiling.SuspendLayout();
			this.tabDistributor.SuspendLayout();
			this.tabInsulation.SuspendLayout();
			this.tabGeneral.SuspendLayout();
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
			this.helpProvider.SetHelpKeyword(this.tabSystemParameters, "html\\euro0pgj.htm");
			this.helpProvider.SetHelpNavigator(this.tabSystemParameters, System.Windows.Forms.HelpNavigator.Topic);
			this.tabSystemParameters.Location = new System.Drawing.Point(0, 27);
			this.tabSystemParameters.Name = "tabSystemParameters";
			this.tabSystemParameters.SelectedIndex = 0;
			this.helpProvider.SetShowHelp(this.tabSystemParameters, true);
			this.tabSystemParameters.Size = new System.Drawing.Size(815, 489);
			this.tabSystemParameters.TabIndex = 5;
			// 
			// tabFloor
			// 
			this.tabFloor.Controls.Add(this.requiredMaterialGridFloor);
			this.tabFloor.Location = new System.Drawing.Point(4, 22);
			this.tabFloor.Name = "tabFloor";
			this.tabFloor.Size = new System.Drawing.Size(807, 463);
			this.tabFloor.TabIndex = 0;
			this.tabFloor.Text = "Fuﬂboden";
			this.tabFloor.UseVisualStyleBackColor = true;
			// 
			// requiredMaterialGridFloor
			// 
			this.requiredMaterialGridFloor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.requiredMaterialGridFloor.Location = new System.Drawing.Point(0, 0);
			this.requiredMaterialGridFloor.Name = "requiredMaterialGridFloor";
			this.requiredMaterialGridFloor.Size = new System.Drawing.Size(807, 463);
			this.requiredMaterialGridFloor.TabIndex = 0;
			this.requiredMaterialGridFloor.GridContentChanged += new Europlan.Common.RequiredMaterialGridContentChangedHandler(this.requiredMaterialGrid_GridContentChanged);
			// 
			// tabWall
			// 
			this.tabWall.Controls.Add(this.requiredMaterialGridWall);
			this.tabWall.Location = new System.Drawing.Point(4, 22);
			this.tabWall.Name = "tabWall";
			this.tabWall.Size = new System.Drawing.Size(807, 463);
			this.tabWall.TabIndex = 1;
			this.tabWall.Text = "Wand";
			this.tabWall.UseVisualStyleBackColor = true;
			// 
			// requiredMaterialGridWall
			// 
			this.requiredMaterialGridWall.Dock = System.Windows.Forms.DockStyle.Fill;
			this.requiredMaterialGridWall.Location = new System.Drawing.Point(0, 0);
			this.requiredMaterialGridWall.Name = "requiredMaterialGridWall";
			this.requiredMaterialGridWall.Size = new System.Drawing.Size(807, 463);
			this.requiredMaterialGridWall.TabIndex = 1;
			this.requiredMaterialGridWall.GridContentChanged += new Europlan.Common.RequiredMaterialGridContentChangedHandler(this.requiredMaterialGrid_GridContentChanged);
			// 
			// tabCeiling
			// 
			this.tabCeiling.Controls.Add(this.requiredMaterialGridCeiling);
			this.tabCeiling.Location = new System.Drawing.Point(4, 22);
			this.tabCeiling.Name = "tabCeiling";
			this.tabCeiling.Size = new System.Drawing.Size(807, 463);
			this.tabCeiling.TabIndex = 2;
			this.tabCeiling.Text = "Decke";
			this.tabCeiling.UseVisualStyleBackColor = true;
			// 
			// requiredMaterialGridCeiling
			// 
			this.requiredMaterialGridCeiling.Dock = System.Windows.Forms.DockStyle.Fill;
			this.requiredMaterialGridCeiling.Location = new System.Drawing.Point(0, 0);
			this.requiredMaterialGridCeiling.Name = "requiredMaterialGridCeiling";
			this.requiredMaterialGridCeiling.Size = new System.Drawing.Size(807, 463);
			this.requiredMaterialGridCeiling.TabIndex = 1;
			this.requiredMaterialGridCeiling.GridContentChanged += new Europlan.Common.RequiredMaterialGridContentChangedHandler(this.requiredMaterialGrid_GridContentChanged);
			// 
			// tabDistributor
			// 
			this.tabDistributor.Controls.Add(this.requiredMaterialGridDistributor);
			this.tabDistributor.Location = new System.Drawing.Point(4, 22);
			this.tabDistributor.Name = "tabDistributor";
			this.tabDistributor.Size = new System.Drawing.Size(807, 463);
			this.tabDistributor.TabIndex = 3;
			this.tabDistributor.Text = "Verteiler";
			this.tabDistributor.UseVisualStyleBackColor = true;
			// 
			// requiredMaterialGridDistributor
			// 
			this.requiredMaterialGridDistributor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.requiredMaterialGridDistributor.Location = new System.Drawing.Point(0, 0);
			this.requiredMaterialGridDistributor.Name = "requiredMaterialGridDistributor";
			this.requiredMaterialGridDistributor.Size = new System.Drawing.Size(807, 463);
			this.requiredMaterialGridDistributor.TabIndex = 1;
			this.requiredMaterialGridDistributor.GridContentChanged += new Europlan.Common.RequiredMaterialGridContentChangedHandler(this.requiredMaterialGrid_GridContentChanged);
			// 
			// tabInsulation
			// 
			this.tabInsulation.Controls.Add(this.requiredMaterialGridInsulation);
			this.tabInsulation.Location = new System.Drawing.Point(4, 22);
			this.tabInsulation.Name = "tabInsulation";
			this.tabInsulation.Size = new System.Drawing.Size(807, 463);
			this.tabInsulation.TabIndex = 4;
			this.tabInsulation.Text = "D‰mmung";
			this.tabInsulation.UseVisualStyleBackColor = true;
			// 
			// requiredMaterialGridInsulation
			// 
			this.requiredMaterialGridInsulation.Dock = System.Windows.Forms.DockStyle.Fill;
			this.requiredMaterialGridInsulation.Location = new System.Drawing.Point(0, 0);
			this.requiredMaterialGridInsulation.Name = "requiredMaterialGridInsulation";
			this.requiredMaterialGridInsulation.Size = new System.Drawing.Size(807, 463);
			this.requiredMaterialGridInsulation.TabIndex = 1;
			this.requiredMaterialGridInsulation.GridContentChanged += new Europlan.Common.RequiredMaterialGridContentChangedHandler(this.requiredMaterialGrid_GridContentChanged);
			// 
			// tabGeneral
			// 
			this.tabGeneral.Controls.Add(this.requiredMaterialGridGeneral);
			this.tabGeneral.Location = new System.Drawing.Point(4, 22);
			this.tabGeneral.Name = "tabGeneral";
			this.tabGeneral.Size = new System.Drawing.Size(807, 463);
			this.tabGeneral.TabIndex = 5;
			this.tabGeneral.Text = "Allgemein";
			this.tabGeneral.UseVisualStyleBackColor = true;
			// 
			// requiredMaterialGridGeneral
			// 
			this.requiredMaterialGridGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
			this.requiredMaterialGridGeneral.Location = new System.Drawing.Point(0, 0);
			this.requiredMaterialGridGeneral.Name = "requiredMaterialGridGeneral";
			this.requiredMaterialGridGeneral.Size = new System.Drawing.Size(807, 463);
			this.requiredMaterialGridGeneral.TabIndex = 1;
			this.requiredMaterialGridGeneral.GridContentChanged += new Europlan.Common.RequiredMaterialGridContentChangedHandler(this.requiredMaterialGrid_GridContentChanged);
			// 
			// helpProvider
			// 
			this.helpProvider.HelpNamespace = "europlan.chm";
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
			this.tabFloor.ResumeLayout(false);
			this.tabWall.ResumeLayout(false);
			this.tabCeiling.ResumeLayout(false);
			this.tabDistributor.ResumeLayout(false);
			this.tabInsulation.ResumeLayout(false);
			this.tabGeneral.ResumeLayout(false);
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
		private RequiredMaterialGrid requiredMaterialGridFloor;
		private RequiredMaterialGrid requiredMaterialGridWall;
		private RequiredMaterialGrid requiredMaterialGridCeiling;
		private RequiredMaterialGrid requiredMaterialGridDistributor;
		private RequiredMaterialGrid requiredMaterialGridInsulation;
		private RequiredMaterialGrid requiredMaterialGridGeneral;
		private System.Windows.Forms.HelpProvider helpProvider;
	}
}
