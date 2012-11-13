using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Europlan.Common {
	public class ConstructionListWrapper : IList<Construction>, IBindingList, IBindingListView {

		private Configuration.ConfigurationType type;

		private Nullable<ConstructionScopeEnum> filter = null;
		private List<ConstructionType> ctFilter = null;

		public ConstructionListWrapper(Configuration.ConfigurationType type) {
			if (!(type == Configuration.ConfigurationType.UserConfiguration || type == Configuration.ConfigurationType.AdminConfiguration || type == Configuration.ConfigurationType.ProjectConfiguration)) {
				throw new Exception("type must be UserConfiguration or AdminConfiguration or ProjectConfiguration");
			}
			this.type = type;
		}

		private List<Construction> Constructions {
			get { return (this.type == Configuration.ConfigurationType.AdminConfiguration ? Configuration.AdminTemplate.Constructions : (this.type == Configuration.ConfigurationType.UserConfiguration ? Configuration.UserTemplate.Constructions : Project.Instance.Config.Constructions)); }
		}

		#region IEnumerable<Construction> Members
		public IEnumerator<Construction> GetEnumerator() {
			return new ConstructionListEnumerator(this.type, (this.filter == null ? ConstructionScopeEnum.All : this.filter.Value), this.ctFilter);
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return new ConstructionListEnumerator(this.type, (this.filter == null ? ConstructionScopeEnum.All : this.filter.Value), this.ctFilter);
		}
		#endregion

		#region ICollection<Construction> Members
		public void Add(Construction item) {
			this.Constructions.Add(item);
		}

		public void Clear() {
			this.Constructions.Clear();
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
			bool result = this.Constructions.Remove(item);
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
			while (current < this.Constructions.Count && ((this.filter != null && ((this.filter.Value & this.Constructions[current].Scope) == ConstructionScopeEnum.UnknownConstruction))) || (this.ctFilter != null && !this.ctFilter.Contains(this.Constructions[current].Type)) || this.Constructions[current].HasBeenDeleted) {
				current++;
			}
			while (i < index && current < this.Constructions.Count) {
				current++;
				while (current < this.Constructions.Count && ((this.filter != null && ((this.filter.Value & this.Constructions[current].Scope) == ConstructionScopeEnum.UnknownConstruction))) || (this.ctFilter != null && !this.ctFilter.Contains(this.Constructions[current].Type)) || this.Constructions[current].HasBeenDeleted) {
					current++;
				}
				i++;
			}
			return current < this.Constructions.Count ? current : -1;
		}

		public void Insert(int index, Construction item) {
			int node = this.GetNode(index);
			if (node < 0) {
				this.Constructions.Insert(this.Constructions.Count, item);
			} else {
				this.Constructions.Insert(node, item);
			}
		}

		public void RemoveAt(int index) {
			int node = this.GetNode(index);
			if (node >= 0) {
				this.Constructions.RemoveAt(node);
			} else {
				throw new ArgumentOutOfRangeException();
			}
		}

		public Construction this[int index] {
			get {
				int node = this.GetNode(index);
				if (node < 0) {
					throw new ArgumentOutOfRangeException();
				} else {
					return this.Constructions[node];
				}
			}
			set {
				int node = this.GetNode(index);
				if (node < 0) {
					throw new ArgumentOutOfRangeException();
				} else {
					this.Constructions[node] = value;
				}
			}
		}
		#endregion

		#region IList Members
		public int Add(object value) {
			if (value is Construction) {
				this.Constructions.Add((Construction)value);
				return this.Constructions.Count - 1;
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
				this.Constructions.Remove((Construction)value);
			}
		}

		object System.Collections.IList.this[int index] {
			get {
				int node = this.GetNode(index);
				if (node < 0) {
					throw new ArgumentOutOfRangeException();
				} else {
					return this.Constructions[node];
				}
			}
			set {
				if (value is Construction) {
					int node = this.GetNode(index);
					if (node < 0) {
						throw new ArgumentOutOfRangeException();
					} else {
						this.Constructions[node] = (Construction)value;
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

        private event ListChangedEventHandler listChanged;
        public event ListChangedEventHandler ListChanged {
            add { this.listChanged += value; }
            remove { this.listChanged -= value; }
        }

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
				string str = "";
				if (this.filter != null) {
					str = ((int)this.filter.Value).ToString();
				}
				if (this.ctFilter != null) {
					foreach (ConstructionType ct in this.ctFilter) {
						str += ";" + ct.Id;
					}
				}
				if (str.Length == 0) {
					str = null;
				}
				return str;
			}
			set {
				int i;
				if (value == null) {
					this.filter = null;
					this.ctFilter = null;
				} else {
					string[] split = value.Split(';');
					if (split.Length > 0 && Int32.TryParse(split[0], out i)) {
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
					} else {
						this.filter = null;
					}
					this.ctFilter = new List<ConstructionType>();
					for (int j = 1; j < split.Length; j++) {
						ConstructionType ct = ConstructionTypeManager.Instance.GetConstructionTypeById(split[j]);
						if (ct != null) {
							this.ctFilter.Add(ct);
						}
					}
					if (this.ctFilter.Count == 0) {
						this.ctFilter = null;
					}
				}
			}
		}

		public void RemoveFilter() {
			this.filter = null;
			this.ctFilter = null;
		}

		public ListSortDescriptionCollection SortDescriptions {
			get { return null; }
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

		public List<ConstructionType> ConstructionTypeFilter {
			get { return this.ctFilter; }
			set { this.ctFilter = value; }
		}
	}

	public class ConstructionListEnumerator : IEnumerator<Construction> {

		private Configuration.ConfigurationType type;
		private int currentNode;
		private bool finished;
		private ConstructionScopeEnum filter;
		private List<ConstructionType> constructionTypeFilter;

		internal ConstructionListEnumerator(Configuration.ConfigurationType type, ConstructionScopeEnum filter, List<ConstructionType> constructionTypeFilter) {
			if (!(type == Configuration.ConfigurationType.UserConfiguration || type == Configuration.ConfigurationType.AdminConfiguration || type == Configuration.ConfigurationType.ProjectConfiguration)) {
				throw new Exception("type must be UserConfiguration or AdminConfiguration or ProjectConfiguration");
			}
			this.type = type;
			this.constructionTypeFilter = constructionTypeFilter;
			this.currentNode = -1;
			this.finished = false;
			this.filter = filter;
		}

		private List<Construction> List {
			get { return (this.type == Configuration.ConfigurationType.AdminConfiguration ? Configuration.AdminTemplate.Constructions : (this.type == Configuration.ConfigurationType.UserConfiguration ? Configuration.UserTemplate.Constructions : Project.Instance.Config.Constructions)); }
		}

		#region IEnumerator<Construction> Members
		public Construction Current {
			get {
				if (this.currentNode < 0 || this.finished) {
					return null;
				} else {
					return this.List[this.currentNode];
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
				if (this.currentNode < 0 || this.finished) {
					return null;
				} else {
					return this.List[this.currentNode];
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
				while (this.currentNode < this.List.Count && (((this.List[this.currentNode].Scope & this.filter) == ConstructionScopeEnum.UnknownConstruction) || (this.constructionTypeFilter != null && !this.constructionTypeFilter.Contains(this.List[this.currentNode].Type)) || this.List[this.currentNode].HasBeenDeleted)) {
					this.currentNode++;
				}
				if (this.currentNode >= this.List.Count) {
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
