using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class QuickDimensioningProjectSummary : IQuickDimensioningSummary {
		#region IQuickDimensioningSummary Members
		public string Name {
			get { return "Gesamt"; }
		}

		public float Area {
			get {
				float area = 0;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						area += room.Area;
					}
				}
				return area;
			}
		}

		public int HeatLoad {
			get {
				int heatLoad = 0;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						heatLoad += room.QuickDimensioningHeatLoad;
					}
				}
				return heatLoad;
			}
		}

		public int CoolLoad {
			get {
				int coolLoad = 0;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						coolLoad += room.QuickDimensioningCoolLoad;
					}
				}
				return coolLoad;
			}
		}

		private float GetProductArea<P>() where P : Product {
			float area = 0;
			foreach (Floor floor in Project.Instance.Floors) {
				foreach (Room room in floor.Rooms) {
					P product = room.GetProductForQuickDimensioning<P>();
					if (product != null) {
						area += product.QuickDimensioningPlannedArea;
					}
				}
			}
			return area;
		}

		private int GetProductCircuits<P>() where P : Product {
			int circuits = 0;
			foreach (Floor floor in Project.Instance.Floors) {
				foreach (Room room in floor.Rooms) {
					P product = room.GetProductForQuickDimensioning<P>();
					if (product != null) {
						circuits += product.QuickDimensioningCircuits;
					}
				}
			}
			return circuits;
		}

		public float EurovalArea {
			get { return this.GetProductArea<EurovalProduct>(); }
		}

		public int EurovalCircuits {
			get { return this.GetProductCircuits<EurovalProduct>(); }
		}

		public float ConcreteActivationArea {
			get { return this.GetProductArea<ConcreteActivationProduct>(); }
		}

		public int ConcreteActivationCircuits {
			get { return this.GetProductCircuits<ConcreteActivationProduct>(); }
		}

		public float HithermArea {
			get { return this.GetProductArea<HithermProduct>(); }
		}

		public int HithermCircuits {
			get { return this.GetProductCircuits<HithermProduct>(); }
		}

		public float HithermCompactArea {
			get { return this.GetProductArea<HithermCompactProduct>(); }
		}

		public int HithermCompactCircuits {
			get { return this.GetProductCircuits<HithermCompactProduct>(); }
		}

		public float HithermCompactRoofArea {
			get { return this.GetProductArea<HithermCompactRoofProduct>(); }
		}

		public int HithermCompactRoofCircuits {
			get { return this.GetProductCircuits<HithermCompactRoofProduct>(); }
		}

		public float ModulKlimaBodenArea {
			get { return this.GetProductArea<ModulKlimaBodenProduct>(); }
		}

		public int ModulKlimaBodenCircuits {
			get { return this.GetProductCircuits<ModulKlimaBodenProduct>(); }
		}

		public float ModulKlimaDeckeArea {
			get { return this.GetProductArea<ModulKlimaDeckeProduct>(); }
		}

		public int ModulKlimaDeckeCircuits {
			get { return this.GetProductCircuits<ModulKlimaDeckeProduct>(); }
		}

		public int NrOfServos {
			get {
				int servos = 0;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						servos += room.QuickDimensioningNrOfServos;
					}
				}
				return servos;
			}
		}

		public string RoomControllers {
			get {
				Dictionary<Room.RoomController, int> controllers = new Dictionary<Room.RoomController, int>();
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						if (room.QuickDimensioningRoomController != Room.RoomController.None) {
							int c = 0;
							if (controllers.ContainsKey(room.QuickDimensioningRoomController)) {
								c = controllers[room.QuickDimensioningRoomController];
							}
							c++;
							controllers[room.QuickDimensioningRoomController] = c;
						}
					}
				}
				string controllersSummary = null;
				foreach (KeyValuePair<Room.RoomController, int> kvp in controllers) {
					if (controllersSummary != null) {
						controllersSummary += ", ";
					} else {
						controllersSummary = "";
					}
					controllersSummary += kvp.Value.ToString() + "*" + kvp.Key.ToString();
				}
				return controllersSummary;
			}
		}
		#endregion
	}
}
