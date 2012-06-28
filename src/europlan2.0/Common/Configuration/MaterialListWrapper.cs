using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Europlan.Common {
	public class MaterialListWrapper : IList<Material>, IBindingList, IBindingListView {

		private Configuration.ConfigurationType type;

		private Nullable<CategoryType> categoryFilter = null;
		private bool onlyAdditional = false;
		private bool admin = false;

		public MaterialListWrapper(Configuration.ConfigurationType type) {
			if (!(type == Configuration.ConfigurationType.UserConfiguration || type == Configuration.ConfigurationType.AdminConfiguration)) {
				throw new Exception("type must be UserConfiguration or AdminConfiguration");
			}
			this.type = type;
		}

		public bool Admin {
			get { return this.admin; }
			set { this.admin = value; }
		}

		private List<Material> Materials {
			get { return (this.type == Configuration.ConfigurationType.AdminConfiguration ? Configuration.AdminTemplate.Materials : Configuration.UserTemplate.Materials); }
		}

		#region IEnumerable<Construction> Members
		public IEnumerator<Material> GetEnumerator() {
			return new MaterialListEnumerator(this.type, this.categoryFilter, this.onlyAdditional);
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return new MaterialListEnumerator(this.type, this.categoryFilter, this.onlyAdditional);
		}
		#endregion

		#region ICollection<Construction> Members
		public void Add(Material item) {
			this.Materials.Add(item);
		}

		public void Clear() {
			this.Materials.Clear();
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
			bool result = this.Materials.Remove(item);
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
			// TODO
			int i = 0;
			int current = 0;

			while
				(
					current < this.Materials.Count && (
						(
							this.categoryFilter != null && (
								this.Materials[current].Category == null ||
								this.Materials[current].Category.Type != this.categoryFilter.Value
							)
						) || (
							this.onlyAdditional &&
							!this.Materials[current].Additional
						)
					)
				) {
				current++;
			}

			/*while (current < this.Materials.Count && (this.categoryFilter != null && (this.Materials[current].Category == null || this.categoryFilter.Value != this.Materials[current].Category.Type))) {
				current++;
			}*/
			while (i < index && current < this.Materials.Count) {
				current++;
				/*while (current < this.Materials.Count && (this.categoryFilter != null && (this.Materials[current].Category == null || this.categoryFilter.Value != this.Materials[current].Category.Type))) {
					current++;
				}*/
				while
					(
						current < this.Materials.Count && (
							(
								this.categoryFilter != null && (
									this.Materials[current].Category == null ||
									this.Materials[current].Category.Type != this.categoryFilter.Value
								)
							) || (
								this.onlyAdditional &&
								!this.Materials[current].Additional
							)
						)
					) {
					current++;
				}
				i++;
			}
			return current < this.Materials.Count ? current : -1;
		}

		public void Insert(int index, Material item) {
			int node = this.GetNode(index);
			if (node < 0) {
				this.Materials.Insert(this.Materials.Count, item);
			} else {
				this.Materials.Insert(node, item);
			}
		}

		public void RemoveAt(int index) {
			int node = this.GetNode(index);
			if (node >= 0) {
				this.Materials.RemoveAt(node);
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
					return this.Materials[node];
				}
			}
			set {
				int node = this.GetNode(index);
				if (node < 0) {
					throw new ArgumentOutOfRangeException();
				} else {
					this.Materials[node] = value;
				}
			}
		}
		#endregion

		#region IList Members
		public int Add(object value) {
			if (value is Material) {
				this.Materials.Add((Material)value);
				return this.Materials.Count - 1;
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
				this.Materials.Remove((Material)value);
			}
		}

		object System.Collections.IList.this[int index] {
			get {
				int node = this.GetNode(index);
				if (node < 0) {
					throw new ArgumentOutOfRangeException();
				} else {
					return this.Materials[node];
				}
			}
			set {
				int node = this.GetNode(index);
				if (node < 0) {
					throw new ArgumentOutOfRangeException();
				} else {
					this.Materials[node] = (Material)value;
				}
			}

			/*get {
				int node = this.GetNode(index);
				if (node < 0) {
					throw new ArgumentOutOfRangeException();
				} else {
					IEnumerator<Material> it = this.GetEnumerator();
					it.MoveNext();
					for (int i = 0; i < index; i++ ) {
						it.MoveNext();
					}
					return it.Current;
					//return this.Materials[node];
				}
			}
			set {
				if (value is Material) {
					int node = this.GetNode(index);
					if (node < 0) {
						throw new ArgumentOutOfRangeException();
					} else {
						IEnumerator<Material> it = this.GetEnumerator();
						it.MoveNext();
						for (int i = 0; i < index; i++) {
							it.MoveNext();
						}
						//return it.Current;
						int t = this.Materials.IndexOf(it.Current);
						this.Materials[t] = (Material)value;
						//this.Materials[node] = (Material)value;
					}
				} else {
					throw new ArgumentException("Object to set is not a material");
				}
			}*/
		}
		#endregion

		#region IBindingList Members
		public void AddIndex(PropertyDescriptor property) {
		}

		public object AddNew() {
			/*if (this.categoryFilter == null) {
				throw new NotSupportedException("The method or operation is not implemented");
			}*/
			Material m = new Material();
			if (this.categoryFilter != null) {
				m.Category = Configuration.UserTemplate.GetUserDefinedCategoryForCategoryType(this.categoryFilter.Value);
			}
			m.UserDefined = !this.admin;
			m.Additional = true;
			m.IsNew = true;
			this.Materials.Add(m);
			return m;
		}

		public bool AllowEdit {
			get { return true; }
		}

		public bool AllowNew {
			//get { return this.categoryFilter == CategoryType.General || this.categoryFilter == CategoryType.Insulation; }
			get { return true; }
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
				string filter = (this.categoryFilter == null ? "" : ((int)this.categoryFilter.Value).ToString()) +
					";" + (this.onlyAdditional ? "1" : "");
				if (filter.EndsWith(";")) {
					filter = filter.Substring(0, filter.Length - 1);
				}
				if (filter.Length == 0) {
					filter = null;
				}
				return filter;
				//return (this.categoryFilter == null ? null : ((int)this.categoryFilter.Value).ToString());
			}
			set {
				int i;
				string cat;
				string add;
				this.categoryFilter = null;
				this.onlyAdditional = false;
				if (value != null) {
					if (value.Contains(";")) {
						cat = value.Substring(0, value.IndexOf(';'));
						add = value.Substring(value.IndexOf(';') + 1);
					} else {
						cat = value;
						add = null;
					}
					if (Int32.TryParse(cat, out i)) {
						if (i == ((int)CategoryType.General)) {
							this.categoryFilter = CategoryType.General;
						} else if (i == ((int)CategoryType.Floor)) {
							this.categoryFilter = CategoryType.Floor;
						} else if (i == ((int)CategoryType.Wall)) {
							this.categoryFilter = CategoryType.Wall;
						} else if (i == ((int)CategoryType.Ceiling)) {
							this.categoryFilter = CategoryType.Ceiling;
						} else if (i == ((int)CategoryType.Distributor)) {
							this.categoryFilter = CategoryType.Distributor;
						} else if (i == ((int)CategoryType.Insulation)) {
							this.categoryFilter = CategoryType.Insulation;
						} else {
							this.categoryFilter = null;
						}
					}
					if (Int32.TryParse(add, out i)) {
						if (i == 1) {
							this.onlyAdditional = true;
						}
					}
				}
				/*} else if (Int32.TryParse(value, out i)) {
					if (i == ((int)CategoryType.General)) {
						this.categoryFilter = CategoryType.General;
					} else if (i == ((int)CategoryType.Floor)) {
						this.categoryFilter = CategoryType.Floor;
					} else if (i == ((int)CategoryType.Wall)) {
						this.categoryFilter = CategoryType.Wall;
					} else if (i == ((int)CategoryType.Ceiling)) {
						this.categoryFilter = CategoryType.Ceiling;
					} else if (i == ((int)CategoryType.Distributor)) {
						this.categoryFilter = CategoryType.Distributor;
					} else if (i == ((int)CategoryType.Insulation)) {
						this.categoryFilter = CategoryType.Insulation;
					} else {
						this.categoryFilter = null;
					}
				}*/
			}
		}

		public void RemoveFilter() {
			this.categoryFilter = null;
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
			get { return this.categoryFilter; }
			set { this.categoryFilter = value; }
		}

		public bool ShowOnlyAdditional {
			get { return this.onlyAdditional; }
			set { this.onlyAdditional = value; }
		}
	}

	public class MaterialListEnumerator : IEnumerator<Material> {

		private Configuration.ConfigurationType type;
		private int currentNode;
		private bool finished;
		private Nullable<CategoryType> categoryFilter;
		private bool showOnlyAdditional = false;

		internal MaterialListEnumerator(Configuration.ConfigurationType type, Nullable<CategoryType> filter, bool showOnlyAdditional) {
			this.type = type;
			this.currentNode = -1;
			this.finished = false;
			this.categoryFilter = filter;
			this.showOnlyAdditional = showOnlyAdditional;
		}

		private List<Material> List {
			get { return (this.type == Configuration.ConfigurationType.AdminConfiguration ? Configuration.AdminTemplate.Materials : Configuration.UserTemplate.Materials); }
		}

		#region IEnumerator<Construction> Members
		public Material Current {
			get {
				if (this.currentNode <= 0 || this.finished) {
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
				if (this.currentNode <= 0 || this.finished) {
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
				while 
					(
						this.currentNode < this.List.Count && (
							(
								this.categoryFilter != null && (
									this.List[currentNode].Category == null ||
									this.List[this.currentNode].Category.Type != this.categoryFilter.Value
								)
							) || (
								this.showOnlyAdditional &&
								!this.List[this.currentNode].Additional
							)
						)
					) {
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
