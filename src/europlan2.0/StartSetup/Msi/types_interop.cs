//This file contains attributes and types that are basic to Interop implemented in .NET
using System;


namespace Youseful.Types.Interop
{
	[Flags]
	public enum OS
	{
		None    = 0x0000,
		Win95	= 0x0001,
		Win98	= 0x0002,
		Win98SE = 0x0004,
		WinME   = 0x0008,
		WinCE   = 0x0010,
		WinNT	= 0x0020,
		WinXP   = 0x0040,
		Win2k	= 0x0080,
		Win2k3  = 0x0100,
	}

	public enum XPEdition
	{
		Home,
		Professional,
		NA
	}
	
	public enum Win2kEdition
	{
		//Standard,
		Professional,
		Server
	}
	

	[AttributeUsage(AttributeTargets.All,AllowMultiple = false,
		 Inherited = false)]
	public class WinAPIAttribute : System.Attribute
	{
		private OS _Version;
		private XPEdition _XPEdition;
		private string _FileName;
		private OS _UpgradeVersion;

		public WinAPIAttribute(OS version)
		{
			this._Version = version;
			this._XPEdition = XPEdition.NA;
		}
		
		public OS UpgradeVersion
		{
			get
			{ return _UpgradeVersion;}
			set	
			{ _UpgradeVersion = value;}
		}

		public string FileName
		{
			get { return _FileName;}
			set { _FileName = value;}
		}	

		
		public OS Version
		{
			get
			{ return _Version;}
			//set	{ _Version = value;}
		}
		
		public XPEdition XP_Edition
		{
			get { return _XPEdition;}
			set { _XPEdition = value;}
		}



	}




}