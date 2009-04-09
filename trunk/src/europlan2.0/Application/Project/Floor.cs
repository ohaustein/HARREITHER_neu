using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Threading;

namespace Europlan.Application {

	[Serializable()]
	public class Floor : IGuiRepresentation, IClipboard {

		private string name;
		private string id;

		private TreeNode floorNode = new TreeNode();

		private List<Room> rooms;

		[NonSerialized]
		private static readonly ILog log = LogManager.GetLogger(typeof(Floor));

		[NonSerialized]
		private System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));

		public Floor() {
			InitializeFloor();
		}

		public Floor(string name) {
			InitializeFloor();
			this.Name = name;
		}

		public Floor(Floor floor) {
			InitializeFloor();
			string copyOf = resources.GetString("CopyOf", Thread.CurrentThread.CurrentUICulture);
			this.Name = copyOf + " " + floor.Name;
			foreach (Room room in floor.rooms) {
				this.rooms.Add(new Room(room));
			}
		}

		private void InitializeFloor() {
			name = "";
			id = "";
			rooms = new List<Room>();
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
					floorNode.Text = (String.IsNullOrEmpty(name) ? "unbenannt" : name);
				}
			}
		}

		public string Id {
			get { return id; }
			set { id = value; }
		}

		public List<Room> Rooms {
			get {
				return rooms;
			}
			set {
				rooms = value;
			}
		}

		internal void InitializeTree(System.Windows.Forms.TreeNode floors) {
			floorNode.Tag = this;
			floors.Nodes.Add(floorNode);
			floorNode.Nodes.Clear();
			foreach (Room room in rooms) {
				room.InitializeTree(floorNode);
			}
		}

		public Type AssociatedPanelType {
			get { 
				return typeof(FloorSummaryPanel);
			}
		}

		public System.Drawing.Icon AssociatedIcon {
			get {
				return null;
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
				
			} else {
				throw new Exception("Paste of this type not supported");
			}			
		}

	}

}

