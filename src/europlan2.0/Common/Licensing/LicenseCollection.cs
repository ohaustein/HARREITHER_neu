using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Licensing {
	public class LicenseCollection<BaseType> : ICollection<BaseType> {

		private List<BaseType> modules = new List<BaseType>();
		private License license;

		public LicenseCollection(License license) {
			this.license = license;
		}

		#region ICollection<BaseType> Members
		public void Add(BaseType item) {
			this.modules.Add(item);
		}

		public void Clear() {
			this.modules.Clear();
		}

		public bool Contains(BaseType item) {
			return this.modules.Contains(item);
		}

		public void CopyTo(BaseType[] array, int arrayIndex) {
			this.modules.CopyTo(array, arrayIndex);
		}

		public int Count {
			get { return this.modules.Count; }
		}

		public bool IsReadOnly {
			get { return false; }
		}

		public bool Remove(BaseType item) {
			return this.modules.Remove(item);
		}
		#endregion

		#region IEnumerable<BaseType> Members
		public IEnumerator<BaseType> GetEnumerator() {
			return this.modules.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return this.modules.GetEnumerator();
		}
		#endregion
	}
}
