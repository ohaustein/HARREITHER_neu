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
		private string regulatorCircuitId = null;
		private RegulatorCircuit regulatorCircuit = null;
		private int maxCircuits;
		private List<string> additionalFloors;
		private List<Product> plannedConnectedProducts = new List<Product>();

		[NonSerialized]
		private static readonly ILog log = LogManager.GetLogger(typeof(Distributor));

		private TreeNode distributorNode = new TreeNode();

		public Distributor() {
			InitializeDistributor();
		}

		public override string ToString() {
			return this.id + ": " + this.name;
		}

		private void InitializeDistributor() {
			id = "";
			name = "";
			regulatorCircuitId = "";
			maxCircuits = 12;
			additionalFloors = new List<string>();
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
		public List<Floor> AdditionalFloors {
			get {
				List<Floor> floors = new List<Floor>();
				foreach (string floorId in additionalFloors) {
					foreach (Floor floor in Project.Instance.Floors) {
						if (floor.Id == floorId) {
							floors.Add(floor);
							continue;
						}
					}
				}
				return floors;
			}
			set {
				foreach (Floor floor in value) {
					if (!additionalFloors.Contains(floor.Id)) {
						additionalFloors.Add(floor.Id);
					}
				}
			}
		}

		public int MaxCircuits {
			get { return maxCircuits; }
			set { maxCircuits = value; }
		}
		
		internal TreeNode Node {
			get { return this.distributorNode; }
		}

		public List<string> AdditionalFloorIds {
			get { return additionalFloors; }
			set { additionalFloors = value; }
		}

		[XmlIgnore]
		public RegulatorCircuit RegulatorCircuit {
			get {
				if (this.regulatorCircuitId != null) {
					if (Project.Instance != null) {
						foreach (RegulatorCircuit c in Project.Instance.RegulatorCircuits) {
							if (c.Id == regulatorCircuitId) {
								this.regulatorCircuit = c;
								continue;
							}
						}
					}
					this.regulatorCircuitId = null;
				}
				return this.regulatorCircuit;
			}
			set {
				this.regulatorCircuit = value;
				this.regulatorCircuitId = null;
			}
		}

		public string RegulatorCircuitId {
			get {
				if (this.RegulatorCircuit == null) {
					return null;
				}
				return this.regulatorCircuit.Id;
			}
			set { this.regulatorCircuitId = value; }
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

		[XmlIgnore]
		public List<Product> PlannedConnectedProducts {
			get { return this.plannedConnectedProducts; }
		}
	}

}
