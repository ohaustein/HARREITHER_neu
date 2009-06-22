using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	public class QuickDimensioningDistributor {

		private string id;
		private string name;

		public QuickDimensioningDistributor() {
			InitializeDistributor();
		}

		private void InitializeDistributor() {
			this.id = System.Guid.NewGuid().ToString();
			this.name = "";
		}

		public override bool Equals(object obj) {
			if (obj is QuickDimensioningDistributor) {
				if ((obj as QuickDimensioningDistributor).Id == this.Id) {
					return true;
				}
			}
			return base.Equals(obj);
		}

		public override string ToString() {
			return this.name;
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

		public string Id {
			get { return this.id; }
			set { this.id = value; }
		}

		public string Name {
			get { return this.name; }
			set { this.name = value; }
		}

	}

}
