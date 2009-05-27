using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class HithermProduct : Product {

		private static HithermProduct instance = null;

		protected HithermProduct() {

		}

		public static HithermProduct Instance {
			get {
				if (instance == null) {
					lock (padlock) {
						if (instance == null) {
							instance = new HithermProduct();
						}
					}
				}
				return instance;
			}
		}
	
	}
	
}
