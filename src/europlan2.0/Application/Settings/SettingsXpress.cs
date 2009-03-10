#region License and Copyright
/* ------------------------------------------------------------------------------
*							SettingsXpress v1.1.00.00 
*
*  Copyright (c) 2003 Amber Alpha Star All rights reserved.
*
*  
*  SettingsXp Library provides a simple to use, 
*  flexible, dynamic, and universal, XML based data store 
*  for your application and user settings. 
*
*	File: SettingsXpress.cs 
*	Created : 5/8/2003 6:58:42 PM by : Amber
*
*		This library is free software; you can redistribute it and/or
*		modify it under the terms of the GNU Lesser General Public
*		License as published by the Free Software Foundation; either
*		version 2.1 of the License, or (at your option) any later version.
*
*		This library is distributed in the hope that it will be useful,
*		but WITHOUT ANY WARRANTY; without even the implied warranty of
*		MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
*		Lesser General Public License for more details.
*
*		You should have received a copy of the GNU Lesser General Public
*		License along with this library; if not, write to the Free Software
*		Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*
*		The use and distribution terms for this software are contained in the file
*		named license.txt, which can be found in the root of this distribution.
*		By using this software in any fashion, you are agreeing to be bound by the
*		terms of this license.
* 
*       You must not remove this notice, or any other, from this software.
*                                                                               
* ------------------------------------------------------------------------------
*/

#endregion

#region Acknowledgements

#endregion

#region Contact Info

/* ------------------------------------------------------------------------------
 *  
 *                          Contact Information
 *  
 *    SettingsXPress is managed in a GotDotNet Workspace, which provides
 *    a central managment space for the library and it's contributors. 
 *    You can participate in the workspace, post comments, suggestions, fixes, bugs, 
 *    and find New releases.
 *	  To Participate in this workspace follow the WorkSpace link.
 * 
 *    WorkSpace: http://www.gotdotnet.com/community/workspaces/workspace.aspx?id=cd9f2d7e-77bf-4231-b741-41b862f7c587
 *    Comments : mailto:AmberAlphaStar@hotmail.com
 *  
 * --------------------------------------------------------------------------------
 */

#endregion

#region Release Notes
/*-----------------------------------------------------------------------------------------------------
 *										Release Information.
 *-----------------------------------------------------------------------------------------------------
 * 04/20/2003 - XmlRegistry Version: 0.5.00.00 Released
 *				
*/
#endregion

#region Todo List
//TODO: Verify all settingKeyNames and settingNames are valid XML Names, and handle invalid names.
#endregion


/*
 * The following references are required to build this Library
 * System
 * System.Data
 * System.Windows Forms
 * System.Xml 
*/
using System;
using System.Xml;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using log4net;

namespace Star.SettingsXpress {

	#region SettingsFile
	/// <summary>
	/// Provides access to the settings file and the root <see cref="SettingsKey"/> in the settings file.
	/// <seealso cref="SettingsKey"/>
	/// </summary>
	/// 
	/// 
	/// <remarks>
	///The SettingsFile acts as a central storage for your 
	///application and component settings. 
	///
	///<para>
	///The settings file is organized in a hierarchical format, based on a logical 
	///ordering of <see cref="SettingsKey">SettingsKeys</see>  stored within it 
	///</para>
	///
	/// 
	/// For example, applications can use the SettingsFile
	/// for storing information that needs to be preserved once the application is closed, 
	/// and access that same information when the application is reloaded. 
	/// For instance, you can store color preferences, screen locations, 
	/// or the size of the window. 
	/// 
	///<para>
	///A <see cref="SettingsKey"/> is the base unit of organization in the settings file, and 
	///can be compared to folders. 
	///</para>
	///
	///<para>
	///Each <see cref="SettingsKey"/> can  have multiple settings stored in it 
	///(a setting can be compared to a file within a folder), 
	///SettingsKeys are used to store the actual settings of your application components, and users. 
	///Each setting holds one particular value, which can be 
	///retrieved or updated when required. 
	///</para>
	///  
	///  <para>
	///  Use the <see cref="Settings"/> property to access
	///  the root <see cref="SettingsKey"/> in the settings file. The root SettingsKey is readonly 
	///  since the settings file depends on it's existence. 
	///  </para>
	///  
	///  <para>
	///  By default the settings file is saved to the current users appData path with the
	///  default name 'Settings.Xml'. To specify a diferent name and location use the
	///  <see cref="Create"/> method.
	///  </para>
	///  
	///  <para>Once you have made changes to the settings file use the <see cref="Update"/>
	///  method to save your changes to the file.
	///  </para>
	///  </remarks>
	///  
	public sealed class SettingsFile {

		#region Constants
		const string RES_NullParameter = "The specified string parameter is null or empty.";
		const string RES_XmlFormat = "Could not Parse or Load the SettingsFile";
		const string RES_RootName = "Settings";
		const string RES_FileName = "Settings.xml";
		#endregion

		#region Class Fields

		static System.Xml.XmlDocument data;
		private static SettingsKey root;
		static bool isOpen = false;
		static bool hasChanges = false;
		static string fileName;
		private static readonly ILog log = LogManager.GetLogger(typeof(SettingsFile));



		#endregion

		#region Class Constructors

		private SettingsFile() {

		}


		static SettingsFile() {
			string path = System.Windows.Forms.Application.UserAppDataPath;
			if (path.IndexOf(System.Windows.Forms.Application.ProductVersion) >= 0) {
				path = path.Substring(0, path.IndexOf(System.Windows.Forms.Application.ProductVersion));
			}

			fileName = Path.Combine(path, RES_FileName);
		}

		#endregion

		#region Private Class Methods


		#region open
		//open
		private static void open() {
			//is parameter fileName null or empty
			if (fileName == null | fileName == string.Empty) {
				throwParameterNullException("fileName");
			}

			data = new XmlDocument();


			// if file exsist load, if not create
			if (File.Exists(fileName)) {
				try {
					data.Load(fileName);
				} catch (XmlException ex) {
					// instead of throwing an exception, delete the settings file
					// throwXmlFormatException(ex);
					File.Delete(fileName);
					data.AppendChild(data.CreateElement(RES_RootName));
					root = new SettingsKey(data.DocumentElement, true);
					log.Error("Error reading settings file: ", ex);
				}
			} else {
				data.AppendChild(data.CreateElement(RES_RootName));
				root = new SettingsKey(data.DocumentElement, true);
			}

			//set rootkey  to readonly
			root = new SettingsKey(data.DocumentElement, true);

			//set handlers
			XmlNodeChangedEventHandler handler = new XmlNodeChangedEventHandler(handleChanges);
			data.NodeChanged += handler;
			data.NodeInserted += handler;
			data.NodeRemoved += handler;

			//set dirty flag
			hasChanges = false;

			//set Open
			isOpen = true;
		}
		#endregion

		#region update
		//update
		internal static void update() {
			if (hasChanges) {
				data.Save(fileName);
			}
			hasChanges = false;
		}

		#endregion

		#region exceptions

		private static void throwParameterNullException(string parameterName) {
			throw new ArgumentNullException(parameterName, RES_NullParameter);
		}

		private static void throwXmlFormatException(XmlException internalException) {
			string message = string.Concat(RES_XmlFormat, fileName);
			throw new SettingsFormatException(message, internalException.LineNumber, internalException.LinePosition, internalException);
		}
		#endregion

		#region handleChanges
		//handles the XmlDocument Change events
		private static void handleChanges(object sender, XmlNodeChangedEventArgs e) {
			hasChanges = true;
		}
		#endregion

		#endregion

		#region Public Class Methods

		#region Create

		/// <summary>
		/// Creates or loads the specified settings file
		/// </summary>
		/// 
		/// <param name="fileName">
		/// The name of the settings file to load.
		/// </param>
		/// 
		/// <remarks>
		///Use this method if you want to specify a specific file name and location,
		///for the settings file. If the specified file exists it is loaded, otherwise
		///it is created. 
		///</remarks>
		public static void Create(string fileName) {
			SettingsFile.fileName = fileName;
			open();
		}

		/// <summary>
		/// Creates or loads the settings file using the default settings.
		/// </summary>
		public static void Create() {
			string path = System.Windows.Forms.Application.UserAppDataPath;
			if (path.IndexOf(System.Windows.Forms.Application.ProductVersion) >= 0) {
				path = path.Substring(0, path.IndexOf(System.Windows.Forms.Application.ProductVersion));
			}
			fileName = Path.Combine(path, RES_FileName);
		}

		#endregion

		#region Update
		//Update
		/// <summary>
		/// Updates the settings file with any changes made.
		/// </summary>
		/// 
		/// <remarks>
		/// It is the clients responsibility to ensure that the
		/// settings file is available for write access 
		/// if the settings cannot be updated
		/// an IOException will be thrown, and the client can recover 
		/// from the exception using the WriteTo method
		/// </remarks>
		public static void Update() {
			update();
		}



		#endregion

		#region WriteTo
		//WriteTo
		/// <summary>
		/// Writes the SettingsFile content to the specified XmlWriter
		/// </summary>
		/// <param name="w">
		/// The XmlWriter to which you want to write to. 
		/// </param>
		public static void WriteTo(XmlWriter w) {
			data.WriteTo(w);
		}

		#endregion

		#endregion

		#region Class Properties


		#region Settings
		//Settings
		/// <summary>
		/// The root <see cref="SettingsKey"/> in the setting file.
		/// <seealso cref="SettingsKey"/>
		/// </summary>
		/// <example>
		/// The following example method creates a sub SettingKey using the root SettingsKey
		/// <code>
		/// 
		/// private SettingsKey GetSettingsKey()
		/// {
		///		SettingsKey dataSettings = SettingsFile.Settings.CreateSubKey("Data");
		///		return dataSettings;
		/// }
		/// </code>
		/// </example>
		public static SettingsKey Settings {
			get {
				if (!isOpen) {
					open();
				}
				return root;
			}
		}
		#endregion

		#endregion

	}
	#endregion

	#region SettingsKey

	/// <summary>
	/// Represents a settings section in the <see cref="SettingsFile"/>.
	/// </summary>
	/// <remarks>
	///<para>
	///The settings file is organized in a hierarchical format, based on a logical 
	///ordering of the SettingsKeys stored within it 
	///</para>
	///
	///<para>
	///A SettingsKey is the base unit of organization in the settings file, and 
	///can be compared to a folder to store settings in. A particular SettingsKey
	///can have subkeys (just as a folder can have subfolders). SettingsKeys can be
	///added and deleted to the settings file. 
	///</para>
	///
	///<para>
	///Each SettingsKey can  have multiple settings stored within it, 
	///Each setting holds one particular value, which can be 
	///retrieved or updated when required. 
	///</para>
	///
	///<para>
	///When storing information in the settings file, select the appropriate 
	///location based upon the type of information being stored and be sure 
	///to avoid destroying settings created by other components in your 
	///application, since this can cause those components to exhibit unexpected 
	///behavior, and can also have an adverse effect upon your application. 
	///</para>
	///
	///<para>
	///NOTE: All SettingsKey and Setting Names must conform to W3C Extended Markup Language recommendation
	///</para>
	///
	/// </remarks>
	public sealed class SettingsKey : System.IDisposable {
		#region Constants

		const string RES_NullParameter = "The specified string parameter is null or empty.";
		const string RES_ReadOnlyKey = "The current SettingsKey is the Root SettingsKey or read-only.";
		const string RES_InvalidKeyRef = "The specified paramter is not a valid reference to a SettingsKey.";
		const string RES_InvalidValueRef = "The specified paramter is not a valid reference to a setting.";
		const string RES_InvalidDeleteWithChildKeys = "The specified SettingsKey has subkeys.Cannot delete a SettingsKey with  subkeys. To delete a SettingsKey that has  subkeys use the DeleteSettingsKeyTree method.";
		const string RES_PointStartString = "{X=";
		const string RES_PointEndString = "Y=";
		const char RES_PointDelimChar = ',';
		const char RES_PointEndChar = '}';
		const string RES_SizeStartString = "{Width=";
		const string RES_SizeEndString = " Height=";
		const char RES_SizeDelimChar = ',';
		const char RES_SizeEndChar = '}';
		const string RES_ColorParseError = "Color could not be parsed";
		const string RES_KnownColorStartString = "COLOR[";
		const string RES_UnKnownColorStartString = "RGB[";
		const char RES_ColorEndChar = ']';
		#endregion

		#region Instance Fields

		private XmlElement data;

		internal bool isReadOnly = true;

		#endregion

		#region Instance Constructors

		internal SettingsKey(XmlElement data, bool isReadOnly) {
			this.data = data;
			this.isReadOnly = isReadOnly;
		}

		private SettingsKey() {
		}

		#endregion

		#region Indexers

		/// <summary>
		/// Returns the specified subkey
		/// You can specify a name of a subkey or a relative path
		/// to a subkey. In C# this is the Indexer 
		/// </summary>
		public SettingsKey this[string path] {
			get {
				return this.CreateSubKey(path);
			}
		}

		#endregion

		#region Public Instance Properties

		#region Name


		//FullName
		/// <summary>
		/// The name including the absolute path of the SettingsKey.
		/// </summary>
		/// <remarks>
		/// The path of the SettingsKey always starts at a Root SettingsKey 
		/// </remarks>
		public string FullName {
			get {
				string str;
				// if i am root return name
				if (data == SettingsFile.Settings.data) {
					str = String.Concat("/", data.Name);
				} else {
					// this occurs when key has been deleted(Orphan Key)
					if ((XmlElement)data.ParentNode == null) {
						return this.data.Name;
					}
					//recursively call Name on parent until we find Root.
					SettingsKey parentKey = new SettingsKey((XmlElement)data.ParentNode, true);
					string parentName = parentKey.FullName;
					str = String.Concat(parentName, "/", data.Name);
				}
				return str;
			}
		}

		//Name
		/// <summary>
		/// The name of the SettingsKey.
		/// </summary>
		public string Name {
			get {
				return this.data.Name;
			}
		}
		#endregion

		#region SubKeyCount
		//SubKeyCount
		/// <summary>
		/// Retrieves the count of subkeys at the base level, 
		/// for the current key.
		/// </summary>
		/// 
		/// <setting>
		/// The number of subkeys for the current key.
		/// </setting>
		/// 
		/// <remarks>
		/// This property does not recursively count names. 
		/// It only returns the count of names on the base 
		/// level from which it was called.
		/// </remarks>
		public int SubKeyCount {
			get {
				return data.ChildNodes.Count;
			}
		}
		#endregion

		#region SettingsCount
		//SettingsCount
		/// <summary>
		/// Retrieves the count of settings stored in the key.
		/// </summary>
		/// <setting>
		/// A count of the number of settings stored in the key.
		/// </setting>
		public int SettingsCount {
			get {
				return data.Attributes.Count;
			}
		}
		#endregion

		#endregion

		#region Public Instance Methods

		#region CreateSubkey
		//CreateSubKey
		/// <summary>
		/// Creates a new subkey or opens an existing subkey.
		/// </summary>
		/// 
		/// <param name="subkeyName">
		/// Name or the path of subkey to create or open.
		/// </param>
		/// 
		/// <returns>
		/// Returns the SettingsKey, with read-write access.
		/// Returns null if the operation failed.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The specified parameter string 'subkeyName' is null or an empty string.
		/// </exception>
		/// 
		///<remarks>
		///If an invalid character is used in any part of the specified
		///subKey name or path the method returns a null reference.
		///If the subkey specified already exsist, this method will 
		///return the subkey with read-write access. If the specified 
		///subkey does not exsist this method will create it and open 
		///it for read-write access. 
		///</remarks> 
		public SettingsKey CreateSubKey(string subkeyName) {
			//trim input
			subkeyName = subkeyName.Trim();
			subkeyName = subkeyName.TrimStart(new char[] { '/' });
			subkeyName = subkeyName.TrimEnd(new char[] { '/' });


			//store state if operation fails
			string xmlData = this.data.InnerXml;

			#region CheckConditions


			//is subkey name null or an empty string
			if (subkeyName == null || subkeyName == String.Empty) {
				throwParameterNullException("subkeyName");
			}
			#endregion

			//split input path
			string[] strs1 = subkeyName.Split(new char[] { '/' });
			XmlElement xmlElement = data;
			XmlElement xmlElement2;



			int index = 0;

			//find all exsisting nodes in path
			while ((xmlElement2 = xmlElement[(strs1[index].Trim())]) != null) {
				xmlElement = xmlElement2;

				index++;
				if (index == (int)strs1.Length) {
					return new SettingsKey(xmlElement, false);
				}
			}

			//iterate strings in path and create if neccasary
			for (int i = index; i < (int)strs1.Length; i++) {

				try {
					//once we've created one we know every other is new
					xmlElement = (XmlElement)xmlElement.AppendChild(xmlElement.OwnerDocument.CreateElement((strs1[i].Trim())));
				} catch {
					//operation failed restore state
					this.data.InnerXml = xmlData;
					return null;
				}

			}
			return new SettingsKey(xmlElement, false);
		}


		#endregion

		#region DeleteSubKey

		//DeleteSubKey
		/// <summary>
		/// Deletes the specified subkey.
		/// </summary>
		/// 
		/// <param name="subkeyName">
		/// The name of the subkey to delete.
		/// </param>
		/// 
		/// <exception cref="UnauthorizedAccessException">
		/// The current key is root SettingsKey or is read-only. 
		/// </exception>
		/// 
		/// <exception cref="InvalidOperationException">
		/// The subkey specified has child subkeys. 
		/// To delete a subkey that has child subkeys 
		/// use DeleteSettingsKeyTree method. 
		/// </exception>
		/// 
		/// <exception cref="ArgumentException">
		/// The specified parameter 'subkeyName' is not a valid 
		/// reference to a SettingsKey. 
		/// </exception>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The paramter string 'subkey' is null or an 
		/// empty string (Nothing in Visual Basic). 
		/// </exception>
		/// 
		/// <remarks>
		/// The string parameter 'subkeyName' is case-sensitive. 
		/// This method can only be used on a key that has write access. 
		/// To delete child subkeys, use DeleteSettingsKeyTree. 
		/// Use caution when deleting keys 
		/// </remarks>
		public void DeleteSubKey(string subkeyName) {

			//All conditions met delete subkey
			DeleteSubKey(subkeyName, true);
		}

		/// <summary>
		/// Deletes the specified subkey, and optionally raises an exception
		/// if the specified subkey is missing.
		/// </summary>
		/// 
		/// <param name="subkeyName">
		/// The name of the subkey to delete.
		/// </param>
		/// 
		/// <param name="throwOnMissingSubkey">
		/// Indicates whether an exception should be raised if the 
		/// specified subkey cannot be found. If this argument is set 
		/// to true and the specified subkey does not exist, 
		/// then an exception is raised. If this argument is set to false 
		/// and the specified subkey does not exist, then no action is taken. 
		/// </param>
		/// 
		/// <exception cref="UnauthorizedAccessException">
		/// The current key is a base(root) key or is read-only.
		/// </exception>
		/// 
		/// <exception cref="InvalidOperationException">
		/// The subkey to delete has subkeys,to delete a subkey 
		/// that has  subkeys use the DeleteSettingsKeyTree method. 
		/// </exception>
		/// 
		/// <exception cref="ArgumentException">
		/// The specified subkey is not a valid reference to a SettingsKey 
		/// and the throwOnMissingSubKey parameter is set to true. 
		/// </exception>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The specified parameter string 'subkeyName' is a null reference or 
		/// an empty string (Nothing in Visual Basic). 
		/// </exception>
		/// 
		/// <remarks>
		/// The string 'subkeyName' is case-sensitive. 
		/// This method can only be used on a SettingsKey opened with write access. 
		/// To delete child subkeys, use the DeleteSettingsKeyTree method. 
		/// Use caution when deleting settings keys. 
		/// </remarks>
		public void DeleteSubKey(string subkeyName, bool throwOnMissingSubkey) {
			this.internalDeleteSubKey(subkeyName, throwOnMissingSubkey, false);
		}

		#endregion

		#region DeleteSettingsKeyTree

		//DeleteSettingsKeyTree
		/// <summary>
		/// Deletes a subkey and all child subkeys within the subkey recursively.
		/// </summary>
		/// 
		/// <param name="subkeyName">
		/// The SettingsKey to delete. 
		/// </param>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The specified pararameter string 'subkey' 
		/// is a null reference or an empty string. 
		/// </exception>
		/// 
		/// <exception cref="ArgumentException">
		/// The subkey parameter does not match a valid SettingsKey.
		/// </exception>
		/// 
		/// <exception cref="UnauthorizedAccessException">
		/// The current key is the root SettingsKey or is read-only.
		/// </exception>
		/// 
		/// <remarks>
		/// CAUTION:Deleting a particular key will remove all 
		/// entries below the key in the tree. No warning will be provided. 
		/// If you want to delete a subkey only when it has no child subkeys, 
		/// use the DeleteSubKey method instead. 
		/// <para>
		/// The string 'subkeyName' is case-sensitive. 
		/// </para>
		/// </remarks>
		public void DeleteSettingsKeyTree(string subkeyName) {
			this.internalDeleteSubKey(subkeyName, true, true);
		}

		#endregion

		#region DeleteSetting

		//DeleteSetting
		/// <summary>
		/// Deletes the specified setting stored in this key.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to delete.
		/// </param>
		/// 
		/// <exception cref="UnauthorizedAccessException">
		/// This SettingsKey is root SettingsKey or is read-only.
		/// </exception>
		/// 
		/// <exception cref="ArgumentException">
		/// The specified setting name is not a valid reference 
		/// to a setting stored in this SettingsKey. 
		/// </exception>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The specified parameter string 'settingName' 
		/// is a null reference or an empty string.
		/// </exception>
		/// 
		/// <remarks>
		/// The string 'settingName' is case-sensitive.
		/// </remarks>
		public void DeleteSetting(string settingName) {
			this.DeleteSetting(settingName, true);
		}

		/// <summary>
		/// Deletes the specified setting from a SettingsKey, and optionally 
		/// raises an exception if the setting does not exsist.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to delete.
		/// </param>
		/// 
		/// <param name="throwOnMissingValue">
		/// Indicates whether an exception should be raised if the specified 
		/// setting cannot be found. If this argument is set to true 
		/// and the specified setting does not exist then an exception is raised. 
		/// If this argument is set to false and the specified setting does not exist, 
		/// then no action is taken.
		/// </param>
		/// 
		/// <exception cref="UnauthorizedAccessException">
		/// The current key is the root SettingsKey or is read-only. 
		/// </exception>
		/// 
		/// <exception cref="ArgumentException">
		/// The specified setting name is not a valid reference to a setting 
		/// stored in the current SettingsKey, and throwOnMissing is set to true.
		/// </exception>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The parameter string 'name' is a null reference or an empty string.
		/// </exception>
		/// 
		/// <remarks>
		/// If throwOnMissingValue is set to false, there is no way to tell 
		/// if the deletion was successful (without subsequently trying to access 
		/// the setting just deleted). Therefore, use caution when deleting settings 
		/// from the settings in this manner.
		/// <para>
		/// The string 'settingName' is case-sensitive.
		/// </para>
		/// </remarks>
		public void DeleteSetting(string settingName, bool throwOnMissingValue) {
			#region conditions

			//is this key readonly
			if (isReadOnly) {
				throwKeyReadOnlyException();
			}

			//is the parameter settingName null or an empty string
			if (settingName == null || settingName == String.Empty) {
				throwParameterNullException("name");
			}

			//is setting missing and throwOnMissingValue set to true
			if (!data.HasAttribute(settingName) & throwOnMissingValue) {
				throwInvalidValueRefException("name");
			}

			#endregion

			//all conditions met and setting exsist delete setting
			if (data.HasAttribute(settingName)) {
				data.RemoveAttribute(settingName);
			}
		}

		#endregion

		#region GetSubKeyNames
		//GetSubKeyNames
		/// <summary>
		/// Retrieves an array of strings that contains all the subkey names.
		/// </summary>
		/// 
		/// <returns>
		/// An array of strings that contains the names of 
		/// the subkeys for the current SettingsKey.
		/// </returns>
		/// 
		/// <remarks>
		/// This method does not recursively find names. 
		/// It returns the names on the base level it was called from.
		/// </remarks>
		public string[] GetSubKeyNames() {
			string[] names;
			int index;
			XmlElement node;
			string[] outNames;
			IEnumerator enumerator;

			names = new String[checked((uint)this.data.ChildNodes.Count)];
			index = 0;
			enumerator = this.data.ChildNodes.GetEnumerator();


			while (enumerator.MoveNext()) {
				node = (XmlElement)enumerator.Current;
				names[index] = node.Name;
				index++;
			}


			outNames = names;
			return outNames;
		}

		#endregion

		#region GetSettingNames
		//GetSettingNames
		/// <summary>
		/// Retrieves an array of strings that contains all 
		/// the setting names associated with this SettingsKey. 
		/// </summary>
		/// 
		/// <returns>
		/// An array of strings that contains the setting names for the current key.
		/// </returns>
		/// 
		/// <remarks>
		/// If no setting names for the key are found, an empty array is returned.
		/// </remarks>
		public string[] GetSettingNames() {
			string[] names;
			int index;
			XmlAttribute attribute;
			string[] outNames;
			IEnumerator enumerator;

			names = new String[checked((uint)this.data.Attributes.Count)];
			index = 0;
			enumerator = this.data.Attributes.GetEnumerator();


			while (enumerator.MoveNext()) {
				attribute = (XmlAttribute)enumerator.Current;
				names[index] = attribute.Name;
				index++;
			}


			outNames = names;
			return outNames;
		}

		#endregion

		#region GetSetting

		//GetSetting
		/// <summary>
		/// Retrieves the setting associated with the current key, 
		/// 
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the  setting to retrieve.
		/// </param>
		/// 
		/// <returns>
		/// The string data associated with the 'settingName'.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The parameter string 'settingName' is null or an empty string.
		/// </exception>
		/// 
		/// <exception cref="ArgumentException">
		/// The specified 'settingName' does not refer to a valid setting.
		/// </exception>
		/// 
		/// <remarks>
		/// The parameter 'settingName' is  case-sensitive.Any setting type stored
		/// in the SettingsKey can be retrieved as a string setting.
		/// </remarks>
		public string GetSetting(string settingName) {

			string str;

			#region conditions

			// is parameter settingName null or an empty string
			if (settingName == null || settingName == String.Empty) {
				this.throwParameterNullException("name");
			}

			// does the setting exsist in this key
			if (!(this.data.HasAttribute(settingName))) {
				this.throwInvalidKeyRefException("name");
			}
			#endregion

			//all conditions met return setting
			str = this.data.Attributes[settingName].Value;
			return str;
		}


		/// <summary>
		/// Retrieves the specified string data associated with the setting, or returns the default 
		/// string setting you provide if the specified 'settingName' is not found.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to retrieve.
		/// </param>
		/// 
		/// <param name="defaultSetting">
		/// Default string setting to return if 'settingName' does not exist.
		/// </param>
		/// 
		/// <returns>
		/// The data associated with 'settingName', or 'defaultSetting' if 'settingName' is not found.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The parameter 'settingName' is null or empty
		/// </exception>
		/// 
		/// <remarks>
		/// The parameter 'settingName' is  case-sensitive.Any datatype stored in the
		/// registry can be retrieved as a string setting. The string representation 
		/// of the setting is returned.
		/// </remarks>
		public string GetSetting(string settingName, string defaultSetting) {
			string str;

			try {
				str = this.GetSetting(settingName);

			} catch (ArgumentNullException) {
				//settingName is empty, rethrow ArgumentNull
				throw;
			} catch (ArgumentException) {
				str = defaultSetting;
			}
			return str;
		}

		/// <summary>
		/// Retrieves the specified bool setting, or returns the default 
		/// bool setting you provide if the specified 'settingName' is not found.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to retrieve.
		/// </param>
		/// 
		/// <param name="defaultSetting">
		/// Default bool setting to return if 'settingName' does not exist.
		/// </param>
		/// 
		/// <returns>
		/// The data associated with 'settingName', or 'defaultSetting' if 'settingName' is not found.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The parameter 'settingName' is null or empty
		/// </exception>
		/// 
		/// <remarks>
		/// The parameter 'settingName' is  case-sensitive.
		/// If the data stored at the specified 'settingName' cannot be converted to the
		/// particular data type the 'defaultSetting' is returned.
		/// </remarks>
		public bool GetSetting(string settingName, bool defaultSetting) {
			bool returnValue = defaultSetting;
			string str = GetSetting(settingName, string.Empty);
			if (str != string.Empty) {
				try {
					returnValue = XmlConvert.ToBoolean(str);
				} catch {
					returnValue = defaultSetting;
				}
			}
			return returnValue;
		}

		/// <summary>
		/// Retrieves the specified byte setting, or returns the default 
		/// byte setting you provide if the specified 'settingName' is not found.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to retrieve.
		/// </param>
		/// 
		/// <param name="defaultSetting">
		/// Default byte setting to return if 'settingName' does not exist.
		/// </param>
		/// 
		/// <returns>
		/// The data associated with 'settingName', or 'defaultSetting' if 'settingName' is not found.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The parameter 'settingName' is null or empty
		/// </exception>
		/// 
		/// <remarks>
		/// The parameter 'settingName' is  case-sensitive.
		/// If the data stored at the specified 'settingName' cannot be converted to the
		/// particular data type the 'defaultSetting' is returned.
		/// </remarks>
		public byte GetSetting(string settingName, byte defaultSetting) {
			byte returnValue = defaultSetting;
			string str = GetSetting(settingName, string.Empty);
			if (str != string.Empty) {
				try {
					returnValue = XmlConvert.ToByte(str);
				} catch {
					returnValue = defaultSetting;
				}

			}
			return returnValue;
		}

		/// <summary>
		/// Retrieves the specified char setting, or returns the default 
		/// char setting you provide if the specified 'settingName' is not found.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to retrieve.
		/// </param>
		/// 
		/// <param name="defaultSetting">
		/// Default char setting to return if 'settingName' does not exist.
		/// </param>
		/// 
		/// <returns>
		/// The data associated with 'settingName', or 'defaultSetting' if 'settingName' is not found.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The parameter 'settingName' is null or empty
		/// </exception>
		/// 
		/// <remarks>
		/// The parameter 'settingName' is  case-sensitive.
		/// If the data stored at the specified 'settingName' cannot be converted to the
		/// particular data type the 'defaultSetting' is returned.
		/// </remarks>
		public char GetSetting(string settingName, char defaultSetting) {
			char returnValue = defaultSetting;
			string str = GetSetting(settingName, string.Empty);
			if (str != string.Empty) {
				try {
					returnValue = XmlConvert.ToChar(str);
				} catch {
					returnValue = defaultSetting;
				}

			}
			return returnValue;
		}

		/// <summary>
		/// Retrieves the specified DateTime setting, or returns the default 
		/// DateTime setting you provide if the specified 'settingName' is not found.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to retrieve.
		/// </param>
		/// 
		/// <param name="defaultSetting">
		/// Default DateTime setting to return if 'settingName' does not exist.
		/// </param>
		/// 
		/// <returns>
		/// The data associated with 'settingName', or 'defaultSetting' if 'settingName' is not found.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The parameter 'settingName' is null or empty
		/// </exception>
		/// 
		/// <remarks>
		/// The parameter 'settingName' is  case-sensitive.
		/// If the data stored at the specified 'settingName' cannot be converted to the
		/// particular data type the 'defaultSetting' is returned.
		/// </remarks>
		public DateTime GetSetting(string settingName, DateTime defaultSetting) {
			DateTime returnValue = defaultSetting;
			string str = GetSetting(settingName, string.Empty);
			if (str != string.Empty) {
				try {
					returnValue = XmlConvert.ToDateTime(str, XmlDateTimeSerializationMode.Utc);
				} catch {
					returnValue = defaultSetting;
				}

			}
			return returnValue;
		}

		/// <summary>
		/// Retrieves the specified DateTime setting with applied format, or returns the default 
		/// DateTime setting you provide if the specified 'settingName' is not found.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to retrieve.
		/// </param>
		/// 
		/// <param name="defaultSetting">
		/// Default DateTime setting to return if 'settingName' does not exist.
		/// </param>
		/// 
		/// <param name="format">The format structure to apply to the 
		///  DateTime. Valid formats include "yyyy-MM-ddTHH:mm:sszzzzzz" and its subsets. 
		///  The data is validated against this format. 
		/// </param>
		/// 
		/// <returns>
		/// The data associated with 'settingName', or 'defaultSetting' if 'settingName' is not found.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The parameter 'settingName' is null or empty
		/// </exception>
		/// 
		/// <remarks>
		/// The parameter 'settingName' is  case-sensitive.
		/// If the data stored at the specified 'settingName' cannot be converted to the
		/// particular data type the 'defaultSetting' is returned.
		/// </remarks>
		public DateTime GetSetting(string settingName, DateTime defaultSetting, string format) {
			DateTime returnValue = defaultSetting;
			string str = GetSetting(settingName, string.Empty);
			if (str != string.Empty) {
				try {
					returnValue = XmlConvert.ToDateTime(str, format);
				} catch {
					returnValue = defaultSetting;
				}

			}
			return returnValue;
		}

		/// <summary>
		/// Retrieves the specified DateTime setting with applied formats, or returns the default 
		/// DateTime setting you provide if the specified 'settingName' is not found.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to retrieve.
		/// </param>
		/// 
		/// <param name="defaultSetting">
		/// Default setting to return if 'settingName' does not exist.
		/// </param>
		/// 
		/// <param name="formats">An array containing the format 
		/// structures to apply to the DateTime. 
		/// Valid formats include "yyyy-MM-ddTHH:mm:sszzzzzz" and its subsets. 
		///</param>
		/// 
		/// <returns>
		/// The data associated with 'settingName', or 'defaultSetting' if 'settingName' is not found.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The parameter 'settingName' is null or empty
		/// </exception>
		/// 
		/// <remarks>
		/// The parameter 'settingName' is  case-sensitive.
		/// If the data stored at the specified 'settingName' cannot be converted to the
		/// particular data type the 'defaultSetting' is returned.
		/// </remarks>
		internal DateTime GetSetting(string settingName, DateTime defaultSetting, string[] formats) {
			DateTime returnValue = defaultSetting;
			string str = GetSetting(settingName, string.Empty);
			if (str != string.Empty) {
				try {
					returnValue = XmlConvert.ToDateTime(str, formats);
				} catch {
					returnValue = defaultSetting;
				}

			}
			return returnValue;
		}


		/// <summary>
		/// Retrieves the specified decimal setting, or returns the default 
		/// decimal setting you provide if the specified 'settingName' is not found.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to retrieve.
		/// </param>
		/// 
		/// <param name="defaultSetting">
		/// Default decimal setting to return if 'settingName' does not exist.
		/// </param>
		/// 
		/// <returns>
		/// The setting associated with 'settingName', or 'defaultSetting' if 'settingName' is not found.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The parameter 'settingName' is null or empty
		/// </exception>
		/// 
		/// <remarks>
		/// The parameter 'settingName' is  case-sensitive.
		/// If the data stored at the specified 'settingName' cannot be converted to the
		/// particular data type the 'defaultSetting' is returned.
		/// </remarks>
		public decimal GetSetting(string settingName, decimal defaultSetting) {
			decimal returnValue = defaultSetting;
			string str = GetSetting(settingName, string.Empty);
			if (str != string.Empty) {
				try {
					returnValue = XmlConvert.ToDecimal(str);
				} catch {
					returnValue = defaultSetting;
				}

			}
			return returnValue;
		}

		/// <summary>
		/// Retrieves the specified double setting, or returns the default 
		/// double setting you provide if the specified 'settingName' is not found.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to retrieve.
		/// </param>
		/// 
		/// <param name="defaultSetting">
		/// Default setting to return if 'settingName' does not exist.
		/// </param>
		/// 
		/// <returns>
		/// The setting associated with 'settingName', or 'defaultSetting' if 'settingName' is not found.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The parameter 'settingName' is null or empty
		/// </exception>
		/// 
		/// <remarks>
		/// The parameter 'settingName' is  case-sensitive.
		/// If the data stored at the specified 'settingName' cannot be converted to the
		/// particular data type the 'defaultSetting' is returned.
		/// </remarks>
		public double GetSetting(string settingName, double defaultSetting) {
			double returnValue = defaultSetting;
			string str = GetSetting(settingName, string.Empty);
			if (str != string.Empty) {
				try {
					returnValue = XmlConvert.ToDouble(str);
				} catch {
					returnValue = defaultSetting;
				}

			}
			return returnValue;
		}

		/// <summary>
		/// Retrieves the specified Guid setting, or returns the default 
		/// Guid setting you provide if the specified 'settingName' is not found.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to retrieve.
		/// </param>
		/// 
		/// <param name="defaultSetting">
		/// Default Guid setting to return if 'settingName' does not exist.
		/// </param>
		/// 
		/// <returns>
		/// The data associated with 'settingName', or 'defaultSetting' if 'settingName' is not found.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The parameter 'settingName' is null or empty
		/// </exception>
		/// 
		/// <remarks>
		/// The parameter 'settingName' is  case-sensitive.
		/// If the data stored at the specified 'settingName' cannot be converted to the
		/// particular data type the 'defaultSetting' is returned.
		/// </remarks>
		public Guid GetSetting(string settingName, Guid defaultSetting) {
			Guid returnValue = defaultSetting;
			string str = GetSetting(settingName, string.Empty);
			if (str != string.Empty) {
				try {
					returnValue = XmlConvert.ToGuid(str);
				} catch {
					returnValue = defaultSetting;
				}

			}
			return returnValue;
		}

		/// <summary>
		/// Retrieves the specified Int16 setting, or returns the default 
		/// Int16 setting you provide if the specified 'settingName' is not found.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to retrieve.
		/// </param>
		/// 
		/// <param name="defaultSetting">
		/// Default Int16 setting to return if 'settingName' does not exist.
		/// </param>
		/// 
		/// <returns>
		/// The data associated with 'settingName', or 'defaultSetting' if 'settingName' is not found.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The parameter 'settingName' is null or empty
		/// </exception>
		/// 
		/// <remarks>
		/// The parameter 'settingName' is  case-sensitive.
		/// If the data stored at the specified 'settingName' cannot be converted to the
		/// particular data type the 'defaultSetting' is returned.
		/// </remarks>
		public short GetSetting(string settingName, short defaultSetting) {
			short returnValue = defaultSetting;
			string str = GetSetting(settingName, string.Empty);
			if (str != string.Empty) {
				try {
					returnValue = XmlConvert.ToInt16(str);
				} catch {
					returnValue = defaultSetting;
				}

			}
			return returnValue;
		}

		/// <summary>
		/// Retrieves the specified integer setting, or returns the default 
		/// integer setting you provide if the specified 'settingName' is not found.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to retrieve.
		/// </param>
		/// 
		/// <param name="defaultSetting">
		/// Default integer setting to return if 'settingName' does not exist.
		/// </param>
		/// 
		/// <returns>
		/// The data associated with 'settingName', or 'defaultSetting' if 'settingName' is not found.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The parameter 'settingName' is null or empty
		/// </exception>
		/// 
		/// <remarks>
		/// The parameter 'settingName' is  case-sensitive.
		/// If the data stored at the specified 'settingName' cannot be converted to the
		/// particular data type the 'defaultSetting' is returned.
		/// </remarks>
		public int GetSetting(string settingName, int defaultSetting) {
			int returnValue = defaultSetting;
			string str = GetSetting(settingName, string.Empty);
			if (str != string.Empty) {
				try {
					returnValue = XmlConvert.ToInt32(str);
				} catch {
					returnValue = defaultSetting;
				}

			}
			return returnValue;
		}

		/// <summary>
		/// Retrieves the specified Int64 setting, or returns the default 
		/// Int64 setting you provide if the specified 'settingName' is not found.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to retrieve.
		/// </param>
		/// 
		/// <param name="defaultSetting">
		/// Default Int64 setting to return if 'settingName' does not exist.
		/// </param>
		/// 
		/// <returns>
		/// The data associated with 'settingName', or 'defaultSetting' if 'settingName' is not found.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The parameter 'settingName' is null or empty
		/// </exception>
		/// 
		/// <remarks>
		/// The parameter 'settingName' is  case-sensitive.
		/// If the data stored at the specified 'settingName' cannot be converted to the
		/// particular data type the 'defaultSetting' is returned.
		/// </remarks>
		public long GetSetting(string settingName, long defaultSetting) {
			long returnValue = defaultSetting;
			string str = GetSetting(settingName, string.Empty);
			if (str != string.Empty) {
				try {
					returnValue = XmlConvert.ToInt64(str);
				} catch {
					returnValue = defaultSetting;
				}

			}
			return returnValue;
		}

		/// <summary>
		/// Retrieves the specified single(float)setting, or returns the default 
		/// single(float)setting you provide if the specified 'settingName' is not found.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to retrieve.
		/// </param>
		/// 
		/// <param name="defaultSetting">
		/// Default single(float) setting to return if 'settingName' does not exist.
		/// </param>
		/// 
		/// <returns>
		/// The data associated with 'settingName', or 'defaultSetting' if 'settingName' is not found.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The parameter 'settingName' is null or empty
		/// </exception>
		/// 
		/// <remarks>
		/// The parameter 'settingName' is  case-sensitive.
		/// If the data stored at the specified 'settingName' cannot be converted to the
		/// particular data type the 'defaultSetting' is returned.
		/// </remarks>
		public float GetSetting(string settingName, float defaultSetting) {
			float returnValue = defaultSetting;
			string str = GetSetting(settingName, string.Empty);
			if (str != string.Empty) {
				try {
					returnValue = XmlConvert.ToSingle(str);
				} catch {
					returnValue = defaultSetting;
				}

			}
			return returnValue;
		}

		/// <summary>
		/// Retrieves the specified TimeSpan setting, or returns the default 
		/// TimeSpan setting you provide if the specified 'settingName' is not found.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to retrieve.
		/// </param>
		/// 
		/// <param name="defaultSetting">
		/// Default TimeSpan setting to return if 'settingName' does not exist.
		/// </param>
		/// 
		/// <returns>
		/// The data associated with 'settingName', or 'defaultSetting' if 'settingName' is not found.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The parameter 'settingName' is null or empty
		/// </exception>
		/// 
		/// <remarks>
		/// The parameter 'settingName' is  case-sensitive.
		/// If the data stored at the specified 'settingName' cannot be converted to the
		/// particular data type the 'defaultSetting' is returned.
		/// </remarks>
		public TimeSpan GetSetting(string settingName, TimeSpan defaultSetting) {
			TimeSpan returnValue = defaultSetting;
			string str = GetSetting(settingName, string.Empty);
			if (str != string.Empty) {
				try {
					returnValue = XmlConvert.ToTimeSpan(str);
				} catch {
					returnValue = defaultSetting;
				}

			}
			return returnValue;
		}

		#endregion

		#region LoadSetting

		//currently set to  private. This method shows an example
		//how to load a setting
		//directly into a refrence variable, passed into the method. 

		/// <summary>
		/// Loads the specified setting value directly to a variable refrence.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to load
		/// </param>
		/// 
		/// <param name="variable">
		/// a reference variable in which the specified
		/// setting value is to be assigned to
		/// </param>
		/// 
		/// <returns>
		/// true if a value was assigned to the refrence variable.
		/// false if the specified setting was not found, or could not
		/// be converted to the same data type as the reference variable.
		/// </returns>
		private bool LoadSetting(string settingName, ref bool variable) {
			bool internalVariable;
			string str = GetSetting(settingName, string.Empty);
			if (str != string.Empty) {
				try {
					internalVariable = XmlConvert.ToBoolean(str);
				} catch {
					return false;
				}

				variable = internalVariable;

				return true;
			}
			return false;
		}

		#endregion

		#region OpenSubKey

		//OpenSubKey
		/// <summary>
		/// Retrieves a subkey.
		/// </summary>
		/// 
		/// <param name="subkeyName">
		/// The name or path of the subkey to open.
		/// </param>
		/// 
		/// <returns>
		/// The subkey requested, or null if the operation failed.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The specified string patameter 'subkeyName'is null or 
		/// empty an empty string.
		/// </exception>
		/// 
		/// <remarks>
		/// The parameter 'subkeyName' is  case-sensitive.
		/// You must open a key before it can be manipulated 
		/// with other methods and properties. If the specified 
		/// subkey cannot be found, then a null setting is returned.
		/// </remarks>
		public SettingsKey OpenSubKey(string subkeyName) {

			return this.OpenSubKey(subkeyName, false);
		}


		/// <summary>
		/// Retrieves a specified subkey, with the specified
		/// read-write access.
		/// </summary>
		/// 
		/// <param name="subkeyName">
		/// The name or path of the subkey to open.
		/// </param>
		/// 
		/// <param name="writable">
		/// Set this argument to true if you need write access to the key.
		/// </param>
		/// 
		/// <returns>
		/// The subkey requested, or null if the operation failed.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The specified string parameter 'subkeyName' is null or an empty string.
		/// </exception>
		/// 
		/// <remarks>
		/// The parameter 'subkeyName' is  case-sensitive.
		/// You must open a key before it can be manipulated 
		/// with other methods and properties. If the specified 
		/// subkey cannot be found, then a null setting is returned.
		/// If writable is set to true, the subkey will be 
		/// opened for read-write access. 
		/// If writable is set to false the key will be opened as read-only.
		/// </remarks>
		public SettingsKey OpenSubKey(string subkeyName, bool writable) {
			SettingsKey key;

			#region conditions

			// is the parameter subkeyName null or an empty string
			if (subkeyName == null || subkeyName == String.Empty) {
				this.throwParameterNullException("subkeyName");
			}

			#endregion

			// if subkey exsist assign it to key
			if (this.data[subkeyName] != null) {
				key = new SettingsKey(this.data[subkeyName], !writable);
			} else
				key = null;

			return key;
		}

		#endregion

		#region StoreSetting
		//StoreSetting

		//string
		/// <summary>
		/// Stores the specified  value in the current SettingsKey.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to store.
		/// </param>
		/// 
		/// <param name="settingValue">
		/// The string value to store for this setting.
		/// </param>
		/// 
		/// <exception cref="UnauthorizedAccessException">
		/// This current SettingsKey is the root SettingsKey or is read-only.
		/// </exception>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The specified 'settingName' 
		/// is a null reference or an empty string or the specified
		/// 'settingValue' is null.
		/// </exception>
		/// 
		/// <remarks>
		/// <para>
		/// Since many settings can be stored in each SettingsKey, 
		/// the 'settingName' parameter specifies the particular setting you wish to manipulate. 
		/// </para>
		/// 
		/// <para>
		/// The key that is opened with the setting being set must have been 
		/// opened with write access, and not be a read-only key. 
		/// Once you have been granted write-access to a key, you can change 
		/// the data associated with any of the settings in that key.
		/// </para>
		/// 
		/// <para>
		/// The parameter 'settingName' is  case-sensitive.
		/// </para>
		/// 
		/// <para>
		/// If the specified setting name does not exist in the key, 
		/// it will be created, and the sepecified settingValue is stored.
		/// If the specified setting name does exist, the settings value will be set.
		/// </para>
		/// </remarks>
		public void StoreSetting(string settingName, string settingValue) {
			#region conditions

			// is the current key readOnly
			if (this.isReadOnly) {
				this.throwKeyReadOnlyException();
			}

			// is the parameter settingName null or an empty string
			if (settingName == null || settingName == String.Empty) {
				this.throwParameterNullException("settingName");
			}

			// is the setting parameter null
			if (settingValue == null || settingValue == string.Empty) {
				this.throwParameterNullException("setting");
			}
			#endregion

			// all conditions met, set settingName to setting
			this.data.SetAttribute(settingName, settingValue);
		}


		//bool
		/// <summary>
		/// Stores the specified  bool value in the current SettingsKey.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to store.
		/// </param>
		/// 
		/// <param name="settingValue">
		/// The bool value to store for this setting.
		/// </param>
		/// 
		/// <exception cref="UnauthorizedAccessException">
		/// This current SettingsKey is the root SettingsKey or is read-only.
		/// </exception>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The specified 'settingName' 
		/// is a null reference or an empty string or the specified
		/// 'settingValue' is null.
		/// </exception>
		/// 
		/// <remarks>
		/// <para>
		/// Since many settings can be stored in each SettingsKey, 
		/// the 'settingName' parameter specifies the particular setting you wish to manipulate. 
		/// </para>
		/// 
		/// <para>
		/// The key that is opened with the setting being set must have been 
		/// opened with write access, and not be a read-only key. 
		/// Once you have been granted write-access to a key, you can change 
		/// the data associated with any of the settings in that key.
		/// </para>
		/// 
		/// <para>
		/// The parameter 'settingName' is  case-sensitive.
		/// </para>
		/// 
		/// <para>
		/// If the specified setting name does not exist in the key, 
		/// it will be created, and the sepecified settingValue is stored.
		/// If the specified setting name does exist, the settings value will be set.
		/// </para>
		/// </remarks>
		public void StoreSetting(string settingName, bool settingValue) {
			string str = XmlConvert.ToString(settingValue);
			StoreSetting(settingName, str);
		}


		//char
		/// <summary>
		/// Stores the specified  char value in the current SettingsKey.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to store.
		/// </param>
		/// 
		/// <param name="settingValue">
		/// The char value to store for this setting.
		/// </param>
		/// 
		/// <exception cref="UnauthorizedAccessException">
		/// This current SettingsKey is the root SettingsKey or is read-only.
		/// </exception>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The specified 'settingName' 
		/// is a null reference or an empty string or the specified
		/// 'settingValue' is null.
		/// </exception>
		/// 
		/// <remarks>
		/// <para>
		/// Since many settings can be stored in each SettingsKey, 
		/// the 'settingName' parameter specifies the particular setting you wish to manipulate. 
		/// </para>
		/// 
		/// <para>
		/// The key that is opened with the setting being set must have been 
		/// opened with write access, and not be a read-only key. 
		/// Once you have been granted write-access to a key, you can change 
		/// the data associated with any of the settings in that key.
		/// </para>
		/// 
		/// <para>
		/// The parameter 'settingName' is  case-sensitive.
		/// </para>
		/// 
		/// <para>
		/// If the specified setting name does not exist in the key, 
		/// it will be created, and the sepecified settingValue is stored.
		/// If the specified setting name does exist, the settings value will be set.
		/// </para>
		/// </remarks>
		public void StoreSetting(string settingName, char settingValue) {
			string str = XmlConvert.ToString(settingValue);
			StoreSetting(settingName, str);
		}




		//byte
		/// <summary>
		/// Stores the specified byte value in the current SettingsKey.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to store.
		/// </param>
		/// 
		/// <param name="settingValue">
		/// The byte value to store for this setting.
		/// </param>
		/// 
		/// <exception cref="UnauthorizedAccessException">
		/// This current SettingsKey is the root SettingsKey or is read-only.
		/// </exception>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The specified 'settingName' 
		/// is a null reference or an empty string or the specified
		/// 'settingValue' is null.
		/// </exception>
		/// 
		/// <remarks>
		/// <para>
		/// Since many settings can be stored in each SettingsKey, 
		/// the 'settingName' parameter specifies the particular setting you wish to manipulate. 
		/// </para>
		/// 
		/// <para>
		/// The key that is opened with the setting being set must have been 
		/// opened with write access, and not be a read-only key. 
		/// Once you have been granted write-access to a key, you can change 
		/// the data associated with any of the settings in that key.
		/// </para>
		/// 
		/// <para>
		/// The parameter 'settingName' is  case-sensitive.
		/// </para>
		/// 
		/// <para>
		/// If the specified setting name does not exist in the key, 
		/// it will be created, and the sepecified settingValue is stored.
		/// If the specified setting name does exist, the settings value will be set.
		/// </para>
		/// </remarks>
		public void StoreSetting(string settingName, byte settingValue) {
			string str = XmlConvert.ToString(settingValue);
			StoreSetting(settingName, str);
		}

		//DateTime
		/// <summary>
		/// Stores the specified DateTime value in the current SettingsKey.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to store.
		/// </param>
		/// 
		/// <param name="settingValue">
		/// The DateTime value to store for this setting.
		/// </param>
		/// 
		/// <exception cref="UnauthorizedAccessException">
		/// This current SettingsKey is the root SettingsKey or is read-only.
		/// </exception>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The specified 'settingName' 
		/// is a null reference or an empty string or the specified
		/// 'settingValue' is null.
		/// </exception>
		/// 
		/// <remarks>
		/// <para>
		/// Since many settings can be stored in each SettingsKey, 
		/// the 'settingName' parameter specifies the particular setting you wish to manipulate. 
		/// </para>
		/// 
		/// <para>
		/// The key that is opened with the setting being set must have been 
		/// opened with write access, and not be a read-only key. 
		/// Once you have been granted write-access to a key, you can change 
		/// the data associated with any of the settings in that key.
		/// </para>
		/// 
		/// <para>
		/// The parameter 'settingName' is  case-sensitive.
		/// </para>
		/// 
		/// <para>
		/// If the specified setting name does not exist in the key, 
		/// it will be created, and the sepecified settingValue is stored.
		/// If the specified setting name does exist, the settings value will be set.
		/// </para>
		/// </remarks>
		public void StoreSetting(string settingName, DateTime settingValue) {
			string str = XmlConvert.ToString(settingValue, XmlDateTimeSerializationMode.Utc);
			StoreSetting(settingName, str);
		}

		//decimal
		/// <summary>
		/// Stores the specified decimal value in the current SettingsKey.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to store.
		/// </param>
		/// 
		/// <param name="settingValue">
		/// The decimal value to store for this setting.
		/// </param>
		/// 
		/// <exception cref="UnauthorizedAccessException">
		/// This current SettingsKey is the root SettingsKey or is read-only.
		/// </exception>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The specified 'settingName' 
		/// is a null reference or an empty string or the specified
		/// 'settingValue' is null.
		/// </exception>
		/// 
		/// <remarks>
		/// <para>
		/// Since many settings can be stored in each SettingsKey, 
		/// the 'settingName' parameter specifies the particular setting you wish to manipulate. 
		/// </para>
		/// 
		/// <para>
		/// The key that is opened with the setting being set must have been 
		/// opened with write access, and not be a read-only key. 
		/// Once you have been granted write-access to a key, you can change 
		/// the data associated with any of the settings in that key.
		/// </para>
		/// 
		/// <para>
		/// The parameter 'settingName' is  case-sensitive.
		/// </para>
		/// 
		/// <para>
		/// If the specified setting name does not exist in the key, 
		/// it will be created, and the sepecified settingValue is stored.
		/// If the specified setting name does exist, the settings value will be set.
		/// </para>
		/// </remarks>
		public void StoreSetting(string settingName, decimal settingValue) {
			string str = XmlConvert.ToString(settingValue);
			StoreSetting(settingName, str);
		}

		//double
		/// <summary>
		/// Stores the specified double value in the current SettingsKey.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to store.
		/// </param>
		/// 
		/// <param name="settingValue">
		/// The double value to store for this setting.
		/// </param>
		/// 
		/// <exception cref="UnauthorizedAccessException">
		/// This current SettingsKey is the root SettingsKey or is read-only.
		/// </exception>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The specified 'settingName' 
		/// is a null reference or an empty string or the specified
		/// 'settingValue' is null.
		/// </exception>
		/// 
		/// <remarks>
		/// <para>
		/// Since many settings can be stored in each SettingsKey, 
		/// the 'settingName' parameter specifies the particular setting you wish to manipulate. 
		/// </para>
		/// 
		/// <para>
		/// The key that is opened with the setting being set must have been 
		/// opened with write access, and not be a read-only key. 
		/// Once you have been granted write-access to a key, you can change 
		/// the data associated with any of the settings in that key.
		/// </para>
		/// 
		/// <para>
		/// The parameter 'settingName' is  case-sensitive.
		/// </para>
		/// 
		/// <para>
		/// If the specified setting name does not exist in the key, 
		/// it will be created, and the sepecified settingValue is stored.
		/// If the specified setting name does exist, the settings value will be set.
		/// </para>
		/// </remarks>
		public void StoreSetting(string settingName, double settingValue) {
			string str = XmlConvert.ToString(settingValue);
			StoreSetting(settingName, str);
		}

		//Guid
		/// <summary>
		/// Stores the specified Guid value in the current SettingsKey.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to store.
		/// </param>
		/// 
		/// <param name="settingValue">
		/// The Guid value to store for this setting.
		/// </param>
		/// 
		/// <exception cref="UnauthorizedAccessException">
		/// This current SettingsKey is the root SettingsKey or is read-only.
		/// </exception>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The specified 'settingName' 
		/// is a null reference or an empty string or the specified
		/// 'settingValue' is null.
		/// </exception>
		/// 
		/// <remarks>
		/// <para>
		/// Since many settings can be stored in each SettingsKey, 
		/// the 'settingName' parameter specifies the particular setting you wish to manipulate. 
		/// </para>
		/// 
		/// <para>
		/// The key that is opened with the setting being set must have been 
		/// opened with write access, and not be a read-only key. 
		/// Once you have been granted write-access to a key, you can change 
		/// the data associated with any of the settings in that key.
		/// </para>
		/// 
		/// <para>
		/// The parameter 'settingName' is  case-sensitive.
		/// </para>
		/// 
		/// <para>
		/// If the specified setting name does not exist in the key, 
		/// it will be created, and the sepecified settingValue is stored.
		/// If the specified setting name does exist, the settings value will be set.
		/// </para>
		/// </remarks>
		public void StoreSetting(string settingName, Guid settingValue) {
			string str = XmlConvert.ToString(settingValue);
			StoreSetting(settingName, str);
		}


		//Int16(short)
		/// <summary>
		/// Stores the specified Int16(short) value in the current SettingsKey.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to store.
		/// </param>
		/// 
		/// <param name="settingValue">
		/// The Int16 value to store for this setting.
		/// </param>
		/// 
		/// <exception cref="UnauthorizedAccessException">
		/// This current SettingsKey is the root SettingsKey or is read-only.
		/// </exception>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The specified 'settingName' 
		/// is a null reference or an empty string or the specified
		/// 'settingValue' is null.
		/// </exception>
		/// 
		/// <remarks>
		/// <para>
		/// Since many settings can be stored in each SettingsKey, 
		/// the 'settingName' parameter specifies the particular setting you wish to manipulate. 
		/// </para>
		/// 
		/// <para>
		/// The key that is opened with the setting being set must have been 
		/// opened with write access, and not be a read-only key. 
		/// Once you have been granted write-access to a key, you can change 
		/// the data associated with any of the settings in that key.
		/// </para>
		/// 
		/// <para>
		/// The parameter 'settingName' is  case-sensitive.
		/// </para>
		/// 
		/// <para>
		/// If the specified setting name does not exist in the key, 
		/// it will be created, and the sepecified settingValue is stored.
		/// If the specified setting name does exist, the settings value will be set.
		/// </para>
		/// </remarks>
		public void StoreSetting(string settingName, short settingValue) {
			string str = XmlConvert.ToString(settingValue);
			StoreSetting(settingName, str);
		}

		//integer(Int32)
		/// <summary>
		/// Stores the specified integer(Int32) value in the current SettingsKey.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to store.
		/// </param>
		/// 
		/// <param name="settingValue">
		/// The integer(Int32) value to store for this setting.
		/// </param>
		/// 
		/// <exception cref="UnauthorizedAccessException">
		/// This current SettingsKey is the root SettingsKey or is read-only.
		/// </exception>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The specified 'settingName' 
		/// is a null reference or an empty string or the specified
		/// 'settingValue' is null.
		/// </exception>
		/// 
		/// <remarks>
		/// <para>
		/// Since many settings can be stored in each SettingsKey, 
		/// the 'settingName' parameter specifies the particular setting you wish to manipulate. 
		/// </para>
		/// 
		/// <para>
		/// The key that is opened with the setting being set must have been 
		/// opened with write access, and not be a read-only key. 
		/// Once you have been granted write-access to a key, you can change 
		/// the data associated with any of the settings in that key.
		/// </para>
		/// 
		/// <para>
		/// The parameter 'settingName' is  case-sensitive.
		/// </para>
		/// 
		/// <para>
		/// If the specified setting name does not exist in the key, 
		/// it will be created, and the sepecified settingValue is stored.
		/// If the specified setting name does exist, the settings value will be set.
		/// </para>
		/// </remarks>
		public void StoreSetting(string settingName, int settingValue) {
			string str = XmlConvert.ToString(settingValue);
			StoreSetting(settingName, str);
		}

		//Int64(long)
		/// <summary>
		/// Stores the specified Int64(long) value in the current SettingsKey.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to store.
		/// </param>
		/// 
		/// <param name="settingValue">
		/// The Int64(long) value to store for this setting.
		/// </param>
		/// 
		/// <exception cref="UnauthorizedAccessException">
		/// This current SettingsKey is the root SettingsKey or is read-only.
		/// </exception>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The specified 'settingName' 
		/// is a null reference or an empty string or the specified
		/// 'settingValue' is null.
		/// </exception>
		/// 
		/// <remarks>
		/// <para>
		/// Since many settings can be stored in each SettingsKey, 
		/// the 'settingName' parameter specifies the particular setting you wish to manipulate. 
		/// </para>
		/// 
		/// <para>
		/// The key that is opened with the setting being set must have been 
		/// opened with write access, and not be a read-only key. 
		/// Once you have been granted write-access to a key, you can change 
		/// the data associated with any of the settings in that key.
		/// </para>
		/// 
		/// <para>
		/// The parameter 'settingName' is  case-sensitive.
		/// </para>
		/// 
		/// <para>
		/// If the specified setting name does not exist in the key, 
		/// it will be created, and the sepecified settingValue is stored.
		/// If the specified setting name does exist, the settings value will be set.
		/// </para>
		/// </remarks>
		public void StoreSetting(string settingName, long settingValue) {
			string str = XmlConvert.ToString(settingValue);
			StoreSetting(settingName, str);
		}


		//single(float)
		/// <summary>
		/// Stores the specified single(float) value in the current SettingsKey.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to store.
		/// </param>
		/// 
		/// <param name="settingValue">
		/// The single(float) value to store for this setting.
		/// </param>
		/// 
		/// <exception cref="UnauthorizedAccessException">
		/// This current SettingsKey is the root SettingsKey or is read-only.
		/// </exception>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The specified 'settingName' 
		/// is a null reference or an empty string or the specified
		/// 'settingValue' is null.
		/// </exception>
		/// 
		/// <remarks>
		/// <para>
		/// Since many settings can be stored in each SettingsKey, 
		/// the 'settingName' parameter specifies the particular setting you wish to manipulate. 
		/// </para>
		/// 
		/// <para>
		/// The key that is opened with the setting being set must have been 
		/// opened with write access, and not be a read-only key. 
		/// Once you have been granted write-access to a key, you can change 
		/// the data associated with any of the settings in that key.
		/// </para>
		/// 
		/// <para>
		/// The parameter 'settingName' is  case-sensitive.
		/// </para>
		/// 
		/// <para>
		/// If the specified setting name does not exist in the key, 
		/// it will be created, and the sepecified settingValue is stored.
		/// If the specified setting name does exist, the settings value will be set.
		/// </para>
		/// </remarks>
		public void StoreSetting(string settingName, float settingValue) {
			string str = XmlConvert.ToString(settingValue);
			StoreSetting(settingName, str);
		}

		//TimeSpan
		/// <summary>
		/// Stores the specified TimeSpan value in the current SettingsKey.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to store.
		/// </param>
		/// 
		/// <param name="settingValue">
		/// The TimeSpan value to store for this setting.
		/// </param>
		/// 
		/// <exception cref="UnauthorizedAccessException">
		/// This current SettingsKey is the root SettingsKey or is read-only.
		/// </exception>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The specified 'settingName' 
		/// is a null reference or an empty string or the specified
		/// 'settingValue' is null.
		/// </exception>
		/// 
		/// <remarks>
		/// <para>
		/// Since many settings can be stored in each SettingsKey, 
		/// the 'settingName' parameter specifies the particular setting you wish to manipulate. 
		/// </para>
		/// 
		/// <para>
		/// The key that is opened with the setting being set must have been 
		/// opened with write access, and not be a read-only key. 
		/// Once you have been granted write-access to a key, you can change 
		/// the data associated with any of the settings in that key.
		/// </para>
		/// 
		/// <para>
		/// The parameter 'settingName' is  case-sensitive.
		/// </para>
		/// 
		/// <para>
		/// If the specified setting name does not exist in the key, 
		/// it will be created, and the sepecified settingValue is stored.
		/// If the specified setting name does exist, the settings value will be set.
		/// </para>
		/// </remarks>
		public void StoreSetting(string settingName, TimeSpan settingValue) {
			string str = XmlConvert.ToString(settingValue);
			StoreSetting(settingName, str);
		}

		//DateTime format
		/// <summary>
		/// Stores the specified DateTime value in the current SettingsKey with applied format.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to store.
		/// </param>
		/// 
		/// <param name="settingValue">
		/// The DateTime value to store for this setting.
		/// </param>
		/// 
		/// <param name="format"> the format to apply to the DateTime value</param>
		/// 
		/// <exception cref="UnauthorizedAccessException">
		/// This current SettingsKey is the root SettingsKey or is read-only.
		/// </exception>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The specified 'settingName' 
		/// is a null reference or an empty string or the specified
		/// 'settingValue' is null.
		/// </exception>
		/// 
		/// <remarks>
		/// <para>
		/// Since many settings can be stored in each SettingsKey, 
		/// the 'settingName' parameter specifies the particular setting you wish to manipulate. 
		/// </para>
		/// 
		/// <para>
		/// The key that is opened with the setting being set must have been 
		/// opened with write access, and not be a read-only key. 
		/// Once you have been granted write-access to a key, you can change 
		/// the data associated with any of the settings in that key.
		/// </para>
		/// 
		/// <para>
		/// The parameter 'settingName' is  case-sensitive.
		/// </para>
		/// 
		/// <para>
		/// If the specified setting name does not exist in the key, 
		/// it will be created, and the sepecified settingValue is stored.
		/// If the specified setting name does exist, the settings value will be set.
		/// </para>
		/// </remarks>
		public void StoreSetting(string settingName, DateTime settingValue, string format) {
			string str = XmlConvert.ToString(settingValue, format);
			StoreSetting(settingName, str);
		}

		#region CLS Incompliant
		//public void StoreSetting(string settingName, sbyte settingValue)
		//		{
		//			string str = XmlConvert.ToString(settingValue);
		//			StoreSetting(settingName,str);
		//		}
		//		public void StoreSetting(string settingName, ushort settingValue)
		//		{
		//			string str = XmlConvert.ToString(settingValue);
		//			StoreSetting(settingName,str);
		//		}

		//		public void StoreSetting(string settingName, uint settingValue)
		//		{
		//			string str = XmlConvert.ToString(settingValue);
		//			StoreSetting(settingName,str);
		//		}
		//
		//		public void StoreSetting(string settingName, ulong settingValue)
		//		{
		//			string str = XmlConvert.ToString(settingValue);
		//			StoreSetting(settingName,str);
		//		}
		#endregion



		#endregion

		#region System Drawing Types
		//Inclusion of the following methods within this 
		//code region will require
		//a reference to System.Drawing

		#region Store Types

		//Point
		/// <summary>
		/// Stores the specified Point Object as a setting in the current SettingsKey.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to store.
		/// </param>
		/// 
		/// <param name="settingValue">
		/// The Point to store for this setting.
		/// </param>
		/// 
		/// <exception cref="UnauthorizedAccessException">
		/// This current SettingsKey is the root SettingsKey or is read-only.
		/// </exception>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The specified 'settingName' 
		/// is a null reference or an empty string or the specified
		/// 'settingValue' is null.
		/// </exception>
		/// 
		/// <remarks>
		/// To retrieve a stored Point from the settings file use the <see cref="GetPoint"/> method.
		/// <para>
		/// Since many settings can be stored in each SettingsKey, 
		/// the 'settingName' parameter specifies the particular setting you wish to manipulate. 
		/// </para>
		/// 
		/// <para>
		/// The key that is opened with the setting being set must have been 
		/// opened with write access, and not be a read-only key. 
		/// Once you have been granted write-access to a key, you can change 
		/// the data associated with any of the settings in that key.
		/// </para>
		/// 
		/// <para>
		/// The parameter 'settingName' is  case-sensitive.
		/// </para>
		/// 
		/// <para>
		/// If the specified setting name does not exist in the key, 
		/// it will be created, and the sepecified settingValue is stored.
		/// </para>
		/// </remarks>
		public void StorePoint(string settingName, Point settingValue) {
			// this was removed because a point at 0,0 is perfectly valid.
			//			#region conditions
			//
			//			// is the settingValue parameter null
			//			if (settingValue == Point.Empty )
			//			{
			//				this.throwParameterNullException("setting");
			//			}
			//		
			//			#endregion

			StoreSetting(settingName, settingValue.ToString());
		}

		//Size
		/// <summary>
		/// Stores the specified Size Objectas a setting in the current SettingsKey.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to store.
		/// </param>
		/// 
		/// <param name="settingValue">
		/// The Size to store for this setting.
		/// </param>
		/// 
		/// <exception cref="UnauthorizedAccessException">
		/// This current SettingsKey is the root SettingsKey or is read-only.
		/// </exception>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The specified 'settingName' 
		/// is a null reference or an empty string or the specified
		/// 'settingValue' is null.
		/// </exception>
		/// 
		/// <remarks>
		/// To retrieve a stored Size from the settings file use the <see cref="GetSize"/> method.
		/// <para>
		/// Since many settings can be stored in each SettingsKey, 
		/// the 'settingName' parameter specifies the particular setting you wish to manipulate. 
		/// </para>
		/// 
		/// <para>
		/// The key that is opened with the setting being set must have been 
		/// opened with write access, and not be a read-only key. 
		/// Once you have been granted write-access to a key, you can change 
		/// the data associated with any of the settings in that key.
		/// </para>
		/// 
		/// <para>
		/// The parameter 'settingName' is  case-sensitive.
		/// </para>
		/// 
		/// <para>
		/// If the specified setting name does not exist in the key, 
		/// it will be created, and the sepecified settingValue is stored.
		/// </para>
		/// </remarks>
		public void StoreSize(string settingName, Size settingValue) {
			#region conditions

			//Removed because a size of 0,0 should be valid.
			//			// is the settingValue parameter null
			//			if (settingValue == Size.Empty)
			//			{
			//				this.throwParameterNullException("setting");
			//			}

			#endregion

			StoreSetting(settingName, settingValue.ToString());
		}


		//Color
		/// <summary>
		/// Stores the specified Color Object as a setting in the current SettingsKey.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to store.
		/// </param>
		/// 
		/// <param name="settingValue">
		/// The Color to store for this setting.
		/// </param>
		/// 
		/// <exception cref="UnauthorizedAccessException">
		/// This current SettingsKey is the root SettingsKey or is read-only.
		/// </exception>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The specified 'settingName' 
		/// is a null reference or an empty string or the specified
		/// 'settingValue' is null.
		/// </exception>
		/// 
		/// <remarks>
		/// To retrieve a stored Color from the settings file use the <see cref="GetColor"/> method.
		/// <para>
		/// Since many settings can be stored in each SettingsKey, 
		/// the 'settingName' parameter specifies the particular setting you wish to manipulate. 
		/// </para>
		/// 
		/// <para>
		/// The key that is opened with the setting being set must have been 
		/// opened with write access, and not be a read-only key. 
		/// Once you have been granted write-access to a key, you can change 
		/// the data associated with any of the settings in that key.
		/// </para>
		/// 
		/// <para>
		/// The parameter 'settingName' is  case-sensitive.
		/// </para>
		/// 
		/// <para>
		/// If the specified setting name does not exist in the key, 
		/// it will be created, and the sepecified settingValue is stored.
		/// </para>
		/// </remarks>
		public void StoreColor(string settingName, Color settingValue) {
			//encode name to a valid XML Name
			settingName = XmlConvert.EncodeName(settingName);

			#region conditions

			// is the settingValue parameter null
			if (settingValue.IsEmpty) {
				this.throwParameterNullException("setting");
			}

			#endregion

			if (settingValue.IsKnownColor) {
				StringBuilder str = new StringBuilder(settingValue.ToKnownColor().ToString());
				str.Insert(0, RES_KnownColorStartString);
				str.Append(RES_ColorEndChar);
				StoreSetting(settingName, str.ToString());
			} else {
				StringBuilder str = new StringBuilder(settingValue.ToArgb().ToString());

				str.Insert(0, RES_UnKnownColorStartString);
				str.Append(RES_ColorEndChar);

				StoreSetting(settingName, str.ToString());
			}
		}
		#endregion

		#region Get Types



		//Point
		/// <summary>
		/// Retrieves a Point Object from the specified setting, or returns the default 
		/// Point you provide if the specified 'settingName' is not found.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to retrieve.
		/// </param>
		/// 
		/// <param name="defaultSetting">
		/// Default Point to return if 'settingName' does not exist.
		/// </param>
		/// 
		/// <returns>
		/// The data associated with 'settingName', or 'defaultSetting' if 'settingName' is not found.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The parameter 'settingName' is null or empty
		/// </exception>
		/// 
		/// <remarks>
		/// To store a Point in the settings file use the <see cref="StorePoint"/> method.
		/// The parameter 'settingName' is  case-sensitive.
		/// If the data stored at the specified 'settingName' cannot be converted to the
		/// particular data type the 'defaultSetting' is returned.
		/// </remarks>
		public Point GetPoint(string settingName, Point defaultSetting) {

			Point internalVariable;
			string str = GetSetting(settingName, string.Empty);
			if (str != string.Empty) {
				try {
					internalVariable = this.parsePoint(str);
				} catch {
					return defaultSetting;
				}

				return internalVariable;
			}
			return defaultSetting;

		}

		//Size
		/// <summary>
		/// Retrieves a Size Object from the specified setting, or returns the default 
		/// Size  you provide if the specified 'settingName' is not found.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to retrieve.
		/// </param>
		/// 
		/// <param name="defaultSetting">
		/// Default Size to return if 'settingName' does not exist.
		/// </param>
		/// 
		/// <returns>
		/// The data associated with 'settingName', or 'defaultSetting' if 'settingName' is not found.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The parameter 'settingName' is null or empty
		/// </exception>
		/// 
		/// <remarks>
		/// To store a Size in the settings file use the <see cref="StoreSize"/> method.
		/// The parameter 'settingName' is  case-sensitive.
		/// If the data stored at the specified 'settingName' cannot be converted to the
		/// particular data type the 'defaultSetting' is returned.
		/// </remarks>
		public Size GetSize(string settingName, Size defaultSetting) {

			Size internalVariable;
			string str = GetSetting(settingName, string.Empty);
			if (str != string.Empty) {
				try {
					internalVariable = this.parseSize(str);
				} catch {
					return defaultSetting;
				}


				return internalVariable;
			}
			return defaultSetting;

		}

		//Color
		/// <summary>
		/// Retrieves a Color Object from the specified setting, or returns the default 
		/// Color  you provide if the specified 'settingName' is not found.
		/// </summary>
		/// 
		/// <param name="settingName">
		/// The name of the setting to retrieve.
		/// </param>
		/// 
		/// <param name="defaultSetting">
		/// Default Color to return if 'settingName' does not exist.
		/// </param>
		/// 
		/// <returns>
		/// The data associated with 'settingName', or 'defaultSetting' if 'settingName' is not found.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// The parameter 'settingName' is null or empty
		/// </exception>
		/// 
		/// <remarks>
		/// To store a Color in the settings file use the <see cref="StoreColor"/> method.
		/// The parameter 'settingName' is  case-sensitive.
		/// If the data stored at the specified 'settingName' cannot be converted to the
		/// particular data type the 'defaultSetting' is returned.
		/// </remarks>
		public Color GetColor(string settingName, Color defaultSetting) {

			Color internalVariable;
			string str = GetSetting(settingName, string.Empty);
			if (str != string.Empty) {
				try {
					internalVariable = this.parseColor(str);
				} catch {
					return defaultSetting;
				}

				return internalVariable;
			}
			return defaultSetting;
		}

		#endregion

		#endregion

		#region ToString
		//ToString
		/// <summary>
		/// Retrieves a string representation of this SettingsKey.
		/// </summary>
		/// 
		/// <returns>
		/// A string representing the key.
		/// </returns>
		/// 
		/// <remarks>
		/// The return value includes the full path of the SettingsKey.
		/// </remarks>
		public override string ToString() {
			return this.FullName;
		}
		#endregion

		#region ToXml
		//ToXml
		/// <summary>
		/// Retrieves the Xml data of the SettingsKey.
		/// </summary>
		/// 
		/// <returns>
		/// An Xml data string of the key.
		/// </returns>
		/// 
		/// <remarks>
		/// 
		/// </remarks>
		public string ToXml() {
			return this.data.OuterXml;
		}
		#endregion

		#endregion

		#region Private Instance Methods

		private void throwParameterNullException(string parameterName) {
			throw new ArgumentNullException(parameterName, RES_NullParameter);
		}


		private void throwKeyReadOnlyException() {
			throw new UnauthorizedAccessException(RES_ReadOnlyKey);
		}

		private void throwInvalidValueRefException(string parameterName) {
			throw new ArgumentException(RES_InvalidValueRef, parameterName);
		}

		private void throwInvalidKeyRefException(string parameterName) {
			throw new ArgumentException(RES_InvalidKeyRef, parameterName);
		}

		private void throwInvalidDeleteWithChildKeys() {
			throw new InvalidOperationException(RES_InvalidDeleteWithChildKeys);
		}

		private void internalDeleteSubKey(string subkeyName, bool throwOnMissingSubkey, bool delTree) {
			#region conditions

			// is this key readonly
			if (isReadOnly) {
				throwKeyReadOnlyException();
			}

			//is the parameter 'subkeyName null or an empty string
			if (subkeyName == null || subkeyName == String.Empty) {
				throwParameterNullException("subkeyName");
			}

			// is subkey missing in the Xml and is throwOnMissing set to true
			if (data[subkeyName] == null) {
				if (throwOnMissingSubkey) {
					throwInvalidKeyRefException("subkeyName");
				} else return;
			}

			if (!delTree) {
				// does subkey exsist and does it have child subkeys
				if (data[subkeyName] != null & data[subkeyName].HasChildNodes) {
					throwInvalidDeleteWithChildKeys();
				}
			}

			#endregion

			// if all conditions met and subkey exsist delete the subkey
			if (data[subkeyName] != null) {
				data.RemoveChild(data[subkeyName]);
			}
		}

		// parse point from string data.
		private Point parsePoint(string pointString) {
			Point p = new Point();
			string xStr;
			string yStr;
			// char array used to strip beginning of string
			char[] start = RES_PointStartString.ToCharArray();
			//char array used to strip end of string
			char[] end = RES_PointEndString.ToCharArray();
			//split by ','
			string[] strs = pointString.Split(RES_PointDelimChar);

			xStr = strs[0].TrimStart(start);

			// parse  xval to int, into point x
			p.X = int.Parse(xStr);


			yStr = strs[1].TrimStart(end);
			//parse yval to int, into point y
			p.Y = int.Parse(yStr.Trim(RES_PointEndChar));

			return p;
		}

		private Size parseSize(string sizeString) {
			Size s = new Size();

			string hStr;
			char[] width = RES_SizeStartString.ToCharArray();
			char[] height = RES_SizeEndString.ToCharArray();

			//spit string by ','
			string[] strs = sizeString.Split(RES_SizeDelimChar);

			//strip, parse to int, to size.Width
			s.Width = int.Parse(strs[0].Trim(width));

			hStr = strs[1].Trim(height);

			//strip, parse to int, to size.Height
			s.Height = int.Parse(hStr.Trim(RES_SizeEndChar));
			return s;
		}



		private Color parseColor(string colorString) {
			//is color a 'KnownColor', or an Argb color
			//or else colorString is neither, and thus invalid.

			//call corisponding parse method

			Color c;
			//Known
			if (colorString.StartsWith("COLOR[")) {
				try {
					c = parseKnownColor(colorString);
				} catch {
					// any exceptions thrown back to GetColor
					throw;
				}

			}
				//Argb
			else if (colorString.StartsWith("RGB[")) {
				try {
					c = parseUnknownColor(colorString);
				} catch {
					// any exceptions thrown back to GetColor
					throw;
				}
			}
				// colorString is neither, thus invalid throw argument.
			else {
				throw new ArgumentException(RES_ColorParseError, "colorString");
			}


			return c;
		}

		private Color parseKnownColor(string colorString) {
			//create char[] to strip tags stored
			char[] strip = RES_KnownColorStartString.ToCharArray();

			//strip 
			string fString = colorString.TrimStart(strip);
			//trim ending 
			fString = fString.Trim(RES_ColorEndChar);

			KnownColor k;

			//if this throws an exception parseColor catches it..
			k = (KnownColor)Enum.Parse(typeof(KnownColor), fString, true);

			return Color.FromKnownColor(k);
		}

		private Color parseUnknownColor(string colorString) {
			//create char[] to strip tags stored
			char[] strip = RES_UnKnownColorStartString.ToCharArray();

			//strip
			string fString = colorString.TrimStart(strip);
			//trim end
			fString = fString.Trim(RES_ColorEndChar);

			//if this throws an exception parseColor catches it..
			return Color.FromArgb(int.Parse(fString));
		}

		#endregion

		#region IDisposable Implementation

		void IDisposable.Dispose() {
			//reserved for the future
			//who would want to dispose of such a nice object ;)

		}
		#endregion

		#region Instance Destructor
		/// <summary>
		/// In C#, finalizers are expressed using destructor syntax.
		/// </summary>
		~SettingsKey() {

		}
		#endregion
	}
	#endregion

	#region SettingsFormatException

	/// <summary>
	/// The exception that is thrown when a SettingsFile format error occurs.
	/// </summary>
	[System.Serializable]
	public sealed class SettingsFormatException : System.Exception {

		#region instance fields
		private int lineNumber;
		private int linePosition;
		#endregion

		#region instance constructors

		/// <summary>
		/// Initializes a new instance of the RegistryFormatException 
		/// </summary>
		public SettingsFormatException()
			: base() {
		}

		/// <summary>
		/// Initializes a new instance of the RegistryFormatException 
		/// with a specified error message.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public SettingsFormatException(string message)
			: base(message) {

		}

		/// <summary>
		/// Initializes a new instance of the RegistryFormatException 
		/// class with a specified error message and a reference 
		/// to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the 
		/// reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause 
		/// of the current exception. If the innerException parameter is 
		/// not a null reference (Nothing in Visual Basic), the current 
		/// exception is raised in a catch block that handles the inner 
		/// exception.</param>
		public SettingsFormatException(string message, Exception innerException)
			: base(message, innerException) {
		}


		/// <summary>
		/// Initializes a new instance of the RegistryFormatException 
		/// class with a specified error message and the line number and position
		/// where the error occured.
		/// </summary>
		/// <param name="message">The error message that explains the 
		/// reason for the exception.</param>
		/// <param name="lineNumber">The line number where the error occured</param>
		/// <param name="linePosition">The line position where the error occured</param>
		public SettingsFormatException(string message, int lineNumber, int linePosition)
			: this(message) {
			this.lineNumber = lineNumber;
			this.linePosition = linePosition;
		}
		/// <summary>
		/// Initializes a new instance of the RegistryFormatException 
		/// class with a specified error message,line number and position
		/// where the error occured and a reference 
		/// to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the 
		/// reason for the exception.</param>
		/// <param name="lineNumber">The line number where the error occured</param>
		/// <param name="linePosition">The line position where the error occured</param>
		/// <param name="innerException">The exception that is the cause 
		/// of the current exception. If the innerException parameter is 
		/// not a null reference (Nothing in Visual Basic), the current 
		/// exception is raised in a catch block that handles the inner 
		/// exception.</param>
		public SettingsFormatException(string message, int lineNumber, int linePosition, Exception innerException)
			: this(message, innerException) {
			this.lineNumber = lineNumber;
			this.linePosition = linePosition;
		}

		//Private constructor for seialization
		private SettingsFormatException(SerializationInfo info, StreamingContext context)
			: base(info, context) {
			lineNumber = info.GetInt32("lineNumber");
			linePosition = info.GetInt16("linePosition");
		}

		#endregion

		#region instance methods

		/// <summary>
		/// retrieves the Object Data for serialization
		/// </summary>
		public override void GetObjectData(SerializationInfo info, StreamingContext context) {
			info.AddValue("lineNumber", lineNumber);
			info.AddValue("linePosition", linePosition);

			base.GetObjectData(info, context);
		}
		#endregion

		#region instance properties

		/// <summary>
		/// Gets the line number where the format error ocurred.
		/// </summary><setting>the line number</setting>
		public int LineNumber {
			get { return this.lineNumber; }
		}

		/// <summary>
		/// Gets the line position where the format error ocurred.
		/// </summary> <setting>the line position</setting>
		public int LinePosition {
			get { return this.linePosition; }
		}

		/// <summary>
		/// Gets a message that describes the current exception.
		/// </summary>
		public override string Message {
			get {
				string msg = base.Message;
				if (!(lineNumber == 0) & !(linePosition == 0)) {
					msg += msg + " ,Error at Line Number(" + lineNumber.ToString() + ") , Position(" + linePosition.ToString() + ")";
				}
				return msg;
			}
		}

		#endregion

	}
	#endregion

}
