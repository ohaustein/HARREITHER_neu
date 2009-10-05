using System;
using System.Collections.Generic;
using System.Text;
using log4net;

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

	}

}
