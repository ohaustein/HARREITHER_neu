using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace Europlan.Common {

	public class RegulatorCircuit {

		private string name;
		private string id;
		private int heatFlowTemperature;

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
				id += Project.Instance.RegulatorCircuits.Count + 1;
			}
			heatFlowTemperature = 35;
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

	}

}
