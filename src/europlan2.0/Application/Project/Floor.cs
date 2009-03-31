using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace Europlan.Application {
	

	public class Floor {

		private string name;
		private string id;

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

		public string Name {
			get { return name; }
			set { name = value; }
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
			TreeNode floor = new TreeNode(this.Name);
			floor.Tag = typeof(FloorSummaryPanel);
			floors.Nodes.Add(floor);
			foreach (Room room in rooms) {
				room.InitializeTree(floor);
			}
		}
	}

}

