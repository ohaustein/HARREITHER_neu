using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Europlan.Common {
	public class MaterialListWrapper : IList<Material>, IBindingList, IBindingListView {

		private List<Material> materials;

		private Nullable<CategoryType> filter = null;

		public MaterialListWrapper() {
			this.materials = Configuration.AdminTemplate.Materials;
		}

		#region IEnumerable<Construction> Members
		public IEnumerator<Material> GetEnumerator() {
			return new MaterialListEnumerator(this.materials, this.filter);
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return new MaterialListEnumerator(this.materials, this.filter);
		}
		#endregion

		#region ICollection<Construction> Members
		public void Add(Material item) {
			this.materials.Add(item);
		}

		public void Clear() {
			this.materials.Clear();
		}

		public bool Contains(Material item) {
			return this.IndexOf(item) >= 0;
		}

		public void CopyTo(Material[] array, int arrayIndex) {
			IEnumerator<Material> e = this.GetEnumerator();
			int i = arrayIndex;
			while (e.MoveNext()) {
				array[i] = e.Current;
				i++;
			}
		}

		public int Count {
			get {
				IEnumerator<Material> e = this.GetEnumerator();
				int i = 0;
				while (e.MoveNext()) {
					i++;
				}
				return i;
			}
		}

		public bool IsReadOnly {
			get { return false; }
		}

		public bool Remove(Material item) {
			bool result = this.materials.Remove(item);
			return result;
		}
		#endregion

		#region ICollection Members
		public void CopyTo(Array array, int index) {
			IEnumerator<Material> e = this.GetEnumerator();
			int i = index;
			while (e.MoveNext()) {
				array.SetValue(e.Current, i);
				i++;
			}
		}

		public bool IsSynchronized {
			get { return false; }
		}

		public object SyncRoot {
			get { return null; }
		}
		#endregion

		#region IList<Material> Members
		public int IndexOf(Material item) {
			int found = -1;
			int i = 0;
			IEnumerator<Material> e = this.GetEnumerator();
			while (found < 0 && e.MoveNext()) {
				if (e.Current == item) {
					found = i;
				}
				i++;
			}
			return found;
		}

		private int GetNode(int index) {
			int i = 0;
			int current = 0;
			while (current < this.materials.Count && (this.filter != null && (this.materials[current].Category == null || this.filter.Value != this.materials[current].Category.Type))) {
				current++;
			}
			while (i < index && current < this.materials.Count) {
				current++;
				while (current < this.materials.Count && (this.filter != null && (this.materials[current].Category == null || this.filter.Value != this.materials[current].Category.Type))) {
					current++;
				}
				i++;
			}
			return current < this.materials.Count ? current : -1;
		}

		public void Insert(int index, Material item) {
			int node = this.GetNode(index);
			if (node < 0) {
				this.materials.Insert(this.materials.Count, item);
			} else {
				this.materials.Insert(node, item);
			}
		}

		public void RemoveAt(int index) {
			int node = this.GetNode(index);
			if (node >= 0) {
				this.materials.RemoveAt(node);
			} else {
				throw new ArgumentOutOfRangeException();
			}
		}

		public Material this[int index] {
			get {
				int node = this.GetNode(index);
				if (node < 0) {
					throw new ArgumentOutOfRangeException();
				} else {
					return this.materials[node];
				}
			}
			set {
				int node = this.GetNode(index);
				if (node < 0) {
					throw new ArgumentOutOfRangeException();
				} else {
					this.materials[node] = value;
				}
			}
		}
		#endregion

		#region IList Members
		public int Add(object value) {
			if (value is Material) {
				this.materials.Add((Material)value);
				return this.materials.Count - 1;
			} else {
				throw new ArgumentException("Object to add is not a material");
			}
		}

		public bool Contains(object value) {
			return (value is Material ? this.Contains((Material)value) : false);
		}

		public int IndexOf(object value) {
			return (value is Material ? this.IndexOf((Material)value) : -1);
		}

		public void Insert(int index, object value) {
			if (value is Material) {
				this.Insert(index, (Material)value);
			} else {
				throw new ArgumentException("Object to insert is not a material");
			}
		}

		public bool IsFixedSize {
			get { return false; }
		}

		public void Remove(object value) {
			if (value is Material) {
				this.materials.Remove((Material)value);
			}
		}

		object System.Collections.IList.this[int index] {
			get {
				int node = this.GetNode(index);
				if (node < 0) {
					throw new ArgumentOutOfRangeException();
				} else {
					return this.materials[node];
				}
			}
			set {
				if (value is Material) {
					int node = this.GetNode(index);
					if (node < 0) {
						throw new ArgumentOutOfRangeException();
					} else {
						this.materials[node] = (Material)value;
					}
				} else {
					throw new ArgumentException("Object to set is not a material");
				}
			}
		}
		#endregion

		#region IBindingList Members
		public void AddIndex(PropertyDescriptor property) {
		}

		public object AddNew() {
			Material m = new Material();
			this.materials.Add(m);
			return m;
		}

		public bool AllowEdit {
			get { return true; }
		}

		public bool AllowNew {
			get { return this.filter == CategoryType.General; }
		}

		public bool AllowRemove {
			get { return true; }
		}

		public void ApplySort(PropertyDescriptor property, ListSortDirection direction) {
			throw new NotSupportedException("The method or operation is not implemented.");
		}

		public int Find(PropertyDescriptor property, object key) {
			throw new NotSupportedException("The method or operation is not implemented.");
		}

		public bool IsSorted {
			get { throw new NotSupportedException("The method or operation is not implemented."); }
		}

		public event ListChangedEventHandler ListChanged;

		public void RemoveIndex(PropertyDescriptor property) {
		}

		public void RemoveSort() {
			throw new NotSupportedException("The method or operation is not implemented.");
		}

		public ListSortDirection SortDirection {
			get { throw new NotSupportedException("The method or operation is not implemented."); }
		}

		public PropertyDescriptor SortProperty {
			get { throw new NotSupportedException("The method or operation is not implemented."); }
		}

		public bool SupportsChangeNotification {
			get { return false; }
		}

		public bool SupportsSearching {
			get { return false; }
		}

		public bool SupportsSorting {
			get { return false; }
		}
		#endregion

		#region IBindingListView Members
		public void ApplySort(ListSortDescriptionCollection sorts) {
			throw new NotSupportedException("The method or operation is not implemented.");
		}

		public string Filter {
			get {
				return (this.filter == null ? null : ((int)this.filter.Value).ToString());
			}
			set {
				int i;
				if (value == null) {
					this.filter = null;
				} else if (Int32.TryParse(value, out i)) {
					if (i == ((int)CategoryType.General)) {
						this.filter = CategoryType.General;
					} else if (i == ((int)CategoryType.Floor)) {
						this.filter = CategoryType.Floor;
					} else if (i == ((int)CategoryType.Wall)) {
						this.filter = CategoryType.Wall;
					} else if (i == ((int)CategoryType.Ceiling)) {
						this.filter = CategoryType.Ceiling;
					} else if (i == ((int)CategoryType.Distributor)) {
						this.filter = CategoryType.Distributor;
					} else if (i == ((int)CategoryType.Insulation)) {
						this.filter = CategoryType.Insulation;
					} else {
						this.filter = null;
					}
				}
			}
		}

		public void RemoveFilter() {
			this.filter = null;
		}

		public ListSortDescriptionCollection SortDescriptions {
			get { return null; /*throw new Exception("The method or operation is not implemented.");*/ }
		}

		public bool SupportsAdvancedSorting {
			get { return false; }
		}

		public bool SupportsFiltering {
			get { return true; }
		}
		#endregion

		public Nullable<CategoryType> FilterCategory {
			get { return this.filter; }
			set { this.filter = value; }
		}
	}

	public class MaterialListEnumerator : IEnumerator<Material> {

		private List<Material> list;
		int currentNode;
		private bool finished;
		private Nullable<CategoryType> filter;

		internal MaterialListEnumerator(List<Material> list, Nullable<CategoryType> filter) {
			this.list = list;
			this.currentNode = -1;
			this.finished = false;
			this.filter = filter;
		}

		#region IEnumerator<Construction> Members
		public Material Current {
			get {
				if (this.currentNode <= 0 || this.finished) {
					return null;
				} else {
					return this.list[this.currentNode];
				}
			}
		}
		#endregion

		#region IDisposable Members
		public void Dispose() {
		}
		#endregion

		#region IEnumerator Members
		object System.Collections.IEnumerator.Current {
			get {
				if (this.currentNode <= 0 || this.finished) {
					return null;
				} else {
					return this.list[this.currentNode];
				}
			}
		}

		public bool MoveNext() {
			if (!this.finished) {
				if (this.currentNode < 0) {
					this.currentNode = 0;
				} else {
					this.currentNode++;
				}
				while (this.currentNode < this.list.Count && (this.filter != null && (this.list[currentNode].Category == null || this.list[this.currentNode].Category.Type != this.filter.Value))) {
					this.currentNode++;
				}
				if (this.currentNode >= this.list.Count) {
					this.finished = true;
				}
			}
			return !this.finished;
		}

		public void Reset() {
			this.currentNode = -1;
			this.finished = false;
		}
		#endregion
	}
}
