using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Europlan.Common {
	public class EcothermPlanner : PipeProductPlanner<EcothermProduct, EcothermProduct.EcothermLayDistance, EcothermProduct.EcothermRimType> {
		public EcothermPlanner()
			: base() {
		}

		public EcothermPlanner(IContainer container)
			: base(container) {
		}
	}
}
