namespace Europlan.Common {
	partial class QuickDimensioningDistributorsSummary {
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
			this.cmbDistributors = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.lblRemainingLabel = new System.Windows.Forms.Label();
			this.lblPlanned = new System.Windows.Forms.Label();
			this.distributorGrid = new Europlan.Common.QuickDimensioningDistributorsGrid();
			this.SuspendLayout();
			// 
			// cmbDistributors
			// 
			this.cmbDistributors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbDistributors.FormattingEnabled = true;
			this.cmbDistributors.Location = new System.Drawing.Point(82, 2);
			this.cmbDistributors.Name = "cmbDistributors";
			this.cmbDistributors.Size = new System.Drawing.Size(173, 21);
			this.cmbDistributors.TabIndex = 6;
			this.cmbDistributors.SelectedIndexChanged += new System.EventHandler(this.cmbDistributors_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(73, 23);
			this.label1.TabIndex = 7;
			this.label1.Text = "Verteiler:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(261, 1);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(176, 23);
			this.button1.TabIndex = 8;
			this.button1.Text = "Verteiler anlegen/löschen";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.btnNewDistributor_Click);
			// 
			// lblRemainingLabel
			// 
			this.lblRemainingLabel.Location = new System.Drawing.Point(3, 26);
			this.lblRemainingLabel.Name = "lblRemainingLabel";
			this.lblRemainingLabel.Size = new System.Drawing.Size(118, 23);
			this.lblRemainingLabel.TabIndex = 9;
			this.lblRemainingLabel.Text = "Verplante Anschlüße:";
			this.lblRemainingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblPlanned
			// 
			this.lblPlanned.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblPlanned.Location = new System.Drawing.Point(127, 26);
			this.lblPlanned.Name = "lblPlanned";
			this.lblPlanned.Size = new System.Drawing.Size(571, 23);
			this.lblPlanned.TabIndex = 10;
			this.lblPlanned.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// distributorGrid
			// 
			this.distributorGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.distributorGrid.ConcreteActivation = true;
			this.distributorGrid.Distributor = null;
			this.distributorGrid.Euroval = true;
			this.distributorGrid.Hitherm = true;
			this.distributorGrid.HithermCompact = true;
			this.distributorGrid.Location = new System.Drawing.Point(3, 52);
			this.distributorGrid.ModulKlimaBoden = true;
			this.distributorGrid.ModulKlimaDecke = true;
			this.distributorGrid.Name = "distributorGrid";
			this.distributorGrid.Size = new System.Drawing.Size(695, 303);
			this.distributorGrid.TabIndex = 5;
			// 
			// QuickDimensioningDistributorsSummary
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblPlanned);
			this.Controls.Add(this.lblRemainingLabel);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmbDistributors);
			this.Controls.Add(this.distributorGrid);
			this.Name = "QuickDimensioningDistributorsSummary";
			this.Size = new System.Drawing.Size(701, 358);
			this.ResumeLayout(false);

		}

		#endregion

		private QuickDimensioningDistributorsGrid distributorGrid;
		private System.Windows.Forms.ComboBox cmbDistributors;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label lblRemainingLabel;
		private System.Windows.Forms.Label lblPlanned;
	}
}
