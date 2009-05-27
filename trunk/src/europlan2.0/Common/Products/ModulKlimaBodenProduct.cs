using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class ModulKlimaBodenProduct : Product {

		private static ModulKlimaBodenProduct instance = null;

		protected ModulKlimaBodenProduct() {

		}

		public override void Initialize() {
			quickDimensioningHeatPower = 50;
		}

		public static ModulKlimaBodenProduct Instance {
			get {
				if (instance == null) {
					lock (padlock) {
						if (instance == null) {
							instance = new ModulKlimaBodenProduct();
						}
					}
				}
				return instance;
			}
		}
	
	}
	
}
