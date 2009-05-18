namespace Europlan.AdminApplication.ContructionEditor {
	partial class MaterialEditorGrid {
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.materialsWrapperBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.panel1 = new System.Windows.Forms.Panel();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.partNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.denominationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.unitDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.priceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.typeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.userDefinedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.cbEuroval = new System.Windows.Forms.CheckBox();
			this.cbModul = new System.Windows.Forms.CheckBox();
			this.cbHitherm = new System.Windows.Forms.CheckBox();
			this.cbDistributor = new System.Windows.Forms.CheckBox();
			this.cbInsulation = new System.Windows.Forms.CheckBox();
			this.cbGeneral = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.materialsWrapperBindingSource)).BeginInit();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// materialListWrapperBindingSource
			// 
			this.materialsWrapperBindingSource.DataSource = typeof(Europlan.Common.MaterialListWrapper);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.cbGeneral);
			this.panel1.Controls.Add(this.cbInsulation);
			this.panel1.Controls.Add(this.cbDistributor);
			this.panel1.Controls.Add(this.cbHitherm);
			this.panel1.Controls.Add(this.cbModul);
			this.panel1.Controls.Add(this.cbEuroval);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 413);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(612, 27);
			this.panel1.TabIndex = 1;
			// 
			// dataGridView1
			// 
			this.dataGridView1.AutoGenerateColumns = false;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.partNumberDataGridViewTextBoxColumn,
            this.denominationDataGridViewTextBoxColumn,
            this.unitDataGridViewTextBoxColumn,
            this.priceDataGridViewTextBoxColumn,
            this.typeDataGridViewTextBoxColumn,
            this.userDefinedDataGridViewCheckBoxColumn});
			this.dataGridView1.DataSource = this.materialsWrapperBindingSource;
			this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridView1.Location = new System.Drawing.Point(0, 0);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(612, 413);
			this.dataGridView1.TabIndex = 2;
			// 
			// idDataGridViewTextBoxColumn
			// 
			this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
			this.idDataGridViewTextBoxColumn.FillWeight = 50F;
			this.idDataGridViewTextBoxColumn.HeaderText = "Nr.";
			this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
			this.idDataGridViewTextBoxColumn.Width = 50;
			// 
			// nameDataGridViewTextBoxColumn
			// 
			this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
			this.nameDataGridViewTextBoxColumn.FillWeight = 150F;
			this.nameDataGridViewTextBoxColumn.HeaderText = "Bezeichnung";
			this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
			this.nameDataGridViewTextBoxColumn.Width = 150;
			// 
			// partNumberDataGridViewTextBoxColumn
			// 
			this.partNumberDataGridViewTextBoxColumn.DataPropertyName = "PartNumber";
			this.partNumberDataGridViewTextBoxColumn.FillWeight = 70F;
			this.partNumberDataGridViewTextBoxColumn.HeaderText = "Bestellnr.";
			this.partNumberDataGridViewTextBoxColumn.Name = "partNumberDataGridViewTextBoxColumn";
			this.partNumberDataGridViewTextBoxColumn.Width = 70;
			// 
			// denominationDataGridViewTextBoxColumn
			// 
			this.denominationDataGridViewTextBoxColumn.DataPropertyName = "Denomination";
			this.denominationDataGridViewTextBoxColumn.FillWeight = 75F;
			this.denominationDataGridViewTextBoxColumn.HeaderText = "Verpackungs- einheit";
			this.denominationDataGridViewTextBoxColumn.Name = "denominationDataGridViewTextBoxColumn";
			this.denominationDataGridViewTextBoxColumn.Width = 75;
			// 
			// unitDataGridViewTextBoxColumn
			// 
			this.unitDataGridViewTextBoxColumn.DataPropertyName = "Unit";
			this.unitDataGridViewTextBoxColumn.FillWeight = 50F;
			this.unitDataGridViewTextBoxColumn.HeaderText = "Einheit";
			this.unitDataGridViewTextBoxColumn.Name = "unitDataGridViewTextBoxColumn";
			this.unitDataGridViewTextBoxColumn.Width = 50;
			// 
			// priceDataGridViewTextBoxColumn
			// 
			this.priceDataGridViewTextBoxColumn.DataPropertyName = "Price";
			this.priceDataGridViewTextBoxColumn.FillWeight = 60F;
			this.priceDataGridViewTextBoxColumn.HeaderText = "Preis pro Einheit";
			this.priceDataGridViewTextBoxColumn.Name = "priceDataGridViewTextBoxColumn";
			this.priceDataGridViewTextBoxColumn.Width = 60;
			// 
			// typeDataGridViewTextBoxColumn
			// 
			this.typeDataGridViewTextBoxColumn.DataPropertyName = "Type";
			this.typeDataGridViewTextBoxColumn.HeaderText = "Type";
			this.typeDataGridViewTextBoxColumn.Name = "typeDataGridViewTextBoxColumn";
			this.typeDataGridViewTextBoxColumn.ReadOnly = true;
			this.typeDataGridViewTextBoxColumn.Visible = false;
			// 
			// userDefinedDataGridViewCheckBoxColumn
			// 
			this.userDefinedDataGridViewCheckBoxColumn.DataPropertyName = "UserDefined";
			this.userDefinedDataGridViewCheckBoxColumn.HeaderText = "UserDefined";
			this.userDefinedDataGridViewCheckBoxColumn.Name = "userDefinedDataGridViewCheckBoxColumn";
			this.userDefinedDataGridViewCheckBoxColumn.ReadOnly = true;
			this.userDefinedDataGridViewCheckBoxColumn.Visible = false;
			// 
			// cbEuroval
			// 
			this.cbEuroval.Appearance = System.Windows.Forms.Appearance.Button;
			this.cbEuroval.Checked = true;
			this.cbEuroval.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbEuroval.Location = new System.Drawing.Point(0, 3);
			this.cbEuroval.Name = "cbEuroval";
			this.cbEuroval.Size = new System.Drawing.Size(70, 24);
			this.cbEuroval.TabIndex = 3;
			this.cbEuroval.Text = "Euroval";
			this.cbEuroval.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbEuroval.UseVisualStyleBackColor = true;
			// 
			// cbModul
			// 
			this.cbModul.Appearance = System.Windows.Forms.Appearance.Button;
			this.cbModul.Checked = true;
			this.cbModul.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbModul.Location = new System.Drawing.Point(76, 3);
			this.cbModul.Name = "cbModul";
			this.cbModul.Size = new System.Drawing.Size(70, 24);
			this.cbModul.TabIndex = 4;
			this.cbModul.Text = "Modul";
			this.cbModul.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbModul.UseVisualStyleBackColor = true;
			// 
			// cbHitherm
			// 
			this.cbHitherm.Appearance = System.Windows.Forms.Appearance.Button;
			this.cbHitherm.Checked = true;
			this.cbHitherm.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbHitherm.Location = new System.Drawing.Point(152, 3);
			this.cbHitherm.Name = "cbHitherm";
			this.cbHitherm.Size = new System.Drawing.Size(70, 24);
			this.cbHitherm.TabIndex = 5;
			this.cbHitherm.Text = "Hitherm";
			this.cbHitherm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbHitherm.UseVisualStyleBackColor = true;
			// 
			// cbDistributor
			// 
			this.cbDistributor.Appearance = System.Windows.Forms.Appearance.Button;
			this.cbDistributor.Checked = true;
			this.cbDistributor.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbDistributor.Location = new System.Drawing.Point(228, 3);
			this.cbDistributor.Name = "cbDistributor";
			this.cbDistributor.Size = new System.Drawing.Size(70, 24);
			this.cbDistributor.TabIndex = 6;
			this.cbDistributor.Text = "Verteiler";
			this.cbDistributor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbDistributor.UseVisualStyleBackColor = true;
			// 
			// cbInsulation
			// 
			this.cbInsulation.Appearance = System.Windows.Forms.Appearance.Button;
			this.cbInsulation.Checked = true;
			this.cbInsulation.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbInsulation.Location = new System.Drawing.Point(304, 3);
			this.cbInsulation.Name = "cbInsulation";
			this.cbInsulation.Size = new System.Drawing.Size(70, 24);
			this.cbInsulation.TabIndex = 7;
			this.cbInsulation.Text = "Dämmung";
			this.cbInsulation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbInsulation.UseVisualStyleBackColor = true;
			// 
			// cbGeneral
			// 
			this.cbGeneral.Appearance = System.Windows.Forms.Appearance.Button;
			this.cbGeneral.Checked = true;
			this.cbGeneral.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbGeneral.Location = new System.Drawing.Point(380, 3);
			this.cbGeneral.Name = "cbGeneral";
			this.cbGeneral.Size = new System.Drawing.Size(70, 24);
			this.cbGeneral.TabIndex = 8;
			this.cbGeneral.Text = "Allgemein";
			this.cbGeneral.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbGeneral.UseVisualStyleBackColor = true;
			// 
			// MaterialEditorGrid
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.panel1);
			this.Name = "MaterialEditorGrid";
			this.Size = new System.Drawing.Size(612, 440);
			((System.ComponentModel.ISupportInitialize)(this.materialsWrapperBindingSource)).EndInit();
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.BindingSource materialsWrapperBindingSource;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn partNumberDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn denominationDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn unitDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn priceDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn typeDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn userDefinedDataGridViewCheckBoxColumn;
		private System.Windows.Forms.CheckBox cbEuroval;
		private System.Windows.Forms.CheckBox cbModul;
		private System.Windows.Forms.CheckBox cbHitherm;
		private System.Windows.Forms.CheckBox cbInsulation;
		private System.Windows.Forms.CheckBox cbDistributor;
		private System.Windows.Forms.CheckBox cbGeneral;

	}
}
