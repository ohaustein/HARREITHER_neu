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
			this.txtDistributor = new System.Windows.Forms.TextBox();
			this.listDistributors = new System.Windows.Forms.ListView();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtDistributor
			// 
			this.txtDistributor.Location = new System.Drawing.Point(4, 43);
			this.txtDistributor.Name = "txtDistributor";
			this.txtDistributor.Size = new System.Drawing.Size(125, 20);
			this.txtDistributor.TabIndex = 0;
			// 
			// listDistributors
			// 
			this.listDistributors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.listDistributors.FullRowSelect = true;
			this.listDistributors.HideSelection = false;
			this.listDistributors.Location = new System.Drawing.Point(4, 70);
			this.listDistributors.MultiSelect = false;
			this.listDistributors.Name = "listDistributors";
			this.listDistributors.Size = new System.Drawing.Size(125, 285);
			this.listDistributors.TabIndex = 1;
			this.listDistributors.UseCompatibleStateImageBehavior = false;
			this.listDistributors.View = System.Windows.Forms.View.List;
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(135, 41);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(75, 23);
			this.btnAdd.TabIndex = 2;
			this.btnAdd.Text = "+";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnRemove
			// 
			this.btnRemove.Location = new System.Drawing.Point(135, 70);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(75, 23);
			this.btnRemove.TabIndex = 3;
			this.btnRemove.Text = "-";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// QuickDimensioningDistributorsSummary
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnRemove);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.listDistributors);
			this.Controls.Add(this.txtDistributor);
			this.Name = "QuickDimensioningDistributorsSummary";
			this.Size = new System.Drawing.Size(701, 358);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtDistributor;
		private System.Windows.Forms.ListView listDistributors;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnRemove;
	}
}
