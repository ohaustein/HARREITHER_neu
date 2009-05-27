using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class HithermCompactProduct : Product {

		private static HithermCompactProduct instance = null;

		protected HithermCompactProduct() {

		}

		public override void Initialize() {
			quickDimensioningHeatPower = 100;
		}

		public static HithermCompactProduct Instance {
			get {
				if (instance == null) {
					lock (padlock) {
						if (instance == null) {
							instance = new HithermCompactProduct();
						}
					}
				}
				return instance;
			}
		}
	
	}
	
}
