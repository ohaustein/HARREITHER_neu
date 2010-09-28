using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WW.Cad.Model;
using WW.Cad.Model.Tables;

namespace Europlan.Common {
	public partial class CadPanelOptions : UserControl {
		private class LayerListViewItem : ListViewItem {

			private DxfLayer layer = null;

			public LayerListViewItem(DxfLayer layer)
				: base(layer.Name) {
				this.layer = layer;
				this.Checked = layer.Enabled;
			}

			public DxfLayer Layer {
				get { return this.layer; }
				set { this.layer = value; }
			}
		}

		public CadPanelOptions() {
			InitializeComponent();
			this.ParentChanged += new EventHandler(ParentChangedHandler);
		}

		#region dirty hack to prevent itemCheckedEvents when adding the items to the listview
		private void ParentChangedHandler(object sender, EventArgs e) {
			if (sender is Control) {
				(sender as Control).ParentChanged -= new EventHandler(ParentChangedHandler);
				Control parent = (sender as Control).Parent;
				while (parent.Parent != null) {
					parent = parent.Parent;
				}
				if (parent is Form) {
					(parent as Form).Load += new EventHandler(LoadHandler);
				} else {
					parent.ParentChanged += new EventHandler(ParentChangedHandler);
				}
			}
		}

		private void LoadHandler(object sender, EventArgs e) {
			this.lstLayers.ItemChecked += new ItemCheckedEventHandler(lstLayers_ItemChecked);
		}
		#endregion

		private CadPlan plan = null;
		private bool ignoreCheck = true;

		public Plan Plan {
			get { return this.plan; }
			set {
				if (this.plan != value && (value == null || value is CadPlan)) {
					this.plan = value as CadPlan;
					if (this.plan != null) {
						DxfModel model = this.plan.LoadModel();
						this.lstLayers.BeginUpdate();
						this.lstLayers.Items.Clear();
						foreach (DxfLayer layer in model.Layers) {
							layer.Enabled = !this.plan.DisabledLayers.Contains(layer.Name);
							this.lstLayers.Items.Add(new LayerListViewItem(layer));
						}
						this.lstLayers.EndUpdate();
					} else {
						this.lstLayers.BeginUpdate();
						this.lstLayers.Items.Clear();
						this.lstLayers.EndUpdate();
					}
				}
			}
		}

		public event EventHandler InvalidateNeeded;

		private void lstLayers_ItemChecked(object sender, ItemCheckedEventArgs e) {
			LayerListViewItem item = e.Item as LayerListViewItem;
			if (item != null) {
				item.Layer.Enabled = item.Checked;
				if (this.InvalidateNeeded != null) {
					this.InvalidateNeeded(this, EventArgs.Empty);
				}
			}
		}

	}
}
