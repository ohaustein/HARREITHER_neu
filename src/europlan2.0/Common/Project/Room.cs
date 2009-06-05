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

		private string name;
		private string id;
		private int roomTemperature;
		private float area;
		private int heatLoad;
		private int coolLoad;
		private int normalizedHeatLoad;
		private int normalizedCoolLoad;
		private int quickDimensioningHeatLoad;
		private int quickDimensioningCoolLoad;
		private RoomController quickDimensioningRoomController = RoomController.None;
		private string quickDimensioningDistributor;
		private string quickDimensioningComments;
		private int additionalHeatLoad;
		private string roomTypeId;

		private List<Product> usedProductsForQuickDimensioning;

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
			this.roomTemperature = room.RoomTemperature;
			this.area = room.Area;
			this.heatLoad = room.HeatLoad;
			this.coolLoad = room.CoolLoad;
			this.normalizedHeatLoad = room.NormalizedHeatLoad;
			this.normalizedCoolLoad = room.NormalizedCoolLoad;
			this.quickDimensioningHeatLoad = room.QuickDimensioningHeatLoad;
			this.quickDimensioningCoolLoad = room.QuickDimensioningCoolLoad;
			this.quickDimensioningRoomController = room.QuickDimensioningRoomController;
			this.quickDimensioningDistributor = room.QuickDimensioningDistributor;
			this.quickDimensioningComments = room.QuickDimensioningComments;
			this.roomTypeId = room.roomTypeId;
			this.usedProductsForQuickDimensioning = new List<Product>();
			foreach (Product product in room.UsedProductsForQuickDimensioning) {
				this.usedProductsForQuickDimensioning.Add(product.Clone(this));
			}
		}

		private void InitializeRoom() {
			name = "";
			id = "";
			roomTemperature = 0;
			area = 0;
			heatLoad = 0;
			coolLoad = 0;
			normalizedHeatLoad = 0;
			normalizedCoolLoad = 0;
			roomNode.Tag = this;
			roomTypeId = "";
			this.usedProductsForQuickDimensioning = new List<Product>();
			this.quickDimensioningHeatLoad = 0;
			this.quickDimensioningCoolLoad = 0;
			this.quickDimensioningRoomController = RoomController.None;
			this.quickDimensioningDistributor = "";
			this.quickDimensioningComments = "";
		}


		internal void Synchronize(Room room) {
			this.Name = room.Name;
			this.RoomTemperature = room.RoomTemperature;
			this.Area = room.Area;
			this.HeatLoad = room.HeatLoad;
			this.CoolLoad = room.CoolLoad;
			this.NormalizedHeatLoad = room.NormalizedHeatLoad;
			this.NormalizedCoolLoad = room.NormalizedCoolLoad;
			this.quickDimensioningHeatLoad = room.QuickDimensioningHeatLoad;
			this.quickDimensioningCoolLoad = room.QuickDimensioningCoolLoad;
			this.quickDimensioningRoomController = room.QuickDimensioningRoomController;
			this.quickDimensioningDistributor = room.QuickDimensioningDistributor;
			this.quickDimensioningComments = room.QuickDimensioningComments;
			this.RoomTypeId = room.RoomTypeId;
			this.usedProductsForQuickDimensioning = new List<Product>();
			foreach (Product product in room.UsedProductsForQuickDimensioning) {
				this.usedProductsForQuickDimensioning.Add(product.Clone(this));
			}
		}

		public string Name {
			get { return name; }
			set { 
				name = value;
				if (roomNode != null) {
					roomNode.Text = (String.IsNullOrEmpty(name) ? "unbenannt" : name);
				}
			}
		}

		public string Id {
			get { return id; }
			set { id = value; }
		}

		public string RoomTypeId {
			get { return roomTypeId; }
			set { roomTypeId = value; }
		}

		[XmlIgnore]
		public RoomType RoomType{
			get {
				if (this.RoomTypeId != "") {
					foreach (RoomType type in Project.Instance.Config.RoomTypes) {
						if (this.RoomTypeId == type.Id) {
							return type;
						}
					}
				} else if (Project.Instance.Config.RoomTypes.Count > 0) {
					this.RoomTypeId = Project.Instance.Config.RoomTypes[0].Id;
					return Project.Instance.Config.RoomTypes[0];
				}
				return null;
			}
			set { this.RoomTypeId = value.Id;}
		}

		public int RoomTemperature {
			get { return roomTemperature; }
			set { roomTemperature = value; }
		}

		public float Area {
			get { return area; }
			set { area = value; }
		}

		public int HeatLoad {
			get { return heatLoad; }
			set { heatLoad = value; }
		}

		public int CoolLoad {
			get { return coolLoad; }
			set { coolLoad = value; }
		}

		public int NormalizedHeatLoad {
			get { return normalizedHeatLoad; }
			set { normalizedHeatLoad = value; }
		}

		public int NormalizedCoolLoad {
			get { return normalizedCoolLoad; }
			set { normalizedCoolLoad = value; }
		}

		public int AdditionalHeatLoad {
			get { return additionalHeatLoad; }
			set { additionalHeatLoad = value; }
		}

		public int QuickDimensioningHeatLoad {
			get { return quickDimensioningHeatLoad; }
			set { quickDimensioningHeatLoad = value; }
		}

		public int GetDefaultQuickDimensioningHeatLoad() {
			if (this.RoomType != null) {
				return (int)(this.area * this.RoomType.HeatLoadPerSquareMeter);
			}
			return 0;
		}

		public int QuickDimensioningCoolLoad {
			get { return quickDimensioningCoolLoad; }
			set { quickDimensioningCoolLoad = value; }
		}

		public int GetDefaultQuickDimensioningCoolLoad() {
			if (this.RoomType != null) {
				return (int)(this.area * this.RoomType.CoolLoadPerSquareMeter);
			}
			return 0;
		}

		[XmlIgnore]
		public int QuickDimensioningNrOfServos {
			get { 
				if (this.QuickDimensioningRoomController != RoomController.None) {
					int nr = 0;
					foreach (Product product in usedProductsForQuickDimensioning) {
						nr += product.QuickDimensioningCircuits;
					}
					return nr;
				}
				return 0;
			}
		}

		public string QuickDimensioningDistributor {
			get { return quickDimensioningDistributor; }
			set { quickDimensioningDistributor = value; }
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

		[System.Xml.Serialization.XmlIgnore()]
		public int FloorHeatingLoss {
			get { return heatLoad - normalizedHeatLoad; }
			set { normalizedHeatLoad = heatLoad - value; }
		}

		public List<Product> UsedProductsForQuickDimensioning {
			get { return usedProductsForQuickDimensioning; }
			set { usedProductsForQuickDimensioning = value; }
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

	}

}
