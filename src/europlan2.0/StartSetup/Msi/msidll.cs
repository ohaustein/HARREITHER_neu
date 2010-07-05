// <file>
//     <copyright see="2003,2004,2005 Youseful Software"/>
//     <license see="MPL"/>
//     <owner name="Youseful Software" email="support@youseful.com"/>
//     <version value="0.95"/>
// </file>
// Original Author: Joseph J Mele (joemele@gmail.com)
// If you like using this wrapper, please,(as it is NOT required)
// make a donation of ANY AMOUNT to a civil rights group for autistics - Aspies for Freedom  (AFF)
// The donation link is http://www.aspiesforfreedom.org/index.php?page=donate
// You have my personal thanks, if you choose to do this. It is the best way
// to show your appreciation.


using System;
using System.Text;
using System.Runtime.InteropServices;
using Youseful.Types.Interop;


#region Update Log
/*************************************************************************
 *
 *  version .92
 * ==============================
 * -------- Added patch functions from msi 3.0
 * -------- Added more functions from previous versions of msi
 * -------- The MsiSource* functions will be added next update
 * -------- WinAPI attribute added.
 * 
 * version .91
 * ==============================
 * --------	 Add patch functions from Ian Mariano's wrapper.
 * --------- Added CharityWare notice.
 * 
 * version .9 
 *======================
 *-------  Ian Mariano (www.modelmatter.com) graciously allowed his changes 
 * to be added. His wrapper is hosted at http://workspaces.gotdotnet.com/MsiInterop
 * External UI handlers added
 * ------ Some XML Comments added
 * ------ Nativemsi Attribute applied
 * ------ WinAPI Attribute applied 
 * 
 *	version .8
 *======================
 *-------  Petr (pbubela@nbox.cz) fixed bug to allow update queries. 
 * The value persist in MsiOpenDatabase changed to int from string.
 *
 * version .7
 *======================
 *------ Divided the MSI API into three base classes
 *------ Added Summary Info API
 * ***********************************************************************/
#endregion


namespace Youseful.Installer.WI
{
	#region Nativemsi Attribute
	 
	[AttributeUsage(AttributeTargets.All,AllowMultiple = false, Inherited = false)]
	public class NativemsiAttribute : System.Attribute
	{
		private WindowsInstallerVersion _Version;
		private msiContext _Context;

		public NativemsiAttribute(WindowsInstallerVersion version)
		{
			this._Version = version;
			this._Context = msiContext.All;
		}
		
		public WindowsInstallerVersion Version
		{
			get
			{ return _Version;}
			//set	{ _Version = value;}
		}
		
		public msiContext Context
		{
			get { return _Context;}
			set { _Context = value;}
		}



	}
	#endregion
	

	
	/// <summary>
	/// contains the file hash information returned by MsiGetFileHash and used in the MsiFileHash table.
	/// </summary>
	[Nativemsi(WindowsInstallerVersion.version15,Context = msiContext.All)]
	[StructLayout(LayoutKind.Explicit)]
	internal struct MSIFILEHASHINFO
	{
		[FieldOffset(0)] internal uint fileHashInfoSize;
		[FieldOffset(4)] internal int data0;
		[FieldOffset(8)] internal int data1;
		[FieldOffset(12)]internal int data2;
		[FieldOffset(16)]internal int data3;
	}

	public class Msidll
	{
		#region DataBase Manipulation
		
		/// <summary>Enumeration of custom action types.</summary>
		public enum	MsiCustomActionType : int
		{
			// executable types

			/// <summary>Target = entry point name</summary>
			Dll = 0x00000001,
			/// <summary>Target = command line args</summary>
			Exe = 0x00000002,
			/// <summary>Target = text string to be formatted and set into property</summary>
			TextData = 0x00000003,
			/// <summary>Target = entry point name, null if none to call</summary>
			JScript = 0x00000005,
			/// <summary>Target = entry point name, null if none to call</summary>
			VBScript = 0x00000006,
			/// <summary>Target = property list for nested engine initialization</summary>
			Install = 0x00000007,

			// source of code

			/// <summary>Source = Binary.Name, data stored in stream</summary>
			BinaryData = 0x00000000,
			/// <summary>Source = File.File, file part of installation</summary>
			SourceFile = 0x00000010,
			/// <summary>Source = Directory.Directory, folder containing existing file</summary>
			Directory = 0x00000020,
			/// <summary>Source = Property.Property, full path to executable</summary>
			Property = 0x00000030,

			// return processing default is syncronous execution, process return code

			/// <summary>ignore action return status, continue running</summary>
			Continue = 0x00000040,
			/// <summary>run asynchronously</summary>
			Async = 0x00000080,

			// execution scheduling flags  default is execute whenever sequenced

			/// <summary>skip if UI sequence already run</summary>
			FirstSequence = 0x00000100,
			/// <summary>skip if UI sequence already run in same process</summary>
			OncePerProcess = 0x00000200,
			/// <summary>run on client only if UI already run on client</summary>
			ClientRepeat = 0x00000300,
			/// <summary>queue for execution within script</summary>
			InScript = 0x00000400,
			/// <summary>in conjunction with InScript: queue in Rollback script</summary>
			Rollback = 0x00000100,
			/// <summary>in conjunction with InScript: run Commit ops from script on success</summary>
			Commit = 0x00000200,

			// security context flag, default to impersonate as user, valid only if InScript

			/// <summary>no impersonation, run in system context</summary>
			NoImpersonate = 0x00000800,
			/// <summary>impersonate for per-machine installs on TS machines</summary>
			TSAware = 0x00004000,

			// script requires 64bit process

			/// <summary>script should run in 64bit process</summary>
			Type64BitScript = 0x00001000,
			/// <summary>don't record the contents of the Target field in the log file.</summary>
			HideTarget = 0x00002000,
		}

		/// <summary>
		/// Enumeration of different modify modes.
		/// </summary>
		public enum ModifyView
		{
			/// <summary>
			/// Writes current data in the cursor to a table row. Updates record if the primary 
			/// keys match an existing row and inserts if they do not match. Fails with a read-only 
			/// database. This mode cannot be used with a view containing joins.
			/// </summary>
			Assign = 3,
			/// <summary>
			/// Remove a row from the table. You must first call the Fetch function with the same
			/// record. Fails if the row has been deleted. Works only with read-write records. This
			/// mode cannot be used with a view containing joins.
			/// </summary>
			Delete = 6,
			/// <summary>
			/// Inserts a record. Fails if a row with the same primary keys exists. Fails with a read-only
			/// database. This mode cannot be used with a view containing joins.
			/// </summary>
			Insert = 1,
			/// <summary>
			/// Inserts a temporary record. The information is not persistent. Fails if a row with the 
			/// same primary key exists. Works only with read-write records. This mode cannot be 
			/// used with a view containing joins.
			/// </summary>
			InsertTemporary = 7,
			/// <summary>
			/// Inserts or validates a record in a table. Inserts if primary keys do not match any row
			/// and validates if there is a match. Fails if the record does not match the data in
			/// the table. Fails if there is a record with a duplicate key that is not identical.
			/// Works only with read-write records. This mode cannot be used with a view containing joins.
			/// </summary>
			Merge = 5,
			/// <summary>
			/// Refreshes the information in the record. Must first call Fetch with the
			/// same record. Fails for a deleted row. Works with read-write and read-only records.
			/// </summary>
			Refresh = 0,
			/// <summary>
			/// Updates or deletes and inserts a record into a table. Must first call Fetch with
			/// the same record. Updates record if the primary keys are unchanged. Deletes old row and
			/// inserts new if primary keys have changed. Fails with a read-only database. This mode cannot
			/// be used with a view containing joins.
			/// </summary>
			Replace = 4,
			/// <summary>
			/// Refreshes the information in the supplied record without changing the position in the
			/// result set and without affecting subsequent fetch operations. The record may then
			/// be used for subsequent Update, Delete, and Refresh. All primary key columns of the
			/// table must be in the query and the record must have at least as many fields as the
			/// query. Seek cannot be used with multi-table queries. This mode cannot be used with
			/// a view containing joins. See also the remarks.
			/// </summary>
			Seek = -1,
			/// <summary>
			/// Validates a record. Does not validate across joins.
			///  You must first call the MsiViewFetch function with the same record. 
			///  Obtain validation errors with MsiViewGetError. 
			///  Works with read-write and read-only records. 
			///  This mode cannot be used with a view containing joins.
			/// </summary>
			VALIDATE = 8,
			/// <summary>
			///  Validates a record that will be deleted later. 
			///  You must first call MsiViewFetch. 
			///  Fails if another row refers to the primary keys of this row.
			///   Validation does not check for the existence of the 
			///   primary keys of this row in properties or strings.
			///    Does not check if a column is a foreign key to multiple tables. 
			///    Obtain validation errors by calling MsiViewGetError.
			///     Works with read-write and read-only records. 
			///  This mode cannot be used with a view containing joins.
			/// </summary>
			VALIDATEDELETE = 11,
			/// <summary>
			/// Validates fields of a fetched or new record. 
			/// Can validate one or more fields of an incomplete record. 
			/// Obtain validation errors by calling MsiViewGetError. 
			/// Works with read-write and read-only records.
			///  This mode cannot be used with a view containing joins.
			/// </summary>
			VALIDATEFIELD = 10,
			/// <summary>
			/// Updates an existing record. Non-primary keys only. Must first call Fetch. Fails with a
			/// deleted record. Works only with read-write records.
			/// </summary>
			Update = 2,
			/// <summary>
			/// Validate a new record. Does not validate across joins. 
			/// Checks for duplicate keys. 
			/// Obtain validation errors by calling MsiViewGetError. 
			/// Works with read-write and read-only records. 
			/// This mode cannot be used with a view containing joins.
			/// </summary>
			VALIDATENEW = 9
		}



		public enum FieldType
		{
			Char,
			LongChar,
			Short,
			Integer,
			Long,
			Binary
		};

		/// <summary>
		/// <para>Install message type for callback is a combination of the following:</para>
		/// <para>A message box style:  MB_*, where MB_OK is the default</para>
		/// <para>A message box icon type:  MB_ICON*, where no icon is the default</para>
		/// <para>A default button:  MB_DEFBUTTON?, where MB_DEFBUTTON1 is the default</para>
		/// <para>One of these flags an install message, no default.</para>
		/// </summary>
		public enum INSTALLMESSAGE : long
		{
			INSTALLMESSAGE_FATALEXIT      = 0x00000000L, // premature termination, possibly fatal OOM
			INSTALLMESSAGE_ERROR          = 0x01000000L, // formatted error message
			INSTALLMESSAGE_WARNING        = 0x02000000L, // formatted warning message
			INSTALLMESSAGE_USER           = 0x03000000L, // user request message
			INSTALLMESSAGE_INFO           = 0x04000000L, // informative message for log
			INSTALLMESSAGE_FILESINUSE     = 0x05000000L, // list of files in use that need to be replaced
			INSTALLMESSAGE_RESOLVESOURCE  = 0x06000000L, // request to determine a valid source location
			INSTALLMESSAGE_OUTOFDISKSPACE = 0x07000000L, // insufficient disk space message
			INSTALLMESSAGE_ACTIONSTART    = 0x08000000L, // start of action: action name & description
			INSTALLMESSAGE_ACTIONDATA     = 0x09000000L, // formatted data associated with individual action item
			INSTALLMESSAGE_PROGRESS       = 0x0A000000L, // progress gauge info: units so far, total
			INSTALLMESSAGE_COMMONDATA     = 0x0B000000L, // product info for dialog: language Id, dialog caption
			INSTALLMESSAGE_INITIALIZE     = 0x0C000000L, // sent prior to UI initialization, no string data
			INSTALLMESSAGE_TERMINATE      = 0x0D000000L, // sent after UI termination, no string data
			INSTALLMESSAGE_SHOWDIALOG     = 0x0E000000L, // sent prior to display or authored dialog or wizard
		};
		
		
		/// <summary>Enumeration of MSI install states.</summary>
		public enum INSTALLSTATE
		{
			/// <summary>component disabled</summary>
			NOTUSED      = -7,  
			/// <summary>configuration data corrupt</summary>
			BADCONFIG    = -6,  
			/// <summary>installation suspended or in progress</summary>
			INCOMPLETE   = -5, 
			/// <summary>run from source, source is unavailable</summary>
			SOURCEABSENT = -4,  
			/// <summary>return buffer overflow</summary>
			MOREDATA     = -3,  
			/// <summary>invalid function argument</summary>
			INVALIDARG   = -2,  
			/// <summary>unrecognized product or feature</summary>
			UNKNOWN      = -1, 
			
			/// <summary>broken</summary>
			BROKEN       =  0,  
			/// <summary>advertised feature</summary>
			ADVERTISED   =  1,  
			/// <summary>component being removed (action state, not settable)</summary>
			REMOVED      =  1,  
			/// <summary>uninstalled (or action state absent but clients remain)</summary>
			ABSENT       =  2, 
			/// <summary>installed on local drive</summary>
			LOCAL        =  3,  
			/// <summary>run from source, CD or net</summary>
			SOURCE       =  4,   
			/// <summary>use default, local or source</summary>
			DEFAULT      =  5,  
		} ;

		public enum USERINFOSTATE
		{
			USERINFOSTATE_MOREDATA   = -3,  // return buffer overflow
			USERINFOSTATE_INVALIDARG = -2,  // invalid function argument
			USERINFOSTATE_UNKNOWN    = -1,  // unrecognized product
			USERINFOSTATE_ABSENT     =  0,  // user info and PID not initialized
			USERINFOSTATE_PRESENT    =  1,  // user info and PID initialized
		};

		public enum  INSTALLLEVEL
		{
			INSTALLLEVEL_DEFAULT = 0,      // install authored default
			INSTALLLEVEL_MINIMUM = 1,      // install only required features
			INSTALLLEVEL_MAXIMUM = 0xFFFF, // install all features
		};                   // intermediate levels dependent on authoring

		[Flags]
			public enum REINSTALLMODE  // bit flags
		{
			REINSTALLMODE_REPAIR           = 0x00000001,  // Reserved bit - currently ignored
			REINSTALLMODE_FILEMISSING      = 0x00000002,  // Reinstall only if file is missing
			REINSTALLMODE_FILEOLDERVERSION = 0x00000004,  // Reinstall if file is missing, or older version
			REINSTALLMODE_FILEEQUALVERSION = 0x00000008,  // Reinstall if file is missing, or equal or older version
			REINSTALLMODE_FILEEXACT        = 0x00000010,  // Reinstall if file is missing, or not exact version
			REINSTALLMODE_FILEVERIFY       = 0x00000020,  // checksum executables, reinstall if missing or corrupt
			REINSTALLMODE_FILEREPLACE      = 0x00000040,  // Reinstall all files, regardless of version
			REINSTALLMODE_MACHINEDATA      = 0x00000080,  // insure required machine reg entries
			REINSTALLMODE_USERDATA         = 0x00000100,  // insure required user reg entries
			REINSTALLMODE_SHORTCUT         = 0x00000200,  // validate shortcuts items
			REINSTALLMODE_PACKAGE          = 0x00000400,  // use re-cache source install package
		};

		[Flags]
		public enum MsiInstallLogMode  // bit flags for use with MsiEnableLog and MsiSetExternalUI
		{
			FATALEXIT      = (1 << (0x00  >> 24)),
			ERROR          = (1 << (0x01  >> 24)),
			WARNING        = (1 << (0x02  >> 24)),
			USER           = (1 << (0x03  >> 24)),
			INFO           = (1 << (0x04  >> 24)),
			RESOLVESOURCE  = (1 << (0x06  >> 24)),
			OUTOFDISKSPACE = (1 << (0x07  >> 24)),
			ACTIONSTART    = (1 << (0x08  >> 24)),
			ACTIONDATA     = (1 << (0x09  >> 24)),
			COMMONDATA     = (1 << (0x0B  >> 24)),
			PROPERTYDUMP   = (1 << (0x0A  >> 24)), // log only
			VERBOSE        = (1 << (0x0C  >> 24)), // log only
			PROGRESS       = (1 << (0x0A  >> 24)), // external handler only
			INITIALIZE     = (1 << (0x0C  >> 24)), // external handler only
			TERMINATE      = (1 << (0x0D  >> 24)), // external handler only
			SHOWDIALOG     = (1 << (0x0E  >> 24)), // external handler only
		};

		public enum INSTALLLOGATTRIBUTES // flag attributes for MsiEnableLog
		{
			APPEND            = (1 << 0),
			FLUSHEACHLINE     = (1 << 1),
		};
		
		
		/// <summary>Enumeration of installation modes.</summary>
		public enum INSTALLMODE
		{
			/// <summary>skip source resolution</summary>
			NOSOURCERESOLUTION   = -3,  // skip source resolution
			/// <summary>skip detection</summary>
			NODETECTION          = -2,  // skip detection
			/// <summary>provide, if available</summary>
			EXISTING             = -1,  // provide, if available
			/// <summary>install, if absent</summary>
			DEFAULT              =  0,  // install, if absent
		};

		public const int MAX_FEATURE_CHARS = 38;   // maximum chars in feature name (same as string GUID)

		[Nativemsi(WindowsInstallerVersion.version15,Context = msiContext.DesignTime | msiContext.RunTime)]
		public const int MSIASSEMBLYINFO_NETASSEMBLY  = 0; // Net assemblies
		
		[Nativemsi(WindowsInstallerVersion.version15,Context = msiContext.DesignTime | msiContext.RunTime)]
		public const int MSIASSEMBLYINFO_WIN32ASSEMBLY =1; // Win32 assemblies


		// Product info attributes: advertised information

		public const string INSTALLPROPERTY_PACKAGENAME    = "PackageName";
		public const string INSTALLPROPERTY_TRANSFORMS     = "Transforms";
		public const string INSTALLPROPERTY_LANGUAGE       = "Language";
		public const string INSTALLPROPERTY_PRODUCTNAME    = "ProductName";
		public const string INSTALLPROPERTY_ASSIGNMENTTYPE = "AssignmentType";
		public const string INSTALLPROPERTY_PACKAGECODE    = "PackageCode";
		public const string INSTALLPROPERTY_VERSION        = "Version";
		//if _WIN32_MSI >=  110
		[Nativemsi(WindowsInstallerVersion.version11,Context = msiContext.DesignTime | msiContext.RunTime)]
		public const string INSTALLPROPERTY_PRODUCTICON    = "ProductIcon";
		//endif //(_WIN32_MSI >=  110)

		// Product info attributes: installed information

		public const string INSTALLPROPERTY_INSTALLEDPRODUCTNAME = "InstalledProductName";
		public const string INSTALLPROPERTY_VERSIONSTRING        = "VersionString";
		public const string INSTALLPROPERTY_HELPLINK             = "HelpLink";
		public const string INSTALLPROPERTY_HELPTELEPHONE        = "HelpTelephone";
		public const string INSTALLPROPERTY_INSTALLLOCATION      = "InstallLocation";
		public const string INSTALLPROPERTY_INSTALLSOURCE        = "InstallSource";
		public const string INSTALLPROPERTY_INSTALLDATE          = "InstallDate";
		public const string INSTALLPROPERTY_PUBLISHER            = "Publisher";
		public const string INSTALLPROPERTY_LOCALPACKAGE         = "LocalPackage";
		public const string INSTALLPROPERTY_URLINFOABOUT         = "URLInfoAbout";
		public const string INSTALLPROPERTY_URLUPDATEINFO        = "URLUpdateInfo";
		public const string INSTALLPROPERTY_VERSIONMINOR         = "VersionMinor";
		public const string INSTALLPROPERTY_VERSIONMAJOR         = "VersionMajor";
		//if (_WIN32_MSI >= 300)
		[Nativemsi(WindowsInstallerVersion.version30,Context = msiContext.InstallTime)]
		public const string INSTALLPROPERTY_AUTHORIZED_LUA_APP   = "AuthorizedLUAApp";
		 //endif //(_WIN32_MSI >= 300)

        //#if (_WIN32_MSI >= 300)
		[Nativemsi(WindowsInstallerVersion.version30,Context = msiContext.InstallTime)]
		public const string  INSTALLPROPERTY_UNINSTALLABLE     = "Uninstallable";

		[Nativemsi(WindowsInstallerVersion.version30,Context = msiContext.InstallTime)]
		public const string  INSTALLPROPERTY_PRODUCTSTATE      = "State";

		[Nativemsi(WindowsInstallerVersion.version30,Context = msiContext.InstallTime)]
		public const string  INSTALLPROPERTY_PATCHSTATE        = "State";

		[Nativemsi(WindowsInstallerVersion.version30,Context = msiContext.InstallTime)]
		public const string  INSTALLPROPERTY_PATCHTYPE         = "PatchType";

		[Nativemsi(WindowsInstallerVersion.version30,Context = msiContext.InstallTime)]
		public const string  INSTALLPROPERTY_LUAENABLED        = "LUAEnabled";

		[Nativemsi(WindowsInstallerVersion.version30,Context = msiContext.InstallTime)]        
		public const string  INSTALLPROPERTY_DISPLAYNAME       = "DisplayName";

		[Nativemsi(WindowsInstallerVersion.version30,Context = msiContext.InstallTime)]
		public const string  INSTALLPROPERTY_MOREINFOURL       = "MoreInfoURL";

        // Source List Info attributes: Advertised information
		[Nativemsi(WindowsInstallerVersion.version30,Context = msiContext.InstallTime)]
		public const string  INSTALLPROPERTY_LASTUSEDSOURCE       = "LastUsedSource";

		[Nativemsi(WindowsInstallerVersion.version30,Context = msiContext.InstallTime)]
		public const string  INSTALLPROPERTY_LASTUSEDTYPE         = "LastUsedType";

		[Nativemsi(WindowsInstallerVersion.version30,Context = msiContext.InstallTime)]
		public const string  INSTALLPROPERTY_MEDIAPACKAGEPATH     = "MediaPackagePath";

		[Nativemsi(WindowsInstallerVersion.version30,Context = msiContext.InstallTime)]
		public const string  INSTALLPROPERTY_DISKPROMPT           = "DiskPrompt";
       //#endif // (_WIN32_MSI >= 300)



		/// <summary>Enumeration of various MSI install types.</summary>
		public enum INSTALLTYPE
		{
			/// <summary>set to indicate default behavior</summary>
			Default = 0,
			/// <summary>set to indicate network install</summary>
			NetworkImage = 1,
			/// <summary>set to indicate a particular instance</summary>
			SingleInstance =2

		};

		// MsiOpenDatabase persist predefine values, otherwise output database path is used
		
		/// <summary>Enumeration of database persistence modes.</summary>
		public enum MsidbOPEN : int
		{
			
			/// <summary>database open read-only, no persistent changes</summary>
			READONLY     =  0, 
			/// <summary>database read/write in transaction mode</summary>
			TRANSACT     =  1, 
			/// <summary>database direct read/write without transaction</summary>
			DIRECT       =  2, 
			/// <summary>create new database, transact mode read/write</summary>
			CREATE       =  3, 
			/// <summary>create new database, transact mode read/write</summary>
			CREATEDIRECT =  4,   
			/// <summary> add flag to indicate patch file</summary>
			PATCH        =  8  //public const MSIDBOPEN_PATCHFILE    32/sizeof(*MSIDBOPEN_READONLY)
		
		};

		// --------------------------------------------------------------------------
		// Functions for rendering UI dialogs from the database representations.
		// Purpose is for product development, not for use during installation.
		// --------------------------------------------------------------------------
		
		/// <summary>
		/// Enable UI in preview mode to facilitate authoring of UI dialogs.
		/// The preview mode will end when the handle is closed..
		/// </summary>
		[DllImport("msi")]
		public static extern int MsiEnableUIPreview(IntPtr msihandle,
			ref IntPtr preview_handle);       // returned handle for UI preview capability

		/// <summary>
		/// Display any UI dialog as modeless and inactive.
		/// Supplying a null name will remove any current dialog.
		/// </summary>
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiPreviewDialog(IntPtr preview_handle,
			string DialogName);      // dialog to display, Dialog table key
	 
		/// <summary>
		/// Display a billboard within a host control in the displayed dialog.
		///  Supplying a null billboard name will remove any billboard displayed.
		/// </summary>
		/// <param name="preview_handle"></param>
		/// <param name="ControlName"> name of control that accepts billboards</param>
		/// <param name="Billboard">name of billboard to display</param>
		/// <returns></returns>
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiPreviewBillboard(IntPtr preview_handle,
			string ControlName,      
			string Billboard);       

		/// <summary>
		/// Enable logging to a file for all install sessions for the client process,
		/// with control over which log messages are passed to the specified log file.
		/// Messages are designated with a combination of bits from INSTALLLOGMODE enum.
		/// </summary>
		/// <param name="LogMode">bit flags designating operations to report</param>
		/// <param name="LogFile">log file, or NULL to disable logging</param>
		/// <param name="LogAttributes"></param>
		/// <returns></returns>
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiEnableLog(
			int     LogMode,           
			string  LogFile,           
			int     LogAttributes);

		// --------------------------------------------------------------------------
		// Functions to query and configure a product as a whole.
		// --------------------------------------------------------------------------
		public enum MSIDBSTATE
		{
			ERROR    =-1,  // invalid database handle
			READ     = 0,  // database open read-only, no persistent changes
			WRITE    = 1,  // database readable and updatable
		} ;

		public enum MSIMODIFY
		{
			SEEK             =-1,  // reposition to current record primary key
			REFRESH          = 0,  // refetch current record data
			INSERT           = 1,  // insert new record, fails if matching key exists
			UPDATE           = 2,  // update existing non-key data of fetched record
			ASSIGN           = 3,  // insert record, replacing any existing record
			REPLACE          = 4,  // update record, delete old if primary key edit
			MERGE            = 5,  // fails if record with duplicate key not identical
			DELETE           = 6,  // remove row referenced by this record from table
			INSERT_TEMPORARY = 7,  // insert a temporary record
			VALIDATE         = 8,  // validate a fetched record
			VALIDATE_NEW     = 9,  // validate a new record
			VALIDATE_FIELD   = 10, // validate field(s) of an incomplete record
			VALIDATE_DELETE  = 11, // validate before deleting record
		} ;

		public enum MSICOLINFO
		{
			/// <summary>
			/// return column names
			/// </summary>
			NAMES = 0,  
			/// <summary>
			/// return column definitions, datatype code followed by width
			/// </summary>
			TYPES = 1, 
		} ;

		public enum  MSICONDITION
		{
			/// <summary> expression evaluates to False </summary>
			FALSE = 0, 
			/// <summary>
			/// expression evaluates to True
			/// </summary>
			TRUE  = 1,
			/// <summary>
			///  no expression present
			/// </summary>
			NONE  = 2,  
			/// <summary>
			/// syntax error in expression
			/// </summary>
			ERROR = 3, 
		};

		public enum MSICOSTTREE
		{
			SELFONLY = 0,
			CHILDREN = 1,
			PARENTS  = 2,
			/// <summary>
			/// Reserved for future use
			/// </summary>
			RESERVED = 3,	
		};

		/// <summary>
		/// 
		/// </summary>
		/// <param name="database">Handle to a database.</param>
		/// <param name="tableName">Table name.</param>
		/// <returns>MSICONDITION</returns>
		[DllImport("msi.dll", CharSet=CharSet.Auto)]
		internal static extern MSICONDITION MsiDatabaseIsTablePersistent(IntPtr database,
			 string tableName);

		/// <summary>
		/// Return the installed state for a product
		/// </summary>
		/// <param name="Product"></param>
		/// <returns></returns>
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern INSTALLSTATE MsiQueryProductState(
			string Product);
		
		/// <summary>
		/// The <c>MsiQueryComponentState</c> Query the installed state of a component in the specified product instance
		///</summary>
		/// <param name="szProductCode"> Information is queried on this product.</param>
		/// <param name="szUserSid">Account of this product instance.</param>
		/// <param name="dwContext">Context of this product instance.</param>
		/// <param name="szComponentCode"> Name of the component being queried.</param>
		/// <param name="pdwState">State value.</param>
		/// <returns></returns>
		/// <remarks>Please refer to the MSDN documentation for more information.</remarks>
		[WinAPI(OS.None,FileName = "Msi.h",UpgradeVersion = OS.WinXP)]
		[Nativemsi(WindowsInstallerVersion.version30, Context = msiContext.InstallTime)]
		[DllImport("msi", CharSet = CharSet.Auto)]
		extern static public int MsiQueryComponentState(
			string szProductCode, string szUserSid,
			MSIINSTALLCONTEXT dwContext,string szComponentCode,ref INSTALLSTATE pdwState);
		
		# region  install missing components and files
		// --------------------------------------------------------------------------
		// Functions to access or install missing components and files.
		// These should be used as a last resort.
		// --------------------------------------------------------------------------
      
		/// <summary>
		/// The <c>MsiInstallMissingComponent</c> Install a component unexpectedly missing, 
		/// provided only for error recovery
		/// This would typically occur due to failue to establish feature availability
		/// The product feature having the smallest incremental cost is installed
		///</summary>
		/// <param name="szProduct">product code.</param>
		/// <param name="szComponent">component Id, string GUID.</param>
		/// <param name="eInstallState">local/source/default, absent invalid.</param>
		/// <returns></returns>
		/// <remarks>Please refer to the MSDN documentation for more information.</remarks>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "Msi.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10, Context = msiContext.InstallTime)]
		[DllImport("msi", CharSet = CharSet.Auto)]
		extern static public int MsiInstallMissingComponent(
			string szProduct, string szComponent,
			INSTALLSTATE eInstallState);

		/// <summary>
		/// The <c>MsiInstallMissingFile</c> Install a file unexpectedly missing,
		/// provided only for error recovery
		/// This would typically occur due to failue to establish feature availability
		/// The missing component is determined from the product's File table, then
		/// the product feature having the smallest incremental cost is installed
       	///</summary>
		/// <param name="szProduct">product code.</param>
		/// <param name="szFile">file name, without path.</param>
		/// <returns></returns>
		/// <remarks>Please refer to the MSDN documentation for more information.</remarks>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "Msi.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10, Context = msiContext.InstallTime)]
		[DllImport("msi", CharSet = CharSet.Auto)]
		extern static public int MsiInstallMissingFile(
			string szProduct, string szFile);


		/// <summary>
		/// The <c>MsiLocateComponent</c> Install a component unexpectedly missing, 
		/// provided only for error recovery
		/// This would typically occur due to failue to establish feature availability
		/// The product feature having the smallest incremental cost is installed
		///</summary>
		/// <param name="szComponent">component Id, string GUID.</param>
		/// <param name="lpPathBuf">returned path.</param>
		/// <param name="pcchBuf">in/out buffer character count.</param>
		/// <returns></returns>
		/// <remarks>Please refer to the MSDN documentation for more information.</remarks>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "Msi.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10, Context = msiContext.InstallTime)]
		[DllImport("msi", CharSet = CharSet.Auto)]
		extern static public int MsiLocateComponent(
			string szComponent, string lpPathBuf,
			ref uint  pcchBuf);


		#endregion
	
		/// <summary>
		/// Return product info
		/// </summary>
		/// <param name="Product">product code</param>
		/// <param name="Attribute">attribute name, case-sensitive</param>
		/// <param name="Buffer">returned value, NULL if not desired ref?</param>
		/// <param name="length">in/out buffer character count</param>
		/// <returns></returns>
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiGetProductInfo(
			string   Product,       
			string   Attribute,     
			string   Buffer,     
			out int  length);      
	
		/// <summary>
		/// Return product info
		/// </summary>
		/// <param name="hProduct">product handle obtained from MsiOpenProduct</param>
		/// <param name="szProperty">property name, case-sensitive</param>
		/// <param name="lpValueBuf">returned value, NULL if not desired</param>
		/// <param name="pcchValueBuf">in/out buffer character count</param>
		/// <returns></returns>
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiGetProductProperty(
			IntPtr hProduct, string szProperty,	string  lpValueBuf,     
			ref uint pcchValueBuf);      


		/// <summary>
		/// Install a new product.
		/// Either may be NULL, but the DATABASE property must be specfied
		/// </summary>
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiInstallProduct(
			string      PackagePath,    // location of package to install
			string      CommandLine);   // command line <property settings>
      		
		/// <summary>
		/// Install/uninstall an advertised or installed product
		/// No action if installed and INSTALLSTATE_DEFAULT specified
		/// </summary>
		/// <param name="Product">product code</param>
		/// <param name="iInstallLevel">how much of the product to install</param>
		/// <param name="eInstallState">local/source/default/absent/lock/uncache</param>
		/// <returns></returns>
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiConfigureProduct(
			string       Product,       
			int          iInstallLevel,   
			INSTALLSTATE eInstallState);   

		/// <summary>
		/// Reinstall product, used to validate or correct problems
		/// </summary>
		/// <param name="Product">product code</param>
		/// <param name="ReinstallMode">one or more REINSTALLMODE modes</param>
		/// <returns></returns>
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiReinstallProduct(
			string     Product,        
			int        ReinstallMode);
 
		/// <summary>
		/// Return the product code for a registered component, called once by apps
		/// </summary>
		/// <param name="Component">component Id registered for this product</param>
		/// <param name="Buffer39">returned string GUID, sized for 39 characters</param>
		/// <returns></returns>
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiGetProductCode(
			string    Component,  
			string    Buffer39);   
   
		/// <summary>
		/// Return the registered user information for an installed product
		/// </summary>
		/// <param name="Product">product code, string GUID</param>
		/// <param name="UserNameBuffer">return user name </param>
		/// <param name="UserNameBufferLength">in/out buffer character count</param>
		/// <param name="OrgNameBuffer">return company name  </param>
		/// <param name="OrgNameBufferLength">in/out buffer character count</param>
		/// <param name="SerialBuffer">return product serial number</param>
		/// <param name="SerialBufferLength">in/out buffer character count</param>
		/// <returns></returns>
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern USERINFOSTATE MsiGetUserInfo(
			string   Product,         
			string   UserNameBuffer,              
			ref int  UserNameBufferLength,  
			string   OrgNameBuffer,           
			ref int  OrgNameBufferLength,  
			string   SerialBuffer,     
			ref int  SerialBufferLength); 
	
		/// <summary>
		/// Obtain and store user info and PID from installation wizard (first run)
		/// </summary>
		/// <param name="Product">product code, string GUID</param>
		/// <returns></returns>
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiCollectUserInfo(
			string  Product); 
    


		//msiQuery.h
 
		// --------------------------------------------------------------------------
		// Installer database management functions - not used by custom actions
		// --------------------------------------------------------------------------

		/// <summary>	
		/// Open an installer database, specifying the persistance mode, which is a pointer.
		/// Predefined persist values are reserved pointer values, requiring pointer arithmetic.
		/// Execution of this function sets the error record, accessible via MsiGetLastErrorRecord
		/// </summary>
		/// <param name="dbpath"></param>
		/// <param name="persist"></param>
		/// <param name="msihandle"></param>
		/// <returns></returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,Context = msiContext.DesignTime | msiContext.RunTime)]
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiOpenDatabase (string dbpath,
			MsidbOPEN persist, 
			out IntPtr msihandle);

		/// <summary>	
		/// Open a product package in order to access product properties
		/// Option to create a "safe" engine that does not look at machine state
		///  and does not allow for modification of machine state
		/// </summary>
		/// <param name="szPackagePath">path to package, or database handle: #nnnn</param>
		/// <param name="hProduct">returned product handle, must be closed</param>
		/// <returns></returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msi.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version15,Context = msiContext.DesignTime | msiContext.RunTime)]
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiOpenPackage (string szPackagePath,
			out IntPtr hProduct);

		/// <summary>	
		/// Open a product package in order to access product properties
		/// Option to create a "safe" engine that does not look at machine state
		///  and does not allow for modification of machine state
		/// </summary>
		/// <param name="szPackagePath">path to package, or database handle: #nnnn</param>
		/// <param name="dwOptions">options flags to indicate whether or not to ignore machine state</param>
		/// <param name="hProduct">returned product handle, must be closed</param>
		/// <returns></returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msi.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version15,Context = msiContext.DesignTime | msiContext.RunTime)]
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiOpenPackageEx (string szPackagePath,
			uint  dwOptions,out IntPtr hProduct);
 
		/// <summary>
		/// Import an MSI text archive table into an open database
		/// Execution of this function sets the error record, accessible via MsiGetLastErrorRecord
		/// </summary>
		/// <param name="msihandle"></param>
		/// <param name="FolderPath">folder containing archive files</param>
		/// <param name="FileName">table archive file to be imported</param>
		/// <returns></returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,Context = msiContext.DesignTime | msiContext.RunTime)]
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiDatabaseImport(IntPtr msihandle,
			string   FolderPath,     
			string   FileName);      

		/// <summary>
		/// Export an MSI table from an open database to a text archive file
		/// Execution of this function sets the error record, accessible via MsiGetLastErrorRecord
		/// </summary>
		/// <param name="msihandle"></param>
		/// <param name="TableName">name of table in database <case-sensitive></param>
		/// <param name="FolderPath">folder containing archive files</param>
		/// <param name="FileName">name of exported table archive file</param>
		/// <returns></returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,Context = msiContext.DesignTime | msiContext.RunTime)]
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern uint MsiDatabaseExport(IntPtr msihandle,
			string   TableName,     
			string   FolderPath,    
			string   FileName);     

		/// <summary>
		/// Merge two database together, allowing duplicate rows
		/// Execution of this function sets the error record, accessible via MsiGetLastErrorRecord
		/// </summary>
		/// <param name="msihandle">Handle to the database</param>
		/// <param name="msihandle2">database to be merged into hDatabase</param>
		/// <param name="TableName">name of non-persistent table to receive errors</param>
		/// <returns></returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,Context = msiContext.DesignTime | msiContext.RunTime)]
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiDatabaseMerge(IntPtr msihandle,
			IntPtr msihandle2,     
			string TableName);     
	
		/// <summary>
		/// Write out all persistent table data, ignored if database opened read-only
		/// Execution of this function sets the error record, accessible via MsiGetLastErrorRecord
		/// </summary>
		/// <param name="msihandle"></param>
		/// <returns></returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,Context = msiContext.DesignTime | msiContext.RunTime)]
		[DllImport("msi",SetLastError = true)]
		public static extern int MsiDatabaseCommit(IntPtr msihandle);

		/// <summary>
		///  Return the update state of a database
		/// </summary>
		/// <param name="msihandle"></param>
		/// <returns></returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,
			 Context = msiContext.DesignTime | msiContext.RunTime)]
		[DllImport("msi")]
		public static extern MSIDBSTATE MsiGetDatabaseState(IntPtr msihandle);
	
		// --------------------------------------------------------------------------
		// Installer database access functions
		// --------------------------------------------------------------------------


		/// <summary>
		///  Prepare a database query, creating a view object
		/// Returns ERROR_SUCCESS if successful, and the view handle is returned,
		/// else ERROR_INVALID_HANDLE, ERROR_INVALID_HANDLE_STATE, ERROR_BAD_QUERY_SYNTAX, ERROR_GEN_FAILURE
		/// Execution of this function sets the error record, accessible via MsiGetLastErrorRecord
		/// </summary>
		/// <param name="msihandle"></param>
		/// <param name="query"></param>
		/// <param name="viewhandle"></param>
		/// <returns></returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,
			 Context = msiContext.DesignTime | msiContext.RunTime)]
		[DllImport("msi")]
		public static extern int MsiDatabaseOpenView(IntPtr msihandle, string query, out IntPtr viewhandle);
 
		/// <summary>
		/// Exectute the view query, supplying parameters as required
		/// Execution of this function sets the error record, accessible via MsiGetLastErrorRecord
		/// </summary>
		/// <param name="viewhandle"></param>
		/// <param name="recordhandle"></param>
		/// <returns>
		/// Returns ERROR_SUCCESS, ERROR_INVALID_HANDLE, ERROR_INVALID_HANDLE_STATE, 
		/// ERROR_GEN_FAILURE
		/// </returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,
			 Context = msiContext.DesignTime | msiContext.RunTime)]	
		[DllImport("msi")]
		public static extern int MsiViewExecute (IntPtr viewhandle, IntPtr recordhandle);

		/// <summary>
		///  Fetch the next sequential record from the view
		/// Result is ERROR_SUCCESS if a row is found, and its handle is returned
		/// else ERROR_NO_MORE_ITEMS if no records remain, and a null handle is returned
		/// else result is error: ERROR_INVALID_HANDLE_STATE, ERROR_INVALID_HANDLE,
		///  ERROR_GEN_FAILURE 	
		/// </summary>
		/// <param name="viewhandle"></param>
		/// <param name="recordhandle"></param>
		/// <returns></returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,
			 Context = msiContext.DesignTime | msiContext.RunTime)]	
		[DllImport("msi")]
		public static extern int MsiViewFetch (IntPtr viewhandle, out IntPtr recordhandle);

		/// <summary>
		/// Modify a database record, parameters must match types in query columns
		/// Returns ERROR_SUCCESS, ERROR_INVALID_HANDLE, ERROR_INVALID_HANDLE_STATE, ERROR_GEN_FAILURE, ERROR_ACCESS_DENIED
		/// Execution of this function sets the error record, accessible via MsiGetLastErrorRecord
		/// </summary>
		/// <param name="viewhandle"></param>
		/// <param name="eModifyMode">modify action to perform</param>
		/// <param name="recordhandle">record obtained from fetch, or new record</param>
		/// <returns></returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,
			 Context = msiContext.DesignTime | msiContext.RunTime)]	
		[DllImport("msi")]
		public static extern int MsiViewModify(IntPtr viewhandle,
			ModifyView eModifyMode,      
			IntPtr recordhandle);           
 
		/// <summary>
		///Return the column names or specifications for the current view
		/// Returns ERROR_SUCCESS, ERROR_INVALID_HANDLE, ERROR_INVALID_PARAMETER,
		///  or ERROR_INVALID_HANDLE_STATE
		/// </summary>
		/// <param name="viewhandle"></param>
		/// <param name="eColumnInfo">retrieve columns names or definitions</param>
		/// <param name="recordhandle">returned data record containing all names or definitions</param>
		/// <returns></returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,
			 Context = msiContext.DesignTime | msiContext.RunTime)]	
		[DllImport("msi")]
		public static extern int MsiViewGetColumnInfo(IntPtr viewhandle,
			MSICOLINFO eColumnInfo,     
			out IntPtr recordhandle);         
			
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,
			 Context = msiContext.DesignTime | msiContext.RunTime)]		
		[DllImport("msi")]
		public static extern int MsiCloseHandle (IntPtr handle);
 	
		/// <summary>
		/// Release the result set for an executed view, to allow re-execution
		/// Only needs to be called if not all records have been fetched
		/// </summary>
		/// <param name="viewhandle"></param>
		/// <returns>Returns ERROR_SUCCESS, ERROR_INVALID_HANDLE, ERROR_INVALID_HANDLE_STATE</returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,
			 Context = msiContext.DesignTime | msiContext.RunTime)]		
		[DllImport("msi")]
		public static extern int MsiViewClose  (IntPtr viewhandle);
 	
		// Returns ERROR_SUCCESS, ERROR_INVALID_HANDLE, ERROR_INVALID_TABLE
        /// <summary>
        /// Return a record containing the names of all primary key columns for a given table
        /// Returns an MSIHANDLE for a record containing the name of each column.
        /// The field count of the record corresponds to the number of primary key columns.
        /// Field [0] of the record contains the table name.
        /// </summary>
        /// <param name="msihandle"></param>
        /// <param name="TableName">the name of a specific table <case-sensitive></param>
        /// <param name="recordhandle">returned record if ERROR_SUCCESS</param>
        /// <returns></returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,
			 Context = msiContext.DesignTime | msiContext.RunTime)]	
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiDatabaseGetPrimaryKeys(IntPtr msihandle,
			string     TableName,       
			out IntPtr recordhandle);         
	
	
		// --------------------------------------------------------------------------
		// Record object functions
		// --------------------------------------------------------------------------


		/// <summary>
		/// Create a new record object with the requested number of fields
		/// Field 0, not included in count, is used for format strings and op codes
		/// All fields are initialized to null
		/// Returns a handle to the created record, or 0 if memory could not be allocated
		/// </summary>
		/// <param name="Params">the number of data fields</param>
		/// <returns></returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,
			 Context = msiContext.DesignTime | msiContext.InstallTime)]	
		[DllImport("msi")]
		public static extern IntPtr MsiCreateRecord(
			int Params);                   

		/// <summary>
		/// Report whether a record field is NULL
		/// Returns TRUE if the field is null or does not exist
		/// Returns FALSE if the field contains data, or the handle is invalid
		/// </summary>
		/// <param name="recordhandle"></param>
		/// <param name="Field"></param>
		/// <returns></returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,
			 Context = msiContext.DesignTime | msiContext.InstallTime)]	
		[DllImport("msi")]
		public static extern bool MsiRecordIsNull(IntPtr recordhandle,
			int Field);

		/// <summary>
		/// Return the length of a record field
		/// Returns 0 if field is NULL or non-existent
		/// Returns sizeof(int) if integer data
		/// Returns character count if string data (not counting null terminator)
		/// Returns bytes count if stream data
		/// </summary>
		/// <param name="recordhandle"></param>
		/// <param name="Field"></param>
		/// <returns></returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,
			 Context = msiContext.DesignTime | msiContext.InstallTime)]
		[DllImport("msi")]
		public static extern int MsiRecordDataSize(IntPtr recordhandle,
			int Field);

		/// <summary>
		/// Set a record field to an integer value 
		/// </summary>
		/// <param name="recordhandle"></param>
		/// <param name="Field"></param>
		/// <param name="Value"></param>
		/// <returns>Returns ERROR_SUCCESS, ERROR_INVALID_HANDLE, ERROR_INVALID_FIELD</returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,
			 Context = msiContext.DesignTime | msiContext.InstallTime)]
		[DllImport("msi")]
		public static extern int MsiRecordSetInteger(IntPtr recordhandle,
			int Field,
			int Value);
 
		/// <summary>
		/// Copy a string into the designated field
		/// A null string pointer and an empty string both set the field to null
		/// </summary>
		/// <param name="recordhandle"></param>
		/// <param name="Field"></param>
		/// <param name="Value"></param>
		/// <returns>Returns ERROR_SUCCESS, ERROR_INVALID_HANDLE, ERROR_INVALID_FIELD</returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,
			 Context = msiContext.DesignTime | msiContext.InstallTime)]
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiRecordSetString(IntPtr recordhandle,
			int    Field,
			string Value);
 
		/// <summary>
		/// 
		/// </summary>
		/// <param name="recordhandle"></param>
		/// <param name="Field"></param>
		/// <returns>
		/// Return the integer value from a record field
		/// Returns the value MSI_NULL_INTEGER if the field is null
		/// or if the field is a string that cannot be converted to an integer
		/// </returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,
			 Context = msiContext.DesignTime | msiContext.InstallTime)]
		[DllImport("msi")]
		public static extern int MsiRecordGetInteger(IntPtr recordhandle,
			int    Field);
 
		/// <summary>
		/// Return the string value of a record field
		/// Integer fields will be converted to a string
		/// Null and non-existent fields will report a value of 0
		/// Fields containing stream data will return ERROR_INVALID_DATATYPE
		/// </summary>
		/// <param name="recordhandle"></param>
		/// <param name="Field"></param>
		/// <param name="ValueBuffer">buffer for returned value</param>
		/// <param name="length">in/out buffer character count</param>
		/// <returns>
		/// Returns ERROR_SUCCESS, ERROR_MORE_DATA, 
		///         ERROR_INVALID_HANDLE, ERROR_INVALID_FIELD, ERROR_BAD_ARGUMENTS
		///	</returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,
			 Context = msiContext.DesignTime | msiContext.InstallTime)]
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiRecordGetString(IntPtr recordhandle,
			int     Field,
			string  ValueBuffer,        
			out int length);            
 
		 

		/// <summary>
		/// Returns the number of fields allocated in the record
		/// Does not count field 0, used for formatting and op codes
		/// </summary>
		/// <param name="recordhandle"></param>
		/// <returns></returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,
			 Context = msiContext.DesignTime | msiContext.InstallTime)]
		[DllImport("msi")]
		public static extern int MsiRecordGetFieldCount(IntPtr recordhandle);

		/// <summary>
		///  Set a record stream field from a file
		/// The contents of the specified file will be read into a stream object
		/// The stream will be persisted if the record is inserted into the database
		/// Execution of this function sets the error record, accessible via MsiGetLastErrorRecord
		/// </summary>
		/// <param name="recordhandle"></param>
		/// <param name="Field"></param>
		/// <param name="FilePath">path to file containing stream data</param>
		/// <returns></returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,
			 Context = msiContext.DesignTime | msiContext.InstallTime)]
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiRecordSetStream(IntPtr recordhandle,
			int     Field,
			string  FilePath);   
 
		/// <summary>
		/// Read bytes from a record stream field into a buffer
		/// Must set the in/out argument to the requested byte count to read
		/// The number of bytes transferred is returned through the argument
		/// If no more bytes are available, ERROR_SUCCESS is still returned
		/// </summary>
		/// <param name="recordhandle"></param>
		/// <param name="Field"></param>
		/// <param name="DataBuffer">buffer to receive bytes from stream</param>
		/// <param name="len">in/out buffer byte count</param>
		/// <returns></returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,
			 Context = msiContext.DesignTime | msiContext.InstallTime)]
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiRecordReadStream(IntPtr recordhandle,
			int      Field,
			StringBuilder DataBuffer,    
			ref int  len);   
 
		/// <summary>
		/// Clears all data fields in a record to NULL
		/// </summary>
		/// <param name="recordhandle"></param>
		/// <returns></returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,
			 Context = msiContext.DesignTime | msiContext.InstallTime)]
		[DllImport("msi")]
		public static extern int MsiRecordClearData(IntPtr recordhandle);

 	
 
		#endregion

		#region Machine Manipulation
		// this deals with installer functions on a machine

	
		//__________________________________________________________________________
		//
		//  SummaryInformation property stream property IDs
		//__________________________________________________________________________



		// standard property definitions, from OLE2 documentation
		public const int PID_DICTIONARY    = ( 0 );// integer count + array of entries
		public const int PID_CODEPAGE      =( 0x1 );// short integer
		public const int PID_TITLE         =  2;  // string
		public const int PID_SUBJECT       =  3;  // string
		public const int PID_AUTHOR        =  4 ; // string
		public const int PID_KEYWORDS      =  5;  // string
		public const int PID_COMMENTS      =  6;  // string
		public const int PID_TEMPLATE      =  7 ; // string
		public const int PID_LASTAUTHOR    =  8;  // string
		public const int PID_REVNUMBER     =  9;  // string
		public const int PID_EDITTIME      = 10;  // datatime
		public const int PID_LASTPRINTED   = 11;  // datetime
		public const int PID_CREATE_DTM    = 12;  // datetime
		public const int PID_LASTSAVE_DTM  = 13;  // datetime
		public const int PID_PAGECOUNT     = 14;  // integer 
		public const int PID_WORDCOUNT   = 15; // integer 
		public const int PID_CHARCOUNT   = 16 ; // integer 
		public const int PID_THUMBNAIL   = 17;  // clipboard format + metafile/bitmap (not supported)
		public const int PID_APPNAME     = 18;  // string
		public const int PID_SECURITY    = 19 ; // integer

		// PIDs given specific meanings for Installer
		public const int PID_MSIVERSION   =  PID_PAGECOUNT;  // integer, Installer version number (major*100+minor)
		public const int PID_MSISOURCE    =  PID_WORDCOUNT;  // integer, type of file image, short/long, media/tree
		public const int PID_MSIRESTRICT   = PID_CHARCOUNT;  // integer, transform restriction
	
	
		// --------------------------------------------------------------------------
		// Functions to iterate registered products, features, and components.
		// As with reg keys, they accept a 0-based index into the enumeration.
		// --------------------------------------------------------------------------

		// 
		/// <summary>
		/// Enumerate the registered products, either installed or advertised
		/// </summary>
		/// <param name="iProductIndex">0-based index into registered products</param>
		/// <param name="ProductBuffer">buffer of char count: 39 (size of string GUID)</param>
		/// <returns></returns>
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiEnumProducts(
			int      iProductIndex,    
			string   ProductBuffer);   

		//if (_WIN32_MSI >=  110)

		/// <summary>
		/// Enumerate products with given upgrade code
		/// </summary>
		/// <param name="UpgradeCode">upgrade code of products to enumerate</param>
		/// <param name="dwReserved">reserved, must be 0</param>
		/// <param name="iProductIndex">0-based index into registered products</param>
		/// <param name="ProductBuffer"> buffer of char count: 39 (size of string GUID)</param>
		/// <returns></returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version11,
			 Context = msiContext.InstallTime | msiContext.RunTime )]
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiEnumRelatedProducts(
			string  UpgradeCode,    
			int     dwReserved,       
			int     iProductIndex,    
			string  ProductBuffer);    
		//endif //(_WIN32_MSI >=  110)

		/// <summary>
		///  Enumerate the advertised features for a given product.
		///  If parent is not required, supplying NULL will improve performance.
		/// </summary>
		/// <param name="szProduct"></param>
		/// <param name="iFeatureIndex">0-based index into published features</param>
		/// <param name="FeatureBuffer">feature name buffer, size=MAX_FEATURE_CHARS+1</param>
		/// <param name="ParentBuffer">parent feature buffer,size=MAX_FEATURE_CHARS+1</param>
		/// <returns></returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "Msi.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10,
			 Context = msiContext.InstallTime | msiContext.RunTime )]
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiEnumFeatures(
			string  szProduct,
			int     iFeatureIndex,  
			string  FeatureBuffer,   
			string  ParentBuffer);  
		

		// Enumerate the client products for a component
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiEnumClients(
			string  szComponent,
			int     iProductIndex,    // 0-based index into client products
			string  ProductBuffer);    // buffer of char count: 39 (size of string GUID)
	

		// Enumerate the qualifiers for an advertised component.
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiEnumComponentQualifiers(
			string   szComponent,         // generic component ID that is qualified
			int      iIndex,	           // 0-based index into qualifiers
			string   QualifierBuffer,      // qualifier buffer
			ref int  QualifierBufferLength,   // in/out qualifier buffer character count
			string   ApplicationDataBuffer,    // description buffer
			ref int  ApplicationDataBufferLength); // in/out description buffer character count
	
		// --------------------------------------------------------------------------
		// Summary information stream management functions
		// --------------------------------------------------------------------------

		// Integer Property IDs:    1, 14, 15, 16, 19 
		// DateTime Property IDs:   10, 11, 12, 13
		// Text Property IDs:       2, 3, 4, 5, 6, 7, 8, 9, 18
		// Unsupported Propery IDs: 0 (PID_DICTIONARY), 17 (PID_THUMBNAIL)

		// Obtain a handle for the _SummaryInformation stream for an MSI database     
		// Execution of this function sets the error record, accessible via MsiGetLastErrorRecord
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiGetSummaryInformation(IntPtr msihandle, // 0 if not open
			string  szDatabasePath,  // path to database, 0 if database handle supplied
			int     uiUpdateCount,    // maximium number of updated values, 0 to open read-only
			ref IntPtr SummaryInfo); // returned handle to summary information data
	
		// Obtain the number of existing properties in the SummaryInformation stream
		[DllImport("msi")]
		public static extern int MsiSummaryInfoGetPropertyCount(IntPtr SummaryInfo,
			ref int PropertyCount); // pointer to location to return total property count

		// Set a single summary information property
		// Returns ERROR_SUCCESS, ERROR_INVALID_HANDLE, ERROR_UNKNOWN_PROPERTY
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiSummaryInfoSetProperty(IntPtr SummaryInfo,
			int      uiProperty,     // property ID, one of allowed values for summary information
			int      uiDataType,     // VT_I4, VT_LPSTR, VT_FILETIME, or VT_EMPTY
			/*INT*/    int      iValue,         // integer value, used only if integer property
			ref	FILETIME pftValue,      // pointer to filetime value, used only if datetime property
			string   Value);       // text value, used only if string property
	
		// Get a single property from the summary information
		// Returns ERROR_SUCCESS, ERROR_INVALID_HANDLE, ERROR_UNKNOWN_PROPERTY
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiSummaryInfoGetProperty(IntPtr SummaryInfo,
			int      uiProperty,     // property ID, one of allowed values for summary information
			ref int  uiDataType,   // returned type: VT_I4, VT_LPSTR, VT_FILETIME, VT_EMPTY
			/*INT*/     ref int  Value,      // returned integer property data
			ref	FILETIME pftValue,      // returned datetime property data
			string   ValueBuffer,     // buffer to return string property data
			ref int  ValueBufferLength); // in/out buffer character count
	

		// Write back changed information to summary information stream
		[DllImport("msi")]
		public static extern int MsiSummaryInfoPersist(IntPtr SummaryInfo);

	
		#endregion

		#region Install Time Functionality

		// this deals with install time functionality
	
	
		public enum INSTALLUILEVEL : long
		{
			NOCHANGE = 0,    // UI level is unchanged
			DEFAULT  = 1,    // default UI is used
			NONE     = 2,    // completely silent installation
			BASIC    = 3,    // simple progress and error handling
			REDUCED  = 4,    // authored UI, wizard dialogs suppressed
			FULL     = 5,    // authored UI with wizards, progress, errors
			ENDDIALOG    = 0x80, // display success/failure dialog at end of install
			PROGRESSONLY = 0x40, // display only progress dialog
		};

		#region	Delegates
		/// <summary>The <c>INSTALLUI_HANDLER</c> delegate defines a callback function that the installer calls for progress notification and error messages.</summary>
		/// <remarks>
		/// The <c>messageType</c> parameter specifies a combination of one message box style, one message box icon type, one default button, and one installation message type.
		/// </remarks>
		public delegate int MsiInstallUIHandler(IntPtr context,
			uint messageType, [MarshalAs(UnmanagedType.LPTStr)] string message);
		#endregion	Delegates

		/// <summary>
		/// modify the default attributes of a feature at runtime(InstallTime).
		///  Note that the default attributes of features are authored
		///   in the Attributes column of the Feature table
		/// </summary>
		/// <param name="hInstall">Handle to the installation provided 
		/// to a DLL custom action or obtained through MsiOpenPackage,
		///  MsiOpenPackageEx, or MsiOpenProduct. 
		/// </param>
		/// <param name="szFeature">Specifies the feature name within the product.	</param>
		/// <param name="dwAttributes">Feature attributes specified at run time as a set of bit flags</param>
		/// <returns></returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version11,
			 Context = msiContext.InstallTime)]
		[DllImport("msi", CharSet=CharSet.Auto)]
		public static extern int MsiSetFeatureAttributes(IntPtr hInstall, 
			string szFeature,
			long dwAttributes);
       
		// --------------------------------------------------------------------------
		// Functions to set the UI handling and logging. The UI will be used for error,
		// progress, and log messages for all subsequent calls to Installer Service
		// API functions that require UI.
		// --------------------------------------------------------------------------
		
		/// <summary>
		///  Enable internal UI
		/// </summary>
		/// <param name="dwUILevel">UI level</param>
		/// <param name="winhandle">handle of owner window</param>
		/// <returns></returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10, Context = msiContext.InstallTime)]
		[DllImport("msi", CharSet=CharSet.Auto)]              
		public static extern INSTALLUILEVEL MsiSetInternalUI (long dwUILevel,ref IntPtr winhandle);        

		/// <summary>The <c>MsiSetExternalUI</c> function enables an external user-interface handler. This external UI handler is called before the normal internal user-interface handler. The external UI handler has the option to suppress the internal UI by returning a non-zero value to indicate that it has handled the messages.</summary>
		/// <param name="handler">The <see cref="MsiInstallUIHandler"/> handler delegate.</param>
		/// <param name="filter">Specifies which messages to handle using the external message handler. If the external handler returns a non-zero result, then that message will not be sent to the UI, instead the message will be logged if logging has been enabled. See <see cref="MsiEnableLog"/>.</param>
		/// <param name="context">Pointer to an application context that is passed to the callback function. This parameter can be used for error checking.</param>
		/// <returns>The return value is the previously set external handler, or <c>null</c> if there was no previously set handler.</returns>
		/// <remarks>Please refer to the MSDN documentation for more information.</remarks>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "msiQuery.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10, Context = msiContext.InstallTime)]
		[DllImport("msi", CharSet = CharSet.Auto)]
		extern static public MsiInstallUIHandler MsiSetExternalUI(
			[MarshalAs(UnmanagedType.FunctionPtr)]
			MsiInstallUIHandler handler, MsiInstallLogMode filter,
			IntPtr context);

		/// <summary>
		/// The <c>MsiUseFeature</c> Indicate intent to use a product feature, increments usage count
		/// Prompts for CD if not loaded, does not install feature
		///</summary>
		/// <param name="szProduct">product code.</param>
		/// <param name="szFeature">feature ID.</param>
		/// <returns></returns>
		/// <remarks>Please refer to the MSDN documentation for more information.</remarks>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "Msi.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10, Context = msiContext.InstallTime)]
		[DllImport("msi", CharSet = CharSet.Auto)]
		extern static public INSTALLSTATE MsiUseFeature(
			string szProduct, string szFeature);
		
		/// <summary>
		/// The <c>MsiUseFeatureEx</c> Indicate intent to use a product feature, increments usage count
		/// Prompts for CD if not loaded, does not install feature
		/// Allows for bypassing component detection where performance is critical
		///</summary>
		/// <param name="szProduct">product code.</param>
		/// <param name="szFeature">feature ID.</param>
		/// <param name="dwInstallMode">INSTALLMODE_NODETECTION, else 0.</param>
		/// <param name="dwReserved">reserved, must be 0.</param>
		/// <returns></returns>
		/// <remarks>Please refer to the MSDN documentation for more information.</remarks>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "Msi.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10, Context = msiContext.InstallTime)]
		[DllImport("msi", CharSet = CharSet.Auto)]
		extern static public INSTALLSTATE MsiUseFeatureEx(
			string szProduct, string szFeature,
			 uint dwInstallMode ,uint dwReserved );
				
		/// <summary>
		/// The <c>MsiGetFeatureUsage</c> Returns the usage metrics for a product features usage count
		///</summary>
		/// <param name="szProduct">product code.</param>
		/// <param name="szFeature">feature ID.</param>
		/// <param name="pdwUseCount">returned use count.</param>
		/// <param name="pwDateUsed">last date used (DOS date format).</param>
		/// <returns></returns>
		/// <remarks>Please refer to the MSDN documentation for more information.</remarks>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "Msi.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10, Context = msiContext.InstallTime)]
		[DllImport("msi", CharSet = CharSet.Auto)]
		extern static public INSTALLSTATE MsiGetFeatureUsage(
			string szProduct, string szFeature,
			ref uint pdwUseCount ,ref uint pwDateUsed );
        
		/// <summary>
		/// The <c>MsiConfigureFeature</c> Returns the usage metrics for a product features usage count
		///</summary>
		/// <param name="szProduct">product code.</param>
		/// <param name="szFeature">feature ID.</param>
		/// <param name="pdwUseCount">returned use count.</param>
		/// <param name="pwDateUsed">last date used (DOS date format).</param>
		/// <returns></returns>
		/// <remarks>Please refer to the MSDN documentation for more information.</remarks>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "Msi.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10, Context = msiContext.InstallTime)]
		[DllImport("msi", CharSet = CharSet.Auto)]
		extern static public INSTALLSTATE MsiConfigureFeature(
			string szProduct, string szFeature,
			ref uint pdwUseCount ,ref uint pwDateUsed );

		/// <summary>
		/// The <c>MsiReinstallFeature</c> Reinstall feature, used to validate or correct problems
		///</summary>
		/// <param name="szProduct">product code.</param>
		/// <param name="szFeature">feature ID, NULL for entire product.</param>
		/// <param name="dwReinstallMode">one or more REINSTALLMODE modes.</param>
		/// <returns></returns>
		/// <remarks>Please refer to the MSDN documentation for more information.</remarks>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "Msi.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10, Context = msiContext.InstallTime)]
		[DllImport("msi", CharSet = CharSet.Auto)]
		extern static public INSTALLSTATE MsiReinstallFeature(
			string szProduct, string szFeature,
			uint dwReinstallMode);

		/// <summary>
		/// The <c>MsiProvideComponent</c> Return full component path, performing any necessary installation
		/// calls MsiQueryFeatureState to detect that all components are installed
		/// then calls MsiConfigureFeature if any of its components are uninstalled
		/// then calls MsiLocateComponent to obtain the path the its key file
		///</summary>
		/// <param name="szProduct">product code in case install required.</param>
		/// <param name="szFeature">feature ID in case install required.</param>
		/// <param name="szComponent">component ID.</param>
		/// <param name="dwReinstallMode">either of type INSTALLMODE or a combination of the REINSTALLMODE flags.</param>
		/// <param name="lpPathBuf">returned path, NULL if not desired.</param>
		/// <param name="pcchPathBuf">in/out buffer character count.</param>
		/// <returns></returns>
		/// <remarks>Please refer to the MSDN documentation for more information.</remarks>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "Msi.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10, Context = msiContext.InstallTime)]
		[DllImport("msi", CharSet = CharSet.Auto)]
		extern static public int MsiProvideComponent(
			string szProduct, string szFeature,string szComponent,
			uint dwReinstallMode,string lpPathBuf,ref uint pcchPathBuf);
 
		/// <summary>
		/// The <c>MsiProvideQualifiedComponent</c> Return full component path for a qualified component,
		///  performing any necessary installation. 
		/// Prompts for source if necessary and increments the usage count for the feature.
      	///</summary>
		/// <param name="szCategory"> component category ID.</param>
		/// <param name="szQualifier">specifies which component to access.</param>
		/// <param name="dwReinstallMode">either of type INSTALLMODE or a combination of the REINSTALLMODE flags.</param>
		/// <param name="lpPathBuf"> returned path, NULL if not desired.</param>
		/// <param name="pcchPathBuf">in/out buffer character count.</param>
		/// <returns></returns>
		/// <remarks>Please refer to the MSDN documentation for more information.</remarks>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "Msi.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10, Context = msiContext.InstallTime)]
		[DllImport("msi", CharSet = CharSet.Auto)]
		extern static public int MsiProvideQualifiedComponent(
			string szCategory, string  szQualifier,
			uint  dwInstallMode,string lpPathBuf,ref uint pcchPathBuf);
      
		/// <summary>
		/// The <c>MsiProvideQualifiedComponentEx</c> Return full component path for a qualified component, performing any necessary installation. 
		/// Prompts for source if necessary and increments the usage count for the feature.
		/// The szProduct parameter specifies the product to match that has published the qualified
		/// component. If null, this API works the same as MsiProvideQualifiedComponent.
		///</summary>
		/// <param name="szCategory"> component category ID.</param>
		/// <param name="szQualifier">specifies which component to access.</param>
		/// <param name="dwInstallMode">either of type INSTALLMODE or a combination of the REINSTALLMODE flags.</param>
		/// <param name="szProduct"> the product code .</param>
		/// <param name="dwUnused1">not used, must be zero.</param>
		/// <param name="dwUnused2"> not used, must be zero.</param>
		/// <param name="lpPathBuf"> returned path, NULL if not desired.</param>
		/// <param name="pcchPathBuf">in/out buffer character count.</param>
		/// <returns></returns>
		/// <remarks>Please refer to the MSDN documentation for more information.</remarks>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "Msi.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10, Context = msiContext.InstallTime)]
		[DllImport("msi", CharSet = CharSet.Auto)]
		extern static public int MsiProvideQualifiedComponentEx(
			string szCategory, string  szQualifier,	uint dwInstallMode,
			string szProduct, uint dwUnused1, uint dwUnused2,
		    string lpPathBuf,ref uint pcchPathBuf);

		/// <summary>
		/// The <c>MsiProvideAssembly</c> Return full component path for a qualified component,
		///  performing any necessary installation. 
		/// Prompts for source if necessary and increments the usage count for the feature.
		/// The szProduct parameter specifies the product to match that has published the qualified
		/// component. If null, this API works the same as MsiProvideQualifiedComponent.
		///</summary>
		/// <param name="szCategory"> component category ID.</param>
		/// <param name="szQualifier">specifies which component to access.</param>
		/// <param name="dwInstallMode">either of type INSTALLMODE or a combination of the REINSTALLMODE flags.</param>
		/// <param name="szProduct"> the product code .</param>
		/// <param name="dwUnused1">not used, must be zero.</param>
		/// <param name="dwUnused2"> not used, must be zero.</param>
		/// <param name="lpPathBuf"> returned path, NULL if not desired.</param>
		/// <param name="pcchPathBuf">in/out buffer character count.</param>
		/// <returns></returns>
		/// <remarks>Please refer to the MSDN documentation for more information.</remarks>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "Msi.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10, Context = msiContext.InstallTime)]
		[DllImport("msi", CharSet = CharSet.Auto)]
		extern static public int MsiProvideAssembly(
			string szCategory, string  szQualifier,	uint dwInstallMode,
			string szProduct, uint dwUnused1, uint dwUnused2,
			string lpPathBuf,ref uint pcchPathBuf);



		/// <summary>
		/// The <c>MsiGetComponentPath</c> Return full path to an installed component
		///</summary>
		/// <param name="szProduct">product code in case install required.</param>
		/// <param name="szComponent">component Id, string GUID.</param>
		/// <param name="lpPathBuf">returned path.</param>
		/// <param name="pcchPathBuf">in/out buffer character count.</param>
		/// <returns></returns>
		/// <remarks>Please refer to the MSDN documentation for more information.</remarks>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "Msi.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10, Context = msiContext.InstallTime)]
		[DllImport("msi", CharSet = CharSet.Auto)]
		extern static public int MsiGetComponentPath(
			string szProduct, string szComponent,
			string lpPathBuf,ref uint pcchPathBuf);



		#endregion

		#region Patch

		[WinAPI(OS.None,FileName = "Msi.h",UpgradeVersion = OS.WinXP)]
		[Nativemsi(WindowsInstallerVersion.version30, Context = msiContext.InstallTime)]
		public enum MSIPATCHSTATE
        {
			/// <summary>
			///  broken
			/// </summary>
			INVALID     =  0, 
			/// <summary>
			///  applied patch
			/// </summary>
			APPLIED     =  1, 
			/// <summary>
			/// applied but superseded
			/// </summary>
			SUPERSEDED  =  2,   
			/// <summary>
			/// applied but obsoleted
			/// </summary>
			OBSOLETED   =  4,  
			/// <summary>
			/// registered only - Not applied
			/// </summary>
			REGISTERED  =  8,   
			/// <summary>
			/// 
			/// </summary>
			ALL         =  (APPLIED | SUPERSEDED | OBSOLETED | REGISTERED)
		} 

		[WinAPI(OS.None,FileName = "Msi.h",UpgradeVersion = OS.WinXP)]
		[Nativemsi(WindowsInstallerVersion.version30, Context = msiContext.InstallTime)]
		public enum MSIINSTALLCONTEXT
		{   			
			/// <summary>
			/// product visible to the current user
			/// </summary>
			FIRSTVISIBLE   =  0,   
			/// <summary>
			///  Invalid context for a product
			/// </summary>
			NONE           =  0,  
			/// <summary>
			/// user managed install context
			/// </summary>
			USERMANAGED    =  1,  
			/// <summary>
			///  user non-managed context
			/// </summary>
			USERUNMANAGED  =  2,  
			/// <summary>
			/// per-machine context
			/// </summary>
			MACHINE        =  4,  
			/// <summary>
			/// All contexts. OR of all valid values
			/// </summary>
			ALL            =  (USERMANAGED |USERUNMANAGED | MACHINE),	
			/// <summary>
			///  all user-managed contexts
			/// </summary>
			ALLUSERMANAGED =  8,  
		} 
		
		[WinAPI(OS.None,FileName = "Msi.h",UpgradeVersion = OS.WinXP)]
		[Nativemsi(WindowsInstallerVersion.version30, Context = msiContext.InstallTime)]
		public enum MSIPATCHDATATYPE
		{
			PATCHFILE = 0,
			XMLPATH   = 1,
			XMLBLOB   = 2,
		} 

	    [WinAPI(OS.None,FileName = "Msi.h",UpgradeVersion = OS.WinXP)]
		[Nativemsi(WindowsInstallerVersion.version30, Context = msiContext.InstallTime)]
		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
		public struct MSIPATCHSEQUENCEINFO
		{
		    public string szPatchData;
			public MSIPATCHDATATYPE ePatchDataType;
			public uint  dwOrder;	//DWORD
			public uint   uStatus;	//UINT
		} 
		
		[WinAPI(OS.None,FileName = "Msi.h",UpgradeVersion = OS.WinXP)]
		[Nativemsi(WindowsInstallerVersion.version30, Context = msiContext.InstallTime)]
		public enum MSISOURCETYPE : long
		{
			UNKNOWN = 0x00000000L,
			/// <summary>
			///  network source
			/// </summary>
			NETWORK = 0x00000001L, 
			/// <summary>
			///  URL source
			/// </summary>
			URL     = 0x00000002L, 
			/// <summary>
			///  media source
			/// </summary>
			MEDIA   = 0x00000004L 
		}
		
		[WinAPI(OS.None,FileName = "Msi.h",UpgradeVersion = OS.WinXP)]
		[Nativemsi(WindowsInstallerVersion.version30, Context = msiContext.InstallTime)]
		public enum MSICODE	 : long
		{
			/// <summary>
			///  product code provided
			/// </summary>
			PRODUCT = 0x00000000L, 
			/// <summary>
			///   patch code provided
			/// </summary>
			PATCH   = 0x40000000L  
		}



		/// <summary>For each product listed by the patch package as eligible to receive the patch, the MsiApplyPatch function invokes an installation and sets the PATCH property to the path of the patch package.</summary>
		/// <param name="patchPackage">A null-terminated string specifying the full path to the patch package. </param>
		/// <param name="installPackage">
		/// <para>If <c>installtype</c> is set to <see cref="MsiInstallType.NetworkImage"/>, this parameter is a null-terminated string that specifies a path to the product that is to be patched. The installer applies the patch to every eligible product listed in the patch package if <c>installPackage</c> is set to <c>null</c> and <c>installType</c> is set to <see cref="MsiInstallType.Default"/>.</para>
		/// <para>If <c>installtype</c> is <see cref="MsiInstallType.SingleInstance"/>, the installer applies the patch to the product specified by <c>installPackage</c>. In this case, other eligible products listed in the patch package are ignored and the <c>installPackage</c> parameter contains the null-terminated string representing the product code of the instance to patch. This type of installation requires the installer running Windows .NET Server 2003 family or Windows XP SP1.</para>
		/// </param>
		/// <param name="installType">
		/// <para>This parameter specifies the type of installation to patch.</para>
		/// <para><see cref="MsiInstallType.NetworkImage"/>  Specifies an administrative installation. In this case, <c>installPackage</c> must be set to a package path. A value of 1 for <see cref="MsiInstallType.NetworkImage"/> sets this for an administrative installation.</para>
		/// <para><see cref="MsiInstallType.Default"/>  Searches system for products to patch. In this case, szInstallPackage must be <c>null</c>.</para>
		/// <para><see cref="MsiInstallType.SingleInstance"/>  Patch the product specified by szInstallPackage. <c>installPackage</c> is the product code of the instance to patch. This type of installation requires the installer running Windows .NET Server 2003 family or Windows XP SP1.</para>
		/// </param>
		/// <param name="commandLine">A null-terminated string that specifies command line property settings.</param>
		/// <returns>
		/// <para><see cref="MsiError.Success"/>  The function succeeded.</para>
		/// <para><see cref="MsiError.PatchPackageOpenFailed"/>  Patch package could not be opened.</para>
		/// <para><see cref="MsiError.PatchPackageInvalid"/>  The patch package is invalid.</para>
		/// <para><see cref="MsiError.PatchPackageUnsupported"/>  The patch package is not unsupported.</para>
		/// <para>An error relating to an action, see <see cref="MsiError"/>.</para>
		/// </returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "Msi.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10, Context = msiContext.InstallTime)]
		[DllImport("msi", CharSet = CharSet.Auto)]
		extern static public int MsiApplyPatch(string patchPackage,
			string installPackage, INSTALLTYPE installType, string commandLine);

		/// <summary>The <c>MsiEnumPatches</c> function enumerates all of the patches that have been applied to a product. The function returns the patch code GUID for each patch that has been applied to the product and returns a list of transforms from each patch that apply to the product. Note that patches may have many transforms only some of which are applicable to a particular product. The list of transforms are returned in the same format as the value of the <c>TRANSFORMS</c> property.</summary>
		/// <param name="product">Specifies the product for which patches are to be enumerated.</param>
		/// <param name="index">Specifies the index of the patch to retrieve. This parameter should be zero for the first call to the <c>MsiEnumPatches</c> function and then incremented for subsequent calls. </param>
		/// <param name="patch">Pointer to a buffer that receives the patch's GUID. This argument is required.</param>
		/// <param name="transform">Pointer to a buffer that receives the list of transforms in the patch that are applicable to the product. This argument is required and cannot be <c>null</c>.</param>
		/// <param name="transformSize">Set to the number of characters copied to <c>transform</c> upon an unsuccessful return of the function. Not set for a successful return. On input, this is the full size of the buffer, including a space for a terminating <c>null</c> character. If the buffer passed in is too small, the count returned does not include the terminating <c>null</c> character.</param>
		/// <returns>
		/// <para><see cref="MsiError.Success"/>  The function succeeded.</para>
		/// <para><see cref="MsiError.BadConfiguration"/>  The configuration data is corrupt.</para>
		/// <para><see cref="MsiError.InvalidParameter"/>  One of the parameters was invalid.</para>
		/// <para><see cref="MsiError.MoreData"/>  A buffer is too small to hold the requested data.</para>
		/// <para><see cref="MsiError.NoMoreItems"/>  There are no clients to return.</para>
		/// </returns>
		/// <remarks>Please refer to the MSDN documentation for more information.</remarks>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "Msi.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10, Context = msiContext.InstallTime)]
		[DllImport("msi", CharSet = CharSet.Auto)]
		extern static public int MsiEnumPatches(string product, uint index,
			string patch, string transform, ref uint transformSize);

		/// <summary>The <c>MsiGetPatchInfo</c> function returns information about a patch.</summary>
		/// <param name="patch">Specifies the patch code for the patch package.</param>
		/// <param name="attribute">Specifies the attribute to be retrieved.  (See <see cref="MsiInstallerProperty.LocalPackage"/>)</param>
		/// <param name="value">Pointer to a buffer that receives the property value. This parameter can be <c>null</c>.</param>
		/// <param name="valueSize">Pointer to a variable that specifies the size, in characters, of the buffer pointed to by the <c>value</c> parameter. On input, this is the full size of the buffer, including a space for a terminating null character. If the buffer passed in is too small, the count returned does not include the terminating null character.</param>
		/// <returns>
		/// <para><see cref="MsiError.Success"/>  The function succeeded.</para>
		/// <para><see cref="MsiError.BadConfiguration"/>  The configuration data is corrupt.</para>
		/// <para><see cref="MsiError.InvalidParameter"/>  One of the parameters was invalid.</para>
		/// <para><see cref="MsiError.MoreData"/>  A buffer is too small to hold the requested data.</para>
		/// <para><see cref="MsiError.UnknownProduct"/>  The patch package is not installed.</para>
		/// <para><see cref="MsiError.UnknownProperty"/>  The property is unrecognized.</para>
		/// </returns>
		/// <remarks>Please refer to the MSDN documentation for more information.</remarks>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "Msi.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[DllImport("msi", CharSet = CharSet.Auto)]
		extern static public int MsiGetPatchInfo(string patch, string attribute,
			string value, ref uint valueSize);

		
		/// <summary>The <c>MsiVerifyPackage</c> function verifies that the given file is an installation package.</summary>
		/// <param name="path">Specifies the path and file name of the package.</param>
		/// <returns>
		/// <para><see cref="MsiError.Success"/>  The file is a package.</para>
		/// <para><see cref="MsiError.InvalidParameter"/>  One of the parameters was invalid.</para>
		/// <para><see cref="MsiError.PatchPackageInvalid"/>  The file is not a valid package.</para>
		/// <para><see cref="MsiError.PatchPackageOpenFailed"/>  The file could not be opened.</para>
		/// </returns>
		[WinAPI(OS.Win2k|OS.WinXP|OS.WinME|OS.Win2k3,
			 FileName = "Msi.h",UpgradeVersion = OS.Win98|OS.Win95)]
		[Nativemsi(WindowsInstallerVersion.version10, Context = msiContext.InstallTime)]
		[DllImport("msi" , CharSet = CharSet.Auto)]
		extern static public int MsiVerifyPackage(string path);

		/// <summary>The <c>MsiRemovePatches.</c> Remove one or more patches from the specified product..</summary>
		/// <param name="szPatchList">semi-colon delimited list of patches to remove; patch can be referenced by patch package path or Patch GUID</param>
		/// <param name="szProductCode">ProductCode GUID of product with patch to remove.</param>
		/// <param name="eUninstallType">type of patch uninstall to perform. Must be INSTALLTYPE_SINGLE_INSTANCE.</param>
		/// <param name="szPropertyList">command line property settings using PROPERTY=VALUE pairs.</param>
		/// <returns>
		/// </returns>
		[WinAPI(OS.None,FileName = "Msi.h",UpgradeVersion = OS.WinXP)]
		[Nativemsi(WindowsInstallerVersion.version30, Context = msiContext.InstallTime)]
		[DllImport("msi" , CharSet = CharSet.Auto)]
		extern static public int MsiRemovePatches(string szPatchList,string szProductCode,
			                                   INSTALLTYPE eUninstallType,string szPropertyList);

		/// <summary>The <c>MsiExtractPatchXMLData.</c> Extract XML data from the patch.</summary>
		/// <param name="szPatchPath">Patch file to open</param>
		/// <param name="dwReserved">Reserved.</param>
		/// <param name="szXMLData">Buffer that gets the XML data.</param>
		/// <param name="pcchXMLData">in/out XML data buffer character count.</param>
		/// <returns>
		/// </returns>
		[WinAPI(OS.None,FileName = "Msi.h",UpgradeVersion = OS.WinXP)]
		[Nativemsi(WindowsInstallerVersion.version30, Context = msiContext.InstallTime)]
		[DllImport("msi" , CharSet = CharSet.Auto)]
		extern static public int MsiExtractPatchXMLData(string szPatchPath,uint dwReserved,
			string szXMLData,ref uint pcchXMLData);


		/// <summary>The <c>MsiGetPatchInfoEx.</c> 
		/// Retrieve extended patch info for a particular patch applied
		/// to a particular product instance.
		///</summary>
		/// <param name="szPatchCode">target patch to query</param>
		/// <param name="szProductCode">target product of patch application.</param>
		/// <param name="szUserSid">Account of this product instance.</param>
		/// <param name="dwContext">context to query for product and patch.</param>
		/// <param name="szProperty">property of patch to retrieve.</param>
		/// <param name="lpValue">address buffer for data.</param>
		/// <param name="pcchValue">in/out value buffer character count.</param>
		/// <returns>
		/// </returns>
		[WinAPI(OS.None,FileName = "Msi.h",UpgradeVersion = OS.WinXP)]
		[Nativemsi(WindowsInstallerVersion.version30, Context = msiContext.InstallTime)]
		[DllImport("msi" , CharSet = CharSet.Auto)]
		extern static public int MsiGetPatchInfoEx(string szPatchCode,string szProductCode,
			string  szUserSid, MSIINSTALLCONTEXT dwContext,string szProperty,string lpValue,
		    ref uint pcchValue);
    
		/// <summary>The <c>MsiApplyMultiplePatches.</c> 
		/// Apply multiple patches to the specified product or to all eligible products
		///the machine
		///</summary>
		/// <param name="szPatchPackages">Patches to apply</param>
		/// <param name="szProductCode">target product of patch application.</param>
		/// <param name="szPropertiesList">Properties settings.</param>
		/// <returns>
		/// </returns>
		[WinAPI(OS.None,FileName = "Msi.h",UpgradeVersion = OS.WinXP)]
		[Nativemsi(WindowsInstallerVersion.version30, Context = msiContext.InstallTime)]
		[DllImport("msi" , CharSet = CharSet.Auto)]
		extern static public int MsiApplyMultiplePatches(string szPatchPackages,string szProductCode,
			string  szPropertiesList);

		/// <summary>The <c>MsiDeterminePatchSequence.</c> 
		/// Apply multiple patches to the specified product or to all eligible products
		///the machine
		///</summary>
		/// <param name="szProductCode">Product code GUID of an installed product</param>
		/// <param name="szUserSid">User account we're interested in.</param>
		/// <param name="dwContext">Installation context we're interested in.</param>
		/// <param name="cPatchInfo">Number of patches in the array.</param>
		/// <param name="pPatchInfo">Array of patch sequence information data.</param>
		/// <returns>
		/// </returns>
		[WinAPI(OS.None,FileName = "Msi.h",UpgradeVersion = OS.WinXP)]
		[Nativemsi(WindowsInstallerVersion.version30, Context = msiContext.InstallTime)]
		[DllImport("msi" , CharSet = CharSet.Auto)]
		extern static public int MsiDeterminePatchSequence(string szProductCode,string szUserSid,
			MSIINSTALLCONTEXT dwContext, uint cPatchInfo, ref MSIPATCHSEQUENCEINFO pPatchInfo);

		/// <summary>The <c>MsiEnumPatchesEx.</c> 
		/// Enumerates all patches applied for a single product or all products based upon
		/// enumeration context and filter
		///</summary>
		/// <param name="szProductCode">Enumerate patches on instances of this product</param>
		/// <param name="szUserSid">Account for enumeration.</param>
		/// <param name="dwContext">Contexts for enumeration.</param>
		/// <param name="dwFilter">Filter for enumeration.</param>
		/// <param name="dwIndex">Index for enumeration.</param>
		/// <param name="szInstalledProductCode">Enumerated patch code.</param>
		/// <param name="szTargetProductCode">Enumerated patch's product code.</param>
		/// <param name="pdwTargetProductContext">Enumerated patch's context.</param>
		/// <param name="szTargetUserSid">Enumerated patch's user account.</param>
		/// <param name="pcchTargetUserSid">in/out character count of szTargetUserSid.</param>
		/// <returns>
		/// </returns>
		[WinAPI(OS.None,FileName = "Msi.h",UpgradeVersion = OS.WinXP)]
		[Nativemsi(WindowsInstallerVersion.version30, Context = msiContext.InstallTime)]
		[DllImport("msi" , CharSet = CharSet.Auto)]
		extern static public int MsiEnumPatchesEx(string szProductCode,string szUserSid,
			uint dwContext, uint dwFilter,uint dwIndex,
			[MarshalAs(UnmanagedType.LPArray, SizeConst=39)] byte[] szPatchCode,
			[MarshalAs(UnmanagedType.LPArray, SizeConst=39)] byte[] szTargetProductCode,
			ref MSIINSTALLCONTEXT  pdwTargetProductContext,
			string szTargetUserSid,ref uint pcchTargetUserSid);




		#endregion
	}
}
