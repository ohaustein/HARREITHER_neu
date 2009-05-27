using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class EurovalProduct : Product {

		private static EurovalProduct instance = null;

		protected EurovalProduct() {

		}

		public static EurovalProduct Instance {
			get {
				if (instance == null) {
					lock (padlock) {
						if (instance == null) {
							instance = new EurovalProduct();
						}
					}
				}
				return instance;
			}
		}
	
	}
	
}
