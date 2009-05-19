using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Europlan.Common;

namespace Europlan.AdminApplication {
	public partial class ConstructionEditor : UserControl {

		private ConstructionScopeEnum defaultConstructionScope = ConstructionScopeEnum.FloorConstruction;
		private Construction construction = null;

		public ConstructionEditor() {
			InitializeComponent();
			this.UpdateConstructionScopeOfGui();
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
			MaterialListWrapper insulations = new MaterialListWrapper();
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
					this.lblThickness.Visible = true;
					this.numThickness.Visible = true;
					break;

				case ConstructionScopeEnum.InsulationConstruction:
					this.cbPeFoil.Visible = true;
					this.lblThickness.Visible = false;
					this.numThickness.Visible = false;
					break;

				case ConstructionScopeEnum.CeilingConstruction:
				default:
					this.cbPeFoil.Visible = false;
					this.lblThickness.Visible = false;
					this.numThickness.Visible = false;
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
					InsulationConstruction wc = this.construction as InsulationConstruction;
					this.cbPeFoil.Checked = (wc != null ? wc.PeFoil : false);
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
			this.constructionBindingSource.DataSource = this.construction;
			this.constructionBindingSource.ResetBindings(false);
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
			foreach (DataGridViewRow row in this.dataGridView1.SelectedRows) {
				row.Selected = false;
			}
		}
	}
}
