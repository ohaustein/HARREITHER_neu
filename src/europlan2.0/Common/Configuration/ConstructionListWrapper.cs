using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Europlan.Common {
	public class ConstructionListWrapper : IList<Construction>, IBindingList, IBindingListView {

		private List<Construction> constructions;

		private Nullable<ConstructionScopeEnum> filter = null;

		public ConstructionListWrapper() {
			this.constructions = Configuration.AdminTemplate.Constructions;
		}

		/*private void OnListChanged(ListChangedEventArgs args) {
			if (this.ListChanged != null) {
				this.ListChanged(this, args);
			}
		}*/

		#region IEnumerable<Construction> Members
		public IEnumerator<Construction> GetEnumerator() {
			return new ConstructionListEnumerator(this.constructions, (this.filter == null ? ConstructionScopeEnum.All : this.filter.Value));
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return new ConstructionListEnumerator(this.constructions, (this.filter == null ? ConstructionScopeEnum.All : this.filter.Value));
		}
		#endregion

		#region ICollection<Construction> Members
		public void Add(Construction item) {
			this.constructions.Add(item);
		}

		public void Clear() {
			this.constructions.Clear();
		}

		public bool Contains(Construction item) {
			return this.IndexOf(item) >= 0;
		}

		public void CopyTo(Construction[] array, int arrayIndex) {
			IEnumerator<Construction> e = this.GetEnumerator();
			int i = arrayIndex;
			while (e.MoveNext()) {
				array[i] = e.Current;
				i++;
			}
		}

		public int Count {
			get {
				IEnumerator<Construction> e = this.GetEnumerator();
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

		public bool Remove(Construction item) {
			bool result = this.constructions.Remove(item);
			return result;
		}
		#endregion

		#region ICollection Members
		public void CopyTo(Array array, int index) {
			IEnumerator<Construction> e = this.GetEnumerator();
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

		#region IList<Construction> Members
		public int IndexOf(Construction item) {
			int found = -1;
			int i = 0;
			IEnumerator<Construction> e = this.GetEnumerator();
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
			while (current < this.constructions.Count && (this.filter != null && ((this.filter.Value & this.constructions[current].Scope) == ConstructionScopeEnum.UnknownConstruction))) {
				current++;
			}
			while (i < index && current < this.constructions.Count) {
				current++;
				while (current < this.constructions.Count && (this.filter != null && ((this.filter.Value & this.constructions[current].Scope) == ConstructionScopeEnum.UnknownConstruction))) {
					current++;
				}
				i++;
			}
			return current < this.constructions.Count ? current : -1;
		}

		public void Insert(int index, Construction item) {
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
				this.constructions.RemoveAt(index);
			}
		}

		public Construction this[int index] {
			get {
				int node = this.GetNode(index);
				if (node < 0) {
					throw new IndexOutOfRangeException();
				} else {
					return this.constructions[node];
				}
			}
			set {
				int node = this.GetNode(index);
				if (node < 0) {
					throw new IndexOutOfRangeException();
				} else {
					this.constructions[node] = value;
				}
			}
		}
		#endregion

		#region IList Members
		public int Add(object value) {
			if (value is Construction) {
				this.constructions.Add((Construction)value);
				return this.constructions.Count - 1;
			} else {
				throw new ArgumentException("Object to add is not a construction");
			}
		}

		public bool Contains(object value) {
			return (value is Construction ? this.Contains((Construction)value) : false);
		}

		public int IndexOf(object value) {
			return (value is Construction ? this.IndexOf((Construction)value) : -1);
		}

		public void Insert(int index, object value) {
			if (value is Construction) {
				this.Insert(index, (Construction)value);
			} else {
				throw new ArgumentException("Object to insert is not a construction");
			}
		}

		public bool IsFixedSize {
			get { return false; }
		}

		public void Remove(object value) {
			if (value is Construction) {
				this.Remove((Construction)value);
			}
		}

		object System.Collections.IList.this[int index] {
			get {
				int node = this.GetNode(index);
				if (node < 0) {
					throw new IndexOutOfRangeException();
				} else {
					return this.constructions[node];
				}
			}
			set {
				if (value is Construction) {
					int node = this.GetNode(index);
					if (node < 0) {
						throw new IndexOutOfRangeException();
					} else {
						this.constructions[node] = (Construction)value;
					}
				} else {
					throw new ArgumentException("Object to set is not a construction");
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
			get { return false; }
		}

		public bool AllowNew {
			get { return false; }
		}

		public bool AllowRemove {
			get { return false; }
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
					this.filter = ConstructionScopeEnum.UnknownConstruction;
					if ((i & ((int)ConstructionScopeEnum.FloorConstruction)) != 0) {
						this.filter = this.filter | ConstructionScopeEnum.FloorConstruction;
					}
					if ((i & ((int)ConstructionScopeEnum.InsulationConstruction)) != 0) {
						this.filter = this.filter | ConstructionScopeEnum.InsulationConstruction;
					}
					if ((i & ((int)ConstructionScopeEnum.WallConstruction)) != 0) {
						this.filter = this.filter | ConstructionScopeEnum.WallConstruction;
					}
					if ((i & ((int)ConstructionScopeEnum.CeilingConstruction)) != 0) {
						this.filter = this.filter | ConstructionScopeEnum.CeilingConstruction;
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

		public ConstructionScopeEnum ConstructionScopeFilter {
			get { return (this.filter == null ? ConstructionScopeEnum.All : this.filter.Value); }
			set { this.filter = (value == ConstructionScopeEnum.All ? (Nullable<ConstructionScopeEnum>)null : (Nullable<ConstructionScopeEnum>)value); }
		}
	}

	public class ConstructionListEnumerator : IEnumerator<Construction> {

		private List<Construction> list;
		int currentNode;
		private bool finished;
		private ConstructionScopeEnum filter;

		internal ConstructionListEnumerator(List<Construction> list, ConstructionScopeEnum filter) {
			this.list = list;
			this.currentNode = -1;
			this.finished = false;
			this.filter = filter;
		}

		#region IEnumerator<Construction> Members
		public Construction Current {
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
				while (this.currentNode < this.list.Count && ((this.list[this.currentNode].Scope & this.filter) == ConstructionScopeEnum.UnknownConstruction)) {
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
