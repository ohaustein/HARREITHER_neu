using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.Xml.Serialization;

namespace Europlan.Common {

	public class RegulatorCircuit {

		private string name;
		private string id;
		private int heatFlowTemperature;
		private int coolFlowTemperature;

		private static readonly ILog log = LogManager.GetLogger(typeof(RegulatorCircuit));


		public RegulatorCircuit() {
			InitializeRegulatorCircuit();
		}

		public RegulatorCircuit(string name) {
			InitializeRegulatorCircuit();
			this.Name = name;
		}

		private void InitializeRegulatorCircuit() {
			name = "";
			id = "RK";
			if (Project.Instance != null) {
				id += String.Format("{0:00}", Project.Instance.RegulatorCircuits.Count + 1);
			}
			heatFlowTemperature = 35;
			coolFlowTemperature = 16;
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public string Id {
			get { return id; }
			set { id = value; }
		}

		public int HeatFlowTemperature {
			get { return heatFlowTemperature; }
			set { heatFlowTemperature = value; }
		}

		public int CoolFlowTemperature {
			get { return coolFlowTemperature; }
			set { coolFlowTemperature = value; }
		}

		public override string ToString() {
			return id + " - " + name;
		}

		[XmlIgnore]
		public List<Distributor> ConnectedDistributors {
			get {
				List<Distributor> distributors = new List<Distributor>();
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Distributor dist in floor.Distributors) {
						if (dist.RegulatorCircuit == this) {
							distributors.Add(dist);
						}
					}
				}
				return distributors;
			}
		}

        public ICollection<PlannedProduct> GetTichelmannConnectedProducts() {
            List<PlannedProduct> products = new List<PlannedProduct>();
            foreach (Floor floor in Project.Instance.Floors) {
                foreach (Room room in floor.Rooms) {
                    foreach (PlannedProduct pp in room.PlannedProducts) {
                        if (pp.Product != null && pp.Product.PlannedConnection != null && pp.Product.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.TICHELMANN && pp.Product.PlannedConnection.RegulatorCircuit == this) {
                            products.Add(pp);
                        }
                    }
                }
            }
            return products;
        }

        internal bool UsedForTichelmann {
            get {
                return this.GetTichelmannConnectedProducts().Count > 0;
            }
        }
    }

}
