using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.Windows.Forms;
using System.Threading;
using Europlan.Common;
using System.Xml.Serialization;

namespace Europlan.Common {

	[Serializable()]
	public class Room : IGuiRepresentation, IClipboard {

		public enum RoomController {
			None,
			RC,
			RCF,
			RCRadio,
			RF
		}

		private string id;
		private string internalId;
		private string name;
		private int roomHeatTemperature;
		private int roomCoolTemperature;
		private int roomRelativeHumidity;
		private float area;
		private int heatLoad;
		private int coolLoad;
		private int floorHeatingLoss;
		private int quickDimensioningRoomTemperature;
		private int quickDimensioningHeatLoad;
		private int quickDimensioningCoolLoad;
		private int quickDimensioningNrOfServos;
		private RoomController quickDimensioningRoomController = RoomController.None;
		private string quickDimensioningComments;
		private int additionalHeatLoad;
		private string quickDimensioningRoomTypeId;
		private bool quickDimensioningInitialized = false;

		private List<Product> usedProductsForQuickDimensioning;
		private List<PlannedProduct> plannedProducts;

		private TreeNode roomNode = new TreeNode();

		[NonSerialized]
		private static readonly ILog log = LogManager.GetLogger(typeof(Room));

		[NonSerialized]
		private System.ComponentModel.ComponentResourceManager resources = ResourcesManager.resources;

		public Room() {
			InitializeRoom();
			this.Name = "";
		}

		public Room(string name) {
			InitializeRoom();
			this.Name = name;
		}

		public Room(Room room) {
			InitializeRoom();
			string copyOf = resources.GetString("CopyOf", Thread.CurrentThread.CurrentUICulture);
			this.Name = copyOf + " " + room.Name;
			this.roomHeatTemperature = room.RoomHeatTemperature;
			this.roomCoolTemperature = room.RoomCoolTemperature;
			this.roomRelativeHumidity = room.RoomRelativeHumidity;
			this.area = room.Area;
			this.heatLoad = room.HeatLoad;
			this.coolLoad = room.CoolLoad;
			this.floorHeatingLoss = room.FloorHeatingLoss;
			this.quickDimensioningHeatLoad = room.QuickDimensioningHeatLoad;
			this.quickDimensioningCoolLoad = room.QuickDimensioningCoolLoad;
			this.quickDimensioningNrOfServos = room.quickDimensioningNrOfServos;
			this.quickDimensioningRoomController = room.QuickDimensioningRoomController;
			this.quickDimensioningComments = room.QuickDimensioningComments;
			this.quickDimensioningRoomTypeId = room.quickDimensioningRoomTypeId; // used the member instead of the public property on purpose here!
			this.usedProductsForQuickDimensioning = new List<Product>();
			this.plannedProducts = new List<PlannedProduct>();
			//foreach (Product product in room.UsedProductsForQuickDimensioning) {
			//    this.usedProductsForQuickDimensioning.Add(product.Clone(this));
			//}
		}

		private void InitializeRoom() {
			id = "";
			this.internalId = System.Guid.NewGuid().ToString();
			name = "";
			roomHeatTemperature = 0;
			roomCoolTemperature = Project.Instance.InsideTemperatureForCooling;
			roomRelativeHumidity = Project.Instance.RelativeHumidity;
			area = 0;
			heatLoad = 0;
			coolLoad = 0;
			floorHeatingLoss = 0;
			roomNode.Tag = this;
			quickDimensioningRoomTypeId = "";
			this.usedProductsForQuickDimensioning = new List<Product>();
			this.plannedProducts = new List<PlannedProduct>();
			this.quickDimensioningHeatLoad = 0;
			this.quickDimensioningCoolLoad = 0;
			this.quickDimensioningNrOfServos = -1;
			this.quickDimensioningRoomController = RoomController.None;
			this.quickDimensioningComments = "";
		}

		internal void Synchronize(Room room) {
			this.Name = room.Name;
			this.RoomHeatTemperature = room.RoomHeatTemperature;
			this.RoomCoolTemperature = room.RoomCoolTemperature;
			this.RoomRelativeHumidity = room.RoomRelativeHumidity;
			this.Area = room.Area;
			this.HeatLoad = room.HeatLoad;
			this.CoolLoad = room.CoolLoad;
			this.NormalizedHeatLoad = room.NormalizedHeatLoad;
			this.NormalizedCoolLoad = room.NormalizedCoolLoad;
			this.quickDimensioningHeatLoad = room.QuickDimensioningHeatLoad;
			this.quickDimensioningCoolLoad = room.QuickDimensioningCoolLoad;
			this.quickDimensioningNrOfServos = room.quickDimensioningNrOfServos;
			this.quickDimensioningRoomController = room.QuickDimensioningRoomController;
			this.quickDimensioningComments = room.QuickDimensioningComments;
			this.quickDimensioningRoomTypeId = room.QuickDimensioningRoomTypeId;
			this.usedProductsForQuickDimensioning = new List<Product>();
			foreach (Product product in room.UsedProductsForQuickDimensioning) {
				this.usedProductsForQuickDimensioning.Add(product.Clone(this));
			}
			this.plannedProducts = new List<PlannedProduct>();
			foreach (PlannedProduct product in room.PlannedProducts) {
				this.plannedProducts.Add(new PlannedProduct(product.Product.Clone(this)));
			}
		}

		[XmlIgnore]
		public Floor AssociatedFloor {
			get {
				foreach (Floor f in Project.Instance.Floors) {
					foreach (Room room in f.Rooms) {
						if (room == this) {
							return f;
						}
					}
				}
				return null;
			}			
		}

		public string Id {
			get { return id; }
			set { 
				id = value;
				if (roomNode != null) {
					roomNode.Text = (String.IsNullOrEmpty(name) ? "unbenannt" : id + ": " + name);
				}		
			}
		}

		public string InternalId {
			get { return internalId; }
			set { internalId = value; }
		}
		
		public string Name {
			get { return name; }
			set { 
				name = value;
				if (roomNode != null) {
					roomNode.Text = (String.IsNullOrEmpty(name) ? "unbenannt" : id + ": " + name);
				}
			}
		}

		public string QuickDimensioningRoomTypeId {
			get { return quickDimensioningRoomTypeId; }
			set { quickDimensioningRoomTypeId = value; }
		}

		[XmlIgnore]
		public RoomType QuickDimensioningRoomType{
			get {
				if (this.QuickDimensioningRoomTypeId != "") {
					foreach (RoomType type in Project.Instance.Config.RoomTypes) {
						if (this.QuickDimensioningRoomTypeId == type.Id) {
							return type;
						}
					}
				} else if (Project.Instance.Config.RoomTypes.Count > 0) {
					this.QuickDimensioningRoomTypeId = Project.Instance.Config.RoomTypes[0].Id;
					return Project.Instance.Config.RoomTypes[0];
				}
				return null;
			}
			set { this.QuickDimensioningRoomTypeId = value.Id;}
		}

		public int RoomHeatTemperature {
			get { return roomHeatTemperature; }
			set { roomHeatTemperature = value; }
		}

		public int RoomCoolTemperature {
			get { return roomCoolTemperature; }
			set { roomCoolTemperature = value; }
		}

		public int RoomRelativeHumidity {
			get { return roomRelativeHumidity; }
			set { roomRelativeHumidity = value; }
		}

		public float Area {
			get { return area; }
			set { area = value; }
		}

		public int HeatLoad {
			get { return heatLoad; }
			set {
				heatLoad = value;
				this.CorrectPlanning();
			}
		}

		[XmlIgnore]
		public double OpenHeatLoad {
			get {
				double covered = 0;
				foreach (PlannedProduct pp in this.plannedProducts) {
					if (!pp.PlannedHeatLoad.Equals(double.NaN)) {
						covered += pp.PlannedHeatLoad;
					}
				}
				return covered - this.NormalizedHeatLoad;
			}
		}

		public int CoolLoad {
			get { return coolLoad; }
			set { coolLoad = value; }
		}

		[XmlIgnore]
		public double OpenCoolLoad {
			get {
				double covered = 0;
				foreach (PlannedProduct pp in this.plannedProducts) {
					if (!pp.PlannedCoolLoad.Equals(double.NaN)) {
						covered += pp.PlannedCoolLoad;
					}
				}
				return covered - this.NormalizedCoolLoad;
			}
		}

		[XmlIgnore]
		public int NormalizedHeatLoad {
			get { return heatLoad - floorHeatingLoss - additionalHeatLoad; }
			set { floorHeatingLoss = heatLoad - additionalHeatLoad - value; }
		}

		[XmlIgnore]
		public int NormalizedCoolLoad {
			get { return coolLoad; }
			set { /*TODO*/; }
		}

		public int AdditionalHeatLoad {
			get { return additionalHeatLoad; }
			set { additionalHeatLoad = value; }
		}

		public int FloorHeatingLoss {
			get { return floorHeatingLoss; }
			set { floorHeatingLoss = value; }
		}

		private void CorrectPlanning() {
			foreach (PlannedProduct pp in this.PlannedProducts) {
				string errorMsg;
				pp.Product.ConfigureProduct(pp.RequestedHeatLoad, pp.RequestedCoolLoad, pp.CalculateHeat, pp.CalculateCool, out errorMsg);
			}
		}

		public int QuickDimensioningHeatLoad {
			get { return quickDimensioningHeatLoad; }
			set { quickDimensioningHeatLoad = value; }
		}

		public int GetDefaultQuickDimensioningHeatLoad() {
			if (this.QuickDimensioningRoomType != null) {
				return (int)(this.area * this.QuickDimensioningRoomType.HeatLoadPerSquareMeter);
			}
			return 0;
		}

		public int QuickDimensioningCoolLoad {
			get { return quickDimensioningCoolLoad; }
			set { quickDimensioningCoolLoad = value; }
		}

		public int GetDefaultQuickDimensioningCoolLoad() {
			if (this.QuickDimensioningRoomType != null) {
				return (int)(this.area * this.QuickDimensioningRoomType.CoolLoadPerSquareMeter);
			}
			return 0;
		}

		private int GetDefaultNrOfServos() {
			if (this.QuickDimensioningRoomController != RoomController.None) {
				int nr = 0;
				foreach (Product product in usedProductsForQuickDimensioning) {
					nr += product.QuickDimensioningCircuits;
				}
				return nr;
			}
			return 0;
		}

		public int QuickDimensioningNrOfServos {
			get {
				if (this.quickDimensioningNrOfServos >= 0) {
					return this.quickDimensioningNrOfServos;
				}
				return this.GetDefaultNrOfServos();
			}
			set {
				if (value == this.GetDefaultNrOfServos()) {
					this.quickDimensioningNrOfServos = -1;
				} else {
					this.quickDimensioningNrOfServos = value;
				}
			}
		}

		public string QuickDimensioningComments {
			get { return quickDimensioningComments; }
			set { quickDimensioningComments = value; }
		}

		public RoomController QuickDimensioningRoomController {
			get { return quickDimensioningRoomController; }
			set { quickDimensioningRoomController = value; }
		}

		public bool QuickDimensioningHeatLoadCovered {
			get { return this.QuickDimensioningCoveredHeatLoad >= this.quickDimensioningHeatLoad; }
		}

		public bool QuickDimensioningCoolLoadCovered {
			get { return this.QuickDimensioningCoveredCoolLoad >= this.quickDimensioningCoolLoad; }
		}

		public int QuickDimensioningCoveredHeatLoad {
			get {
				int coveredLoad = 0;
				foreach (Product product in this.UsedProductsForQuickDimensioning) {
					coveredLoad += product.QuickDimensioningHeatPower;
				}
				return coveredLoad;
			}
		}

		public int QuickDimensioningCoveredCoolLoad {
			get {
				int coveredLoad = 0;
				foreach (Product product in this.UsedProductsForQuickDimensioning) {
					coveredLoad += product.QuickDimensioningCoolPower;
				}
				return coveredLoad;
			}
		}

		public int QuickDimensioningRoomTemperature {
			get { return quickDimensioningRoomTemperature; }
			set { quickDimensioningRoomTemperature = value; }
		}

		public List<Product> UsedProductsForQuickDimensioning {
			get { return usedProductsForQuickDimensioning; }
			set { usedProductsForQuickDimensioning = value; }
		}

		public List<PlannedProduct> PlannedProducts {
			get { return plannedProducts; }
			set {
				plannedProducts = value;
				foreach (PlannedProduct p in plannedProducts) {
					p.Product.AssociatedRoom = this;
				}
			}
		}

		public P GetProductForQuickDimensioning<P>() where P : Product {
			foreach (Product product in this.usedProductsForQuickDimensioning) {
				if (product.GetType() == typeof(P)) {
					return (P)product;
				}
			}
			return null;
		}
		
		/*internal void InitializeTree(TreeNode floor) {
			roomNode.Tag = this;
			floor.Nodes.Add(roomNode);
		}*/

		public Type AssociatedPanelType {
			get {
				return typeof(RoomSummaryPanel);
			}
		}

		public System.Drawing.Icon AssociatedIcon {
			get {
				return null;
			}
		}

		public TreeNode FindNode(object element) {
			if (element == this) {
				return roomNode;
			} else {
				foreach (PlannedProduct pp in this.PlannedProducts) {
					TreeNode node = pp.FindNode(element);
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
			return false;
		}

		public string SupportedPasteFormat {
			get { return null; }
		}

		public object Copy() {
			return new Room(this);
		}

		public void Paste(object o) {
			throw new Exception("Paste not supported");
		}

		internal TreeNode Node {
			get { return this.roomNode; }
		}

		public bool QuickDimensioningInitialized {
			get { return this.quickDimensioningInitialized; }
			set { this.quickDimensioningInitialized = value; }
		}

		public void InitializeQuickDimensioning() {
			if (this.quickDimensioningInitialized) {
				log.Warn("trying to initialize quick dimensioning, but it was already initialized before");
			} else {
				int heatload = this.HeatLoad;
				if (heatload <= 0) {
					heatload = this.GetDefaultQuickDimensioningHeatLoad();
				}
				this.QuickDimensioningHeatLoad = heatLoad;
				this.QuickDimensioningRoomTemperature = this.RoomHeatTemperature;
				this.quickDimensioningInitialized = true;
			}
		}

		public void RevertQuickDimensioning() {
			this.usedProductsForQuickDimensioning.Clear();
			this.QuickDimensioningRoomType = Project.Instance.Config.RoomTypes[0];
			this.quickDimensioningComments = "";
			this.quickDimensioningCoolLoad = 0;
			this.quickDimensioningHeatLoad = 0;
			this.quickDimensioningRoomController = RoomController.None;
			this.quickDimensioningNrOfServos = -1;
			this.quickDimensioningRoomTemperature = 0;
			this.quickDimensioningInitialized = false;
			this.InitializeQuickDimensioning();
		}

		internal void UpdateTree() {
			int i = 0;
			bool expand = this.Node.Nodes.Count == 0;
			foreach (PlannedProduct product in this.PlannedProducts) {
				int index = this.Node.Nodes.IndexOf(product.Node);
				if (index < 0) {
					this.Node.Nodes.Insert(i, product.Node);
				} else if (index > i) {
					if (product.Node.IsSelected) {
						for (int j = i; j < index; j++) {
							this.Node.Nodes.RemoveAt(i);
						}
					} else {
						this.Node.Nodes.RemoveAt(index);
						this.Node.Nodes.Insert(i, product.Node);
					}
				}
				i++;
			}
			while (this.Node.Nodes.Count > i) {
				this.Node.Nodes.RemoveAt(i);
			}
			if (expand) {
				this.Node.Expand();
			}
		}
	
		internal void FinalizeLoading() {
 			foreach (PlannedProduct pp in this.plannedProducts) {
				pp.Product.AssociatedRoom = this;
				pp.FinalizeLoading();
			}
			foreach (Product p in this.usedProductsForQuickDimensioning) {
				p.AssociatedRoom = this;
				p.FinalizeLoading(null);
			}
		}

		public Floor GetFloor() {
			foreach (Floor f in Project.Instance.Floors) {
				foreach (Room r in f.Rooms) {
					if (r == this) {
						return f;
					}
				}
			}
			return null;
		}

		public override string ToString() {
			return this.Id + ": " + this.Name;
		}
	}

}
