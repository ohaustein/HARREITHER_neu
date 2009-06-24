using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Europlan.Common {

	[Serializable()]
	public class Distributor : IGuiRepresentation {

		private string id;
		private string name;
		private string regulatorCircuitId;

		[NonSerialized]
		private static readonly ILog log = LogManager.GetLogger(typeof(Distributor));

		private TreeNode distributorNode = new TreeNode();

		public Distributor() {
			InitializeDistributor();
		}

		private void InitializeDistributor() {
			id = "";
			name = "";
			regulatorCircuitId = "";
			distributorNode.Tag = this;
		}

		public string Id {
			get { return id; }
			set {
				id = value;
				if (distributorNode != null) {
					distributorNode.Text = (String.IsNullOrEmpty(name) ? "unbenannt" : id + ": " + name);
				}
			}
		}

		public string Name {
			get { return name; }
			set {
				name = value;
				if (distributorNode != null) {
					distributorNode.Text = (String.IsNullOrEmpty(name) ? "unbenannt" : id + ": " + name);
				}
			}
		}

		[XmlIgnore]
		public RegulatorCircuit RegulatorCircuit {
			get {
				RegulatorCircuit circuit = null;
				if (Project.Instance != null) {
					foreach (RegulatorCircuit c in Project.Instance.RegulatorCircuits) {
						if (c.Id == regulatorCircuitId) {
							circuit = c;
							continue;
						}
					}
				}
				return circuit;
			}
			set { regulatorCircuitId = value.Id; }
		}

		public string RegulatorCircuitId {
			get { return regulatorCircuitId; }
			set { regulatorCircuitId = value; }
		}

		public TreeNode FindNode(object element) {
			if (element == this) {
				return distributorNode;
			}
			return null;
		}

		public Type AssociatedPanelType {
			get {
				return typeof(DistributorPanel);
			}
		}

		public System.Drawing.Icon AssociatedIcon {
			get {
				return null;
			}
		}

	}

}
