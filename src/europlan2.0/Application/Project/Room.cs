using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.Windows.Forms;
using System.Threading;

namespace Europlan.Application {

	[Serializable()]
	public class Room : IGuiRepresentation, IClipboard {

		private string name;
		private string id;
		private int roomTemperature;
		private float area;
		private int heatPower;
		private int coolPower;
		private int normalizedHeatPower;
		private int normalizedCoolPower;
		private int additionalHeatPower;
		private string roomTypeId;

		private TreeNode roomNode = new TreeNode();

		[NonSerialized]
		private static readonly ILog log = LogManager.GetLogger(typeof(Room));

		[NonSerialized]
		private System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));

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
			this.heatPower = room.HeatPower;
			this.coolPower = room.CoolPower;
			this.normalizedHeatPower = room.NormalizedHeatPower;
			this.normalizedCoolPower = room.NormalizedCoolPower;
			this.roomTypeId = room.roomTypeId;
		}

		private void InitializeRoom() {
			name = "";
			id = "";
			roomTemperature = 0;
			area = 0;
			heatPower = 0;
			coolPower = 0;
			normalizedHeatPower = 0;
			normalizedCoolPower = 0;
			roomNode.Tag = this;
			roomTypeId = "";
		}


		internal void Synchronize(Room room) {
			this.Name = room.Name;
			this.RoomTemperature = room.RoomTemperature;
			this.Area = room.Area;
			this.HeatPower = room.HeatPower;
			this.CoolPower = room.CoolPower;
			this.NormalizedHeatPower = room.NormalizedHeatPower;
			this.NormalizedCoolPower = room.NormalizedCoolPower;
			this.RoomTypeId = room.RoomTypeId;
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

		public int RoomTemperature {
			get { return roomTemperature; }
			set { roomTemperature = value; }
		}

		public float Area {
			get { return area; }
			set { area = value; }
		}

		public int HeatPower {
			get { return heatPower; }
			set { heatPower = value; }
		}

		public int CoolPower {
			get { return coolPower; }
			set { coolPower = value; }
		}

		public int NormalizedHeatPower {
			get { return normalizedHeatPower; }
			set { normalizedHeatPower = value; }
		}

		public int NormalizedCoolPower {
			get { return normalizedCoolPower; }
			set { normalizedCoolPower = value; }
		}

		public int AdditionalHeatPower {
			get { return additionalHeatPower; }
			set { additionalHeatPower = value; }
		}

		[System.Xml.Serialization.XmlIgnore()]
		public int FloorHeatingLoss {
			get { return heatPower - normalizedHeatPower; }
			set { normalizedHeatPower = heatPower - value; }
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
