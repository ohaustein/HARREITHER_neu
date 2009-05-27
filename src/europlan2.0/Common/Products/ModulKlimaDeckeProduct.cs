using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class ModulKlimaDeckeProduct : Product {

		private static ModulKlimaDeckeProduct instance = null;

		protected ModulKlimaDeckeProduct() {

		}

		public override void Initialize() {
			quickDimensioningHeatPower = 80;
		}

		public static ModulKlimaDeckeProduct Instance {
			get {
				if (instance == null) {
					lock (padlock) {
						if (instance == null) {
							instance = new ModulKlimaDeckeProduct();
						}
					}
				}
				return instance;
			}
		}
	
	}
	
}
