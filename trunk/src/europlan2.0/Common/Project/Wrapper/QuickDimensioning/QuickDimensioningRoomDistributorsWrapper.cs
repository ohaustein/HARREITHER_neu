using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class QuickDimensioningRoomDistributorsWrapper {

		private Room room;
		private Floor floor;
		private Distributor distributor;

		public QuickDimensioningRoomDistributorsWrapper(Room room, Floor floor, Distributor distributor) {
			this.room = room;
			this.floor = floor;
			this.distributor = distributor;
		}

		public string RoomId {
			get { return this.room.Id; }
		}

		public string RoomName {
			get { return this.room.Name; }
		}

		public string FloorName {
			get { return this.floor.Name; } // TODO
		}

		/*private int GetPlannedCircuitsForRoom() {
			int circuits = 0;

			// Euroval
			Product product = this.room.GetProductForQuickDimensioning<EurovalProduct>();
			if (product != null) {
				if (product.QuickDimensioningConnectedDistributors.ContainsKey(this.distributor.Id)) {
					circuits += product.QuickDimensioningConnectedDistributors[this.distributor.Id];
				}
			}
			// ConcreteActivation
			product = this.room.GetProductForQuickDimensioning<ConcreteActivationProduct>();
			if (product != null) {
				if (product.QuickDimensioningConnectedDistributors.ContainsKey(this.distributor.Id)) {
					circuits += product.QuickDimensioningConnectedDistributors[this.distributor.Id];
				}
			}
			// Hitherm
			product = this.room.GetProductForQuickDimensioning<HithermProduct>();
			if (product != null) {
				if (product.QuickDimensioningConnectedDistributors.ContainsKey(this.distributor.Id)) {
					circuits += product.QuickDimensioningConnectedDistributors[this.distributor.Id];
				}
			}
			// Hitherm Compact
			product = this.room.GetProductForQuickDimensioning<HithermCompactProduct>();
			if (product != null) {
				if (product.QuickDimensioningConnectedDistributors.ContainsKey(this.distributor.Id)) {
					circuits += product.QuickDimensioningConnectedDistributors[this.distributor.Id];
				}
			}
			// Modul Klimaboden
			product = this.room.GetProductForQuickDimensioning<ModulKlimaBodenProduct>();
			if (product != null) {
				if (product.QuickDimensioningConnectedDistributors.ContainsKey(this.distributor.Id)) {
					circuits += product.QuickDimensioningConnectedDistributors[this.distributor.Id];
				}
			}
			// Modul Klimadecke
			product = this.room.GetProductForQuickDimensioning<ModulKlimaDeckeProduct>();
			if (product != null) {
				if (product.QuickDimensioningConnectedDistributors.ContainsKey(this.distributor.Id)) {
					circuits += product.QuickDimensioningConnectedDistributors[this.distributor.Id];
				}
			}

			return circuits;
		}*/

		private int GetOpenCircuitsForProduct<P>() where P : Product {
			Product product = this.room.GetProductForQuickDimensioning<P>();
			int open = 0;
			if (product != null) {
				open = product.QuickDimensioningCircuits;
				foreach (int connected in product.QuickDimensioningConnectedDistributors.Values) {
					open -= connected;
				}
			}
			return open;
		}

		public OpenCircuits GetOpenCircuits<P>() where P : Product {
			P product = this.room.GetProductForQuickDimensioning<P>();
			if (product != null) {
				return new OpenCircuits(product.QuickDimensioningCircuits, this.GetOpenCircuitsForProduct<P>());
			}
			return new OpenCircuits(0, 0);
		}

		public Nullable<int> GetPlannedCircuits<P>() where P : Product {
			P product = this.room.GetProductForQuickDimensioning<P>();
			if (product != null && product.QuickDimensioningConnectedDistributors.ContainsKey(this.distributor.Id)) {
				return product.QuickDimensioningConnectedDistributors[this.distributor.Id];
			}

			return null;
		}

		public int GetMaximumPlannableCircuits<P>() where P : Product {
			OpenCircuits open = this.GetOpenCircuits<P>();
			if (open == null) {
				return 0;
			}
			Nullable<int> planned = this.GetPlannedCircuits<P>();
			if (planned == null) {
				planned = 0;
			}
			return open.Open + planned.Value;
		}

		private void SetPlannedCircuits<P>(Nullable<int> circuits) where P : Product {
			P product = this.room.GetProductForQuickDimensioning<P>();
			if (circuits == 0) {
				circuits = null;
			}
			if (product != null) {
				if (circuits == null && product.QuickDimensioningConnectedDistributors.ContainsKey(this.distributor.Id)) {
					product.QuickDimensioningConnectedDistributors.Remove(this.distributor.Id);
				} else if (circuits != null) {
					product.QuickDimensioningConnectedDistributors[this.distributor.Id] = circuits.Value;
				}
			} else {
				// TODO log
			}
		}

		public bool IsProductPlanned<P>() where P : Product {
			return this.room.GetProductForQuickDimensioning<P>() != null;
		}

		// Euroval
		public OpenCircuits EurovalOpenCircuits {
			get { return this.GetOpenCircuits<EurovalProduct>(); }
		}
		public Nullable<int> EurovalPlannedCircuits {
			get { return this.GetPlannedCircuits<EurovalProduct>(); }
			set { this.SetPlannedCircuits<EurovalProduct>(value); }
		}

		// Concrete Activation
		public OpenCircuits ConcreteActivationOpenCircuits {
			get { return this.GetOpenCircuits<ConcreteActivationProduct>(); }
		}
		public Nullable<int> ConcreteActivationPlannedCircuits {
			get { return this.GetPlannedCircuits<ConcreteActivationProduct>(); }
			set { this.SetPlannedCircuits<ConcreteActivationProduct>(value); }
		}

		// Hitherm
		public OpenCircuits HithermOpenCircuits {
			get { return this.GetOpenCircuits<HithermProduct>(); }
		}
		public Nullable<int> HithermPlannedCircuits {
			get { return this.GetPlannedCircuits<HithermProduct>(); }
			set { this.SetPlannedCircuits<HithermProduct>(value); }
		}

		// Hitherm Compact
		public OpenCircuits HithermCompactOpenCircuits {
			get { return this.GetOpenCircuits<HithermCompactProduct>(); }
		}
		public Nullable<int> HithermCompactPlannedCircuits {
			get { return this.GetPlannedCircuits<HithermCompactProduct>(); }
			set { this.SetPlannedCircuits<HithermCompactProduct>(value); }
		}

		// Hitherm Compact Dachschräge
		public OpenCircuits HithermCompactRoofOpenCircuits {
			get { return this.GetOpenCircuits<HithermCompactRoofProduct>(); }
		}
		public Nullable<int> HithermCompactRoofPlannedCircuits {
			get { return this.GetPlannedCircuits<HithermCompactRoofProduct>(); }
			set { this.SetPlannedCircuits<HithermCompactRoofProduct>(value); }
		}

		// ModulKlimaBoden
		public OpenCircuits ModulKlimaBodenOpenCircuits {
			get { return this.GetOpenCircuits<ModulKlimaBodenProduct>(); }
		}
		public Nullable<int> ModulKlimaBodenPlannedCircuits {
			get { return this.GetPlannedCircuits<ModulKlimaBodenProduct>(); }
			set { this.SetPlannedCircuits<ModulKlimaBodenProduct>(value); }
		}

		// ModulKlimaDecke
		public OpenCircuits ModulKlimaDeckeOpenCircuits {
			get { return this.GetOpenCircuits<ModulKlimaDeckeProduct>(); }
		}
		public Nullable<int> ModulKlimaDeckePlannedCircuits {
			get { return this.GetPlannedCircuits<ModulKlimaDeckeProduct>(); }
			set { this.SetPlannedCircuits<ModulKlimaDeckeProduct>(value); }
		}
	}

	public class OpenCircuits {
		private int total;
		private int open;

		public OpenCircuits(int total, int open) {
			this.total = total;
			this.open = open;
		}

		public int Total {
			get { return this.total; }
			set { this.total = value; }
		}

		public int Open {
			get { return this.open; }
			set { this.open = value; }
		}

		public override string ToString() {
			if (this.total == 0) {
				return "";
			}
			return this.open.ToString() + " / " + this.total.ToString();
		}
	}
}
