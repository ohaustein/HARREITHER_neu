using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace Europlan.Application {


	public class Floor : IGuiRepresentation {

		private string name;
		private string id;
		private TreeNode floorNode;

		private List<Room> rooms;

		private static readonly ILog log = LogManager.GetLogger(typeof(Floor));

		public Floor() {
			InitializeFloor();
		}

		public Floor(string name) {
			InitializeFloor();
			this.name = name;
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
				}
			}
		}

		public string Name {
			get { return name; }
			set { 
				name = value;
				if (floorNode != null) {
					floorNode.Text = name;
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
			floorNode = new TreeNode(this.Name);
			floorNode.Tag = this;
			floors.Nodes.Add(floorNode);
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

	}

}

