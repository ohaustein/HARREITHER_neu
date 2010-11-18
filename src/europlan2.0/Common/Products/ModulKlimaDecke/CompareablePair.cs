using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class CompareablePair<V> : IComparable where V : IComparable {
		public V value1;
		public V value2;

		public CompareablePair() {
		}

		public CompareablePair(V value1, V value2) {
			this.value1 = value1;
			this.value2 = value2;
		}

		#region IComparable Members
		public int CompareTo(object obj) {
			if (obj is CompareablePair<V>) {
				int result = this.value1.CompareTo((obj as CompareablePair<double>).value1);
				if (result != 0) {
					return result;
				}
				return this.value2.CompareTo((obj as CompareablePair<double>).value2);
			}
			return 0;
		}
		#endregion
	}
}
