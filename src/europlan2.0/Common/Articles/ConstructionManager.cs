using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class ConstructionManager {
		private static ConstructionManager instance = null;

		public static ConstructionManager Instance {
			get {
				if (instance == null) {
					instance = new ConstructionManager();
				}
				return instance;
			}
		}

		private List<Construction> constructions;

		private ConstructionManager() {
			this.constructions = new List<Construction>();
		}

		public List<Construction> Constructions {
			get { return this.Constructions; }
		}
	}
}
