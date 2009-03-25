using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;

namespace Europlan.Licensing {
	[XmlRoot("license")]
	public class License : AbstractLicense<LicensedModule, LicensedSystem> {

		protected override LicensedModule NewModule(string moduleName) {
			return new LicensedModule(moduleName);
		}

		public static License LoadLicense(Stream stream) {
			XmlSerializer serializer = new XmlSerializer(typeof(License));
			return (License)serializer.Deserialize(stream);
		}

		public void SaveLicense(Stream stream) {
			XmlSerializer serializer = new XmlSerializer(typeof(License));
			serializer.Serialize(stream, this);
		}
	}

	/*public class ModuleCollection : ICollection<LicensedModule> {

		private List<LicensedModule> modules = new List<LicensedModule>();
		private License license;

		public ModuleCollection(License license) {
			this.license = license;
		}

		#region ICollection<LicensedModule> Members
		public void Add(LicensedModule item) {
			this.modules.Add(item);
		}

		public void Clear() {
			this.modules.Clear();
		}

		public bool Contains(LicensedModule item) {
			return this.modules.Contains(item);
		}

		public void CopyTo(LicensedModule[] array, int arrayIndex) {
			this.modules.CopyTo(array, arrayIndex);
		}

		public int Count {
			get { return this.modules.Count; }
		}

		public bool IsReadOnly {
			get { return false; }
		}

		public bool Remove(LicensedModule item) {
			return this.modules.Remove(item);
		}
		#endregion

		#region IEnumerable<LicensedModule> Members
		public IEnumerator<LicensedModule> GetEnumerator() {
			return this.modules.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return this.modules.GetEnumerator();
		}
		#endregion
	}

	public class SystemCollection : ICollection<LicensedSystem> {

		private List<LicensedSystem> modules = new List<LicensedSystem>();
		private License license;

		public SystemCollection(License license) {
			this.license = license;
		}

		#region ICollection<LicensedSystem> Members
		public void Add(LicensedSystem item) {
			this.modules.Add(item);
		}

		public void Clear() {
			this.modules.Clear();
		}

		public bool Contains(LicensedSystem item) {
			return this.modules.Contains(item);
		}

		public void CopyTo(LicensedSystem[] array, int arrayIndex) {
			this.modules.CopyTo(array, arrayIndex);
		}

		public int Count {
			get { return this.modules.Count; }
		}

		public bool IsReadOnly {
			get { return false; }
		}

		public bool Remove(LicensedSystem item) {
			return this.modules.Remove(item);
		}
		#endregion

		#region IEnumerable<LicensedSystem> Members
		public IEnumerator<LicensedSystem> GetEnumerator() {
			return this.modules.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return this.modules.GetEnumerator();
		}
		#endregion
	}*/
}
