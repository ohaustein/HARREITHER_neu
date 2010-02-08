using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {

	public class AuslegeNode : TreeNode {

		private PlannedProduct plannedProduct = null;
		private int[] azValues;
		private int[] rzValues;

		public AuslegeNode() : base() {
			
		}

		public AuslegeNode(string name) : base(name) {

		}

		public PlannedProduct PlannedProduct {
			get { return plannedProduct; }
			set { plannedProduct = value; }
		}

		public int[] AzValues {
			get { return azValues; }
			set { azValues = value; }
		}

		public int[] RzValues {
			get { return rzValues; }
			set { rzValues = value; }
		}

	}

}
