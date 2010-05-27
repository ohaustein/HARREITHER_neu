using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Europlan.Common;
using System.Threading;

namespace Europlan.Common {
	public partial class ConstructionEditor : UserControl {

		private ConstructionScopeEnum defaultConstructionScope = ConstructionScopeEnum.FloorConstruction;
		private Construction construction = null;

		public ConstructionEditor() {
			InitializeComponent();
			this.SetLanguage();
			this.UpdateConstructionScopeOfGui();
		}

		public void SetLanguage() {
			this.lblId.Text = EuroplanRes.General_Nummer; //"Nummer:";
			this.lblName.Text = EuroplanRes.General_Bezeichnung; //"Bezeichnung:";
			this.lblThickness.Text = EuroplanRes.ConstructionEditor_Estrichdicke; //"Estrichdicke:";
			this.cbPeFoil.Text = EuroplanRes.ConstructionEditor_PeFolie; //"PE Folie";
			this.lblFactor.Text = EuroplanRes.ConstructionEditor_Faktor; //"Faktor:";
			this.chkHitherm.Text = EuroplanRes.ConstructionEditor_Hitherm; //"Hitherm®";
			this.chkHithermCompact.Text = EuroplanRes.ConstructionEditor_HithermCompact; //"Hitherm® Compact";
			this.colName.HeaderText = EuroplanRes.General_BezeichnungCol; //"Bezeichnung";
			this.colMaterial.HeaderText = EuroplanRes.ConstructionEditor_MaterialCol; //"Material";
			this.colLambdaValue.HeaderText = EuroplanRes.ConstructionEditor_LambdaCol; //"lambda (W/m K)";
			this.colThickness.HeaderText = EuroplanRes.ConstructionEditor_DickeCol; //"d (mm)";
			this.colRValue.HeaderText = EuroplanRes.ConstructionEditor_RWertCol; //"R (m²K/W)";
		}

		public ConstructionScopeEnum ConstructionScope {
			get {
				if (this.construction != null && this.construction.Type != null) {
					return this.construction.Type.Scope;
				} else {
					return this.defaultConstructionScope;
				}
			}
		}

		public ConstructionScopeEnum DefaultConstructionScope {
			get { return this.defaultConstructionScope; }
			set {
				this.defaultConstructionScope = value;
				this.UpdateConstructionScopeOfGui();
			}
		}

		public Construction Construction {
			get { return this.construction; }
			set {
				this.construction = value;
				this.UpdateGui();
			}
		}

		private void UpdateConstructionScopeOfGui() {
			MaterialListWrapper insulations = new MaterialListWrapper(Configuration.ConfigurationType.UserConfiguration);
			insulations.FilterCategory = CategoryType.Insulation;
			//this.colMaterial.DataSource = insulations;
			this.colMaterial.ValueMember = "Material";
			this.colMaterial.DisplayMember = "Name";
			//this.colMaterial.Items.Add(null);
			this.colMaterial.Items.Clear();
			//this.colMaterial.Items.Add(Material.EmptyMaterial);
			this.colMaterial.Items.Add(new MaterialItem("", null));
			foreach (Material m in insulations) {
				this.colMaterial.Items.Add(new MaterialItem(m.Name, m));
			}
			switch (this.ConstructionScope) {
				case ConstructionScopeEnum.FloorConstruction:
					this.cbPeFoil.Visible = false;
					this.lblThickness.Visible = false;
					this.numThickness.Visible = false;
					this.lblFactor.Visible = false;
					this.numFactor.Visible = false;
					this.chkHitherm.Visible = false;
					this.chkHithermCompact.Visible = false;
					break;

				case ConstructionScopeEnum.InsulationConstruction:
					this.cbPeFoil.Visible = true;
					this.lblThickness.Visible = false;
					this.numThickness.Visible = false;
					this.lblFactor.Visible = false;
					this.numFactor.Visible = false;
					this.chkHitherm.Visible = false;
					this.chkHithermCompact.Visible = false;
					break;

				case ConstructionScopeEnum.WallConstruction:
					this.cbPeFoil.Visible = false;
					this.lblThickness.Visible = false;
					this.numThickness.Visible = false;
					this.lblFactor.Visible = true;
					this.numFactor.Visible = Licensing.LicenseManager.Instance.License.IsModuleEnabled(Licensing.AbstractLicensedModule.FeatInternal);
					this.chkHitherm.Visible = true;
					this.chkHithermCompact.Visible = true;
					break;

				case ConstructionScopeEnum.CeilingConstruction:
				default:
					this.cbPeFoil.Visible = false;
					this.lblThickness.Visible = false;
					this.numThickness.Visible = false;
					this.lblFactor.Visible = false;
					this.numFactor.Visible = false;
					this.chkHitherm.Visible = false;
					this.chkHithermCompact.Visible = false;
					break;
			}
		}

		private void UpdateGui() {
			this.UpdateConstructionScopeOfGui();
			switch (this.ConstructionScope) {
				case ConstructionScopeEnum.FloorConstruction:
					FloorConstruction fc = this.construction as FloorConstruction;
					this.numThickness.Value = (fc != null ? (decimal)fc.FloorThickness : (decimal)0); // = (fc != null ? fc.FloorThickness.ToString() : "");
					break;

				case ConstructionScopeEnum.InsulationConstruction:
					InsulationConstruction ic = this.construction as InsulationConstruction;
					this.cbPeFoil.Checked = (ic != null ? ic.PeFoil : false);
					break;

				case ConstructionScopeEnum.WallConstruction:
					WallConstruction wc = this.construction as WallConstruction;
					this.numFactor.Value = (wc != null ? (decimal)wc.Factor : (decimal)1);
					this.chkHitherm.Checked = (wc != null ? wc.IsHithermWall : false);
					this.chkHithermCompact.Checked = (wc != null ? wc.IsHithermCompactWall : false);
					break;

				case ConstructionScopeEnum.CeilingConstruction:
				default:
					break;
			}
			if (this.construction != null) {
				this.txtId.Text = this.construction.Id;
				this.txtName.Text = this.construction.Name;
			} else {
				this.txtId.Text = "";
				this.txtName.Text = "";
				this.numThickness.Value = 0;
				this.cbPeFoil.Checked = false;
			}
			this.constructionLayerBindingSource.DataSource = this.construction == null ? null : this.construction.Layers;
			this.constructionLayerBindingSource.ResetBindings(false);
		}

		private void txtId_TextChanged(object sender, EventArgs e) {
			this.construction.Id = this.txtId.Text;
		}

		private void txtName_TextChanged(object sender, EventArgs e) {
			this.construction.Name = this.txtName.Text;
		}

		private void numThickness_ValueChanged(object sender, EventArgs e) {
			if (this.construction is FloorConstruction) {
				(this.construction as FloorConstruction).FloorThickness = (float)this.numThickness.Value;
			}
		}

		private void numFactor_ValueChanged(object sender, EventArgs e) {
			if (this.construction is WallConstruction) {
				(this.construction as WallConstruction).Factor = (double)this.numFactor.Value;
			}
		}

		private void chkHitherm_CheckedChanged(object sender, EventArgs e) {
			if (this.construction is WallConstruction) {
				(this.construction as WallConstruction).IsHithermWall = this.chkHitherm.Checked;
			}
		}

		private void chkHithermCompact_CheckedChanged(object sender, EventArgs e) {
			if (this.construction is WallConstruction) {
				(this.construction as WallConstruction).IsHithermCompactWall = this.chkHithermCompact.Checked;
			}
		}

		private class MaterialItem {
			private string name;
			private Material material;

			public MaterialItem(string name, Material material) {
				this.name = name;
				this.material = material;
			}

			public string Name {
				get { return this.name; }
			}

			public Material Material {
				get { return this.material; }
			}
		}

		public void ClearSelection() {
			foreach (DataGridViewRow row in this.gridLayers.SelectedRows) {
				row.Selected = false;
			}
		}

		public void Cleanup() {
			if (this.gridLayers.SelectedCells.Count > 0) {
				if (this.gridLayers.SelectedCells[0].OwningRow.DataBoundItem == null) {
					this.gridLayers.CancelEdit();
				} else {
					this.gridLayers.EndEdit();
				}
			}
		}

		public bool ReadOnly {
			set {
				this.gridLayers.ReadOnly = value;
				this.gridLayers.AllowUserToAddRows = !value;
				this.txtId.ReadOnly = value;
				this.txtName.ReadOnly = value;
				this.numThickness.ReadOnly = value;
				this.cbPeFoil.Enabled = !value;
				if (value) {
					this.gridLayers.DefaultCellStyle.ForeColor = SystemColors.GrayText;
					//this.gridLayers.DefaultCellStyle.BackColor = SystemColors.Control;
					this.txtId.BackColor = SystemColors.Window;
					this.txtName.BackColor = SystemColors.Window;
					this.numThickness.BackColor = SystemColors.Window;
					this.txtId.ForeColor = SystemColors.GrayText;
					this.txtName.ForeColor = SystemColors.GrayText;
					this.numThickness.ForeColor = SystemColors.GrayText;
				} else {
					this.gridLayers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
					//this.gridLayers.DefaultCellStyle.BackColor = SystemColors.Window;
					this.txtId.BackColor = SystemColors.Window;
					this.txtName.BackColor = SystemColors.Window;
					this.numThickness.BackColor = SystemColors.Window;
					this.txtId.ForeColor = SystemColors.WindowText;
					this.txtName.ForeColor = SystemColors.WindowText;
					this.numThickness.ForeColor = SystemColors.WindowText;
				}

			}
			get { return this.gridLayers.ReadOnly; }
		}
	}
}
