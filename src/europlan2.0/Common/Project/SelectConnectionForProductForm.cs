using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class SelectConnectionForProductForm : Form {

		private TreeNode rootNode;
		private Dictionary<object, TreeNode> nodes = new Dictionary<object, TreeNode>();

		public SelectConnectionForProductForm(Floor floor) {
			InitializeComponent();
			this.tvDistributors.Nodes.Clear();
			DistributorList distributors = floor.GetAllAvailableDistributors();
			TreeNode[] distributorNodes = new TreeNode[distributors.Count];
			int i = 0;
			foreach (Distributor d in distributors) {
				distributorNodes[i] = new TreeNode(d.Id + ": " + d.Name);
				distributorNodes[i].Tag = d;
				this.nodes.Add(d, distributorNodes[i]);
				i++;
			}
			rootNode = new TreeNode("Projekt", distributorNodes);
			this.tvDistributors.Nodes.Add(rootNode);
			this.tvDistributors.ExpandAll();
		}

		private void tvDistributors_AfterSelect(object sender, TreeViewEventArgs e) {
			this.btnOk.Enabled = tvDistributors.SelectedNode != null && tvDistributors.SelectedNode.Tag != null;
		}

		public ProductConnection SelectedConnection {
			get {
				if (tvDistributors.SelectedNode == null) {
					return null;
				}
				if (tvDistributors.SelectedNode.Tag is Distributor) {
					return new ProductConnection(tvDistributors.SelectedNode.Tag as Distributor, (Distributor)null);
				}
				if (tvDistributors.SelectedNode.Tag is Product) {
					return new ProductConnection(tvDistributors.SelectedNode.Tag as PlannedProduct, (Distributor)null);
				}
				return null;
			}
			set {
				if (value == null) {
					this.tvDistributors.SelectedNode = null;
				} else {
					object sel = value.Connection;
					if (sel != null && this.nodes.ContainsKey(sel)) {
						this.tvDistributors.SelectedNode = this.nodes[sel];
					} else {
						this.tvDistributors.SelectedNode = null;
					}
				}
			}
		}
	}
}