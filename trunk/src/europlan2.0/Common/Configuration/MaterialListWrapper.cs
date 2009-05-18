using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Europlan.Common {
	public class MaterialListWrapper : IList<Material>, IBindingList, IBindingListView {

		private List<Material> constructions;

		private Nullable<MaterialTypeEnum> filter = null;

		public MaterialListWrapper() {
			this.constructions = Configuration.AdminTemplate.Materials;
		}

		#region IEnumerable<Construction> Members
		public IEnumerator<Material> GetEnumerator() {
			return new MaterialListEnumerator(this.constructions, (this.filter == null ? MaterialTypeEnum.All : this.filter.Value));
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return new MaterialListEnumerator(this.constructions, (this.filter == null ? MaterialTypeEnum.All : this.filter.Value));
		}
		#endregion

		#region ICollection<Construction> Members
		public void Add(Material item) {
			this.constructions.Add(item);
		}

		public void Clear() {
			this.constructions.Clear();
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
			bool result = this.constructions.Remove(item);
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
			// TODO while (current < this.constructions.Count && (this.filter != null && ((this.filter.Value & this.constructions[current].Type) == MaterialTypeEnum.UnknownMaterial))) {
				current++;
			// TODO}
			while (i < index && current < this.constructions.Count) {
				current++;
				// TODOwhile (current < this.constructions.Count && (this.filter != null && ((this.filter.Value & this.constructions[current].Type) == MaterialTypeEnum.UnknownMaterial))) {
					current++;
					// TODO}
				i++;
			}
			return current < this.constructions.Count ? current : -1;
		}

		public void Insert(int index, Material item) {
			int node = this.GetNode(index);
			if (node < 0) {
				this.constructions.Insert(this.constructions.Count, item);
			} else {
				this.constructions.Insert(node, item);
			}
		}

		public void RemoveAt(int index) {
			int node = this.GetNode(index);
			if (node >= 0) {
				this.constructions.RemoveAt(node);
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
					return this.constructions[node];
				}
			}
			set {
				int node = this.GetNode(index);
				if (node < 0) {
					throw new ArgumentOutOfRangeException();
				} else {
					this.constructions[node] = value;
				}
			}
		}
		#endregion

		#region IList Members
		public int Add(object value) {
			if (value is Material) {
				this.constructions.Add((Material)value);
				return this.constructions.Count - 1;
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
				this.constructions.Remove((Material)value);
			}
		}

		object System.Collections.IList.this[int index] {
			get {
				int node = this.GetNode(index);
				if (node < 0) {
					throw new ArgumentOutOfRangeException();
				} else {
					return this.constructions[node];
				}
			}
			set {
				if (value is Material) {
					int node = this.GetNode(index);
					if (node < 0) {
						throw new ArgumentOutOfRangeException();
					} else {
						this.constructions[node] = (Material)value;
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
			throw new NotSupportedException("The method or operation is not implemented.");
		}

		public bool AllowEdit {
			get { return true; }
		}

		public bool AllowNew {
			get { return false; }
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
					this.filter = MaterialTypeEnum.UnknownMaterial;
					if ((i & ((int)MaterialTypeEnum.Euroval)) != 0) {
						this.filter = this.filter | MaterialTypeEnum.Euroval;
					}
					if ((i & ((int)MaterialTypeEnum.Modul)) != 0) {
						this.filter = this.filter | MaterialTypeEnum.Modul;
					}
					if ((i & ((int)MaterialTypeEnum.Hitherm)) != 0) {
						this.filter = this.filter | MaterialTypeEnum.Hitherm;
					}
					if ((i & ((int)MaterialTypeEnum.Distributor)) != 0) {
						this.filter = this.filter | MaterialTypeEnum.Distributor;
					}
					if ((i & ((int)MaterialTypeEnum.Insulation)) != 0) {
						this.filter = this.filter | MaterialTypeEnum.Insulation;
					}
					if ((i & ((int)MaterialTypeEnum.General)) != 0) {
						this.filter = this.filter | MaterialTypeEnum.General;
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

		public MaterialTypeEnum MaterialTypeFilter {
			get { return (this.filter == null ? MaterialTypeEnum.All : this.filter.Value); }
			set { this.filter = (value == MaterialTypeEnum.All ? (Nullable<MaterialTypeEnum>)null : (Nullable<MaterialTypeEnum>)value); }
		}
	}

	public class MaterialListEnumerator : IEnumerator<Material> {

		private List<Material> list;
		int currentNode;
		private bool finished;
		private MaterialTypeEnum filter;

		internal MaterialListEnumerator(List<Material> list, MaterialTypeEnum filter) {
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
				// TODOwhile (this.currentNode < this.list.Count && ((this.list[this.currentNode].Type & this.filter) == MaterialTypeEnum.UnknownMaterial)) {
					this.currentNode++;
					// TODO}
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
