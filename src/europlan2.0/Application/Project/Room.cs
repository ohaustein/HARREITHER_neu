using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.Windows.Forms;

namespace Europlan.Application {
	
	
	public class Room : IGuiRepresentation {

		private string name;
		private string id;
		private int temperature;
		private float area;
		private int heatPower;
		private int coolPower;
		private int normalizedHeatPower;
		private int normalizedCoolPower;
		private TreeNode roomNode = new TreeNode();

		private static readonly ILog log = LogManager.GetLogger(typeof(Room));

		public Room() {
			InitializeRoom();
		}

		public Room(string name) {
			InitializeRoom();
			this.Name = name;
		}

		private void InitializeRoom() {
			name = "";
			id = "";
			temperature = 0;
			area = 0;
			heatPower = 0;
			coolPower = 0;
			normalizedHeatPower = 0;
			normalizedCoolPower = 0;
		}


		internal void Synchronize(Room room) {
			this.Name = room.Name;
			this.Temperature = room.Temperature;
			this.Area = room.Area;
			this.HeatPower = room.HeatPower;
			this.CoolPower = room.CoolPower;
			this.NormalizedHeatPower = room.NormalizedHeatPower;
			this.NormalizedCoolPower = room.NormalizedCoolPower;
		}

		public string Name {
			get { return name; }
			set { 
				name = value;
				if (roomNode != null) {
					roomNode.Text = name;
				}
			}
		}

		public string Id {
			get { return id; }
			set { id = value; }
		}

		public int Temperature {
			get { return temperature; }
			set { temperature = value; }
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
		
		internal void InitializeTree(TreeNode floor) {
			roomNode.Tag = this;
			floor.Nodes.Add(roomNode);
		}

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
	}

}
