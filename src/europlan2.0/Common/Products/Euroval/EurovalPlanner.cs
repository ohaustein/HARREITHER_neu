using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Europlan.Common {
	public class EurovalPlanner : PipeProductPlanner<EurovalProduct, EurovalProduct.EurovalLayDistance, EurovalProduct.EurovalRimType> {
		public EurovalPlanner()
			: base() {
		}

		public EurovalPlanner(IContainer container)
			: base(container) {
		}
	}
}
