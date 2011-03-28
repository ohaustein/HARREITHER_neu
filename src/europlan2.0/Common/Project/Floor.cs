using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Threading;
using WW.Math.Geometry;

namespace Europlan.Common {

	[Serializable()]
	public class Floor : IGuiRepresentation, IClipboard {

		private string name;
		private string id;

		private TreeNode floorNode = new TreeNode();

		private RoomList rooms;
		private DistributorList distributors;
		private string associatedPlanId = null;
		private List<Segment2D> expansionGaps = null;

		private Construction lastInsulationConstruction = null;
		private string lastInsulationConstructionId = null;

		[NonSerialized]
		private static readonly ILog log = LogManager.GetLogger(typeof(Floor));

		public Floor() {
			InitializeFloor();
		}

		public Floor(string name) {
			InitializeFloor();
			this.Name = name;
		}

		public Floor(Floor floor) {
			InitializeFloor();
			string copyOf = EuroplanRes.General_KopieVon;
			this.Name = copyOf + " " + floor.Name;
			foreach (Room room in floor.rooms) {
				this.rooms.Add(new Room(room));
			}
		}

		public override string ToString() {
			return this.Name;
		}

		private void InitializeFloor() {
			name = "";
			id = System.Guid.NewGuid().ToString();
			rooms = new RoomList();
			distributors = new DistributorList();
			this.floorNode.Tag = this;
            this.floorNode.ImageKey = "Geschoﬂ.png";
            this.floorNode.SelectedImageKey = "Geschoﬂ.png";
			this.expansionGaps = new List<Segment2D>();
		}

		internal void Synchronize(Floor floor) {
			this.Name = floor.Name;
			foreach (Room room in floor.Rooms) {
				Room r = this.Rooms.Find(delegate(Room r1) { return r1.Id == room.Id; });
				if (r != null) {
					r.Synchronize(room);
				} else {
					this.Rooms.Add(room);
				}
			}
		}

		public string Name {
			get { return name; }
			set { 
				name = value;
				if (floorNode != null) {
					floorNode.Text = (String.IsNullOrEmpty(name) ? EuroplanRes.Floor_Unbenannt : name);
				}
			}
		}

		public string Id {
			get { return id; }
			set {
				if (value != null && value != "") {
					id = value;
				}
			}
		}

		public RoomList Rooms {
			get {
				return rooms;
			}
			set {
				rooms = value;
			}
		}

		public DistributorList Distributors {
			get {
				return distributors;
			}
			set {
				distributors = value;
			}
		}

		public string AssociatedPlanId {
			get {
				return associatedPlanId;
			}
			set {
				associatedPlanId = value;
			}
		}

		public List<Segment2D> ExpansionGaps {
			get {
				return expansionGaps;
			}
			set {
				expansionGaps = value;
			}
		}

		public DistributorList GetAllAvailableDistributors() {
			DistributorList list = new DistributorList();
			list.AddRange(this.distributors);
			foreach (Floor f in Project.Instance.Floors) {
				foreach (Distributor d in f.Distributors) {
					if (d.AdditionalFloors.Contains(this) && !list.Contains(d)) {
						list.Add(d);
					}
				}
			}
			return list;
		}

		/*internal void InitializeTree(System.Windows.Forms.TreeNode floors) {
			floorNode.Tag = this;
			floors.Nodes.Add(floorNode);
			floorNode.Nodes.Clear();
			foreach (Room room in rooms) {
				room.InitializeTree(floorNode);
			}
		}*/

		internal void UpdateTree() {
			int i = 0;
			bool expand = this.Node.Nodes.Count == 0;
			
			foreach (Distributor distributor in this.Distributors) {
				int index = this.Node.Nodes.IndexOf(distributor.Node);
				if (index < 0) {
					this.Node.Nodes.Insert(i, distributor.Node);
				} else if (index > i) {
					if (distributor.Node.IsSelected) {
						for (int j = i; j < index; j++) {
							this.Node.Nodes.RemoveAt(i);
						}
					} else {
						this.Node.Nodes.RemoveAt(index);
						this.Node.Nodes.Insert(i, distributor.Node);
					}
				}
				i++;
			}

			foreach (Room room in this.Rooms) {
				int index = this.Node.Nodes.IndexOf(room.Node);
				if (index < 0) {
					this.Node.Nodes.Insert(i, room.Node);
				} else if (index > i) {
					if (room.Node.IsSelected) {
						for (int j = i; j < index; j++) {
							this.Node.Nodes.RemoveAt(i);
						}
					} else {
						this.Node.Nodes.RemoveAt(index);
						this.Node.Nodes.Insert(i, room.Node);
					}
				}
				room.UpdateTree();
				i++;
			}
			while (this.Node.Nodes.Count > i) {
				this.Node.Nodes.RemoveAt(i);
			}
			if (expand) {
				this.Node.Expand();
			}
		}

		internal TreeNode Node {
			get { return this.floorNode; }
		}

		public Type AssociatedPanelType {
			get { 
				return typeof(FloorSummaryPanel);
			}
		}

		public TreeNode FindNode(object element) {
			if (element == this) {
				return floorNode;
			} else {
				foreach (Room r in rooms) {
					TreeNode node = r.FindNode(element);
					if (node != null) {
						return node;
					}
				}
				foreach (Distributor d in distributors) {
					TreeNode node = d.FindNode(element);
					if (node != null) {
						return node;
					}
				}
			}
			return null;
		}

		public bool SupportsCut {
			get { return false; }
		}

		public bool SupportsCopy {
			get { return true; }
		}

		public string DataFormat {
			get { return this.GetType().ToString(); }
		}

		public bool SupportsPaste(string data) {
			if (data == typeof(Room).ToString()) {
				return true;
			}
			return false;
		}

		public string SupportedPasteFormat {
			get { return typeof(Room).ToString(); }
		}

		public object Copy() {
			return new Floor(this);
		}

		public void Paste(object o) {
			if (o.GetType() == typeof(Room)) {
				rooms.Add(o as Room);
			} else if (o.GetType() == typeof(Distributor)) {
				distributors.Add(o as Distributor);
			} else {
				throw new Exception("Paste of this type not supported");
			}			
		}


		internal void FinalizeLoading() {
			foreach (Room room in this.rooms) {
				room.FinalizeLoading();
			}
		}

		public List<PlannedProduct> FindConnectedProduct(PlannedProduct origin) {
			List<PlannedProduct> connectedProducts = new List<PlannedProduct>();
			foreach (Room r in this.Rooms) {
				foreach (PlannedProduct pp in r.PlannedProducts) {
					if (pp.Product.PlannedConnection != null && pp.Product.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT && pp.Product.PlannedConnection.OtherProduct == origin) {
						connectedProducts.Add(pp);
						//return pp;
					}
				}
			}
			return connectedProducts;
			//return null;
		}

		public string LastInsulationConstructionId {
			get {
				if (this.lastInsulationConstruction == null) {
					return this.lastInsulationConstructionId;
				}
				return this.lastInsulationConstruction.Id;
			}
			set {
				if (this.LastInsulationConstructionId != value) {
					this.lastInsulationConstruction = null;
					this.lastInsulationConstructionId = value;
				}
			}
		}

		[XmlIgnore]
		public Construction LastInsulationConstruction {
			get {
				if (this.lastInsulationConstructionId != null) {
					this.lastInsulationConstruction = Project.Instance.Config.GetConstruction(this.lastInsulationConstructionId);
					this.lastInsulationConstructionId = null;
				}
				if (lastInsulationConstruction == null) {
					int i = 0;
					while (i < Project.Instance.Config.Constructions.Count && this.lastInsulationConstruction == null) {
						if (Project.Instance.Config.Constructions[i].Type.Scope == ConstructionScopeEnum.InsulationConstruction) {
							this.lastInsulationConstruction = Project.Instance.Config.Constructions[i];
						}
						i++;
					}
				}
				return this.lastInsulationConstruction;
			}
			set { this.lastInsulationConstruction = value; }
		}
	}

}

