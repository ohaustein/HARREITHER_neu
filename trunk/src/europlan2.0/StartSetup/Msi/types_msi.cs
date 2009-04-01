using System;
using Youseful.Types.Interop;

namespace Youseful.Installer.WI
{
		
		#region Enums

		[Flags]
		public enum Dialog_Style : int
		{
			Empty               = 0x00000000,
			Visible				= 0x00000001,
			Modal				= 0x00000002,
			Minimize			= 0x00000004,
			SysModal			= 0x00000008,
			KeepModeless		= 0x00000010,
			TrackDiskSpace		= 0x00000020,
			UseCustomPalette	= 0x00000040,
			RTLRO				= 0x00000080,
			RightAligned		= 0x00000100,
			LeftScroll			= 0x00000200,
			BiDi				= RTLRO | RightAligned | LeftScroll,
			Error				= 0x00010000,
		}

		[Flags]
		public enum File_Attributes : int
		{
			Empty				=	0,
			ReadOnly			=	1,
			Hidden				=	2,
			System				=	4,
			Vital				=	512,
			Checksum			=	1024,
			PatchAdded			=	4096,
			NonCompressed		=	8192,
			Compressed			=	16384
		}

		[Flags]
		public enum Component_Attributes : int
		{
			LocalOnly			= 0,
			SourceOnly			= 1,
			Optional			= 2,
			KeyPath				= 4,
			SharedDllRefCount	= 8,
			Permanent			= 16,
			ODBCDataSource		= 32,
			Transitive			= 64,
			NeverOverwrite		= 128
      
		}

		//[Flags]
		public enum INSTALLFEATUREATTRIBUTE // bit flags
		{
			FAVORLOCAL             = 1 << 0,
			FAVORSOURCE            = 1 << 1,
			FOLLOWPARENT           = 1 << 2,
			FAVORADVERTISE         = 1 << 3,
			DISALLOWADVERTISE      = 1 << 4,
			NOUNSUPPORTEDADVERTISE = 1 << 5,
			EMPTY				   = 0,
		};

		/// <summary>
		/// Enumeration of existing versions of Windows Installer
		/// </summary>
		public enum WindowsInstallerVersion
		{
			version10,
			version11,
			version15,
			version20,
			version30
		}
	
		/// <summary>
		/// Determines the kind of #install classes/functions 
		/// that are permissible to call in a given context
		/// </summary>
		[Flags]
	public enum msiContext
		{
			/// <summary>
			/// Building the Windows Installer Database file
			/// </summary>
			DesignTime  = 0x0001,
			/// <summary>
			/// Querying the state of the machine(not during an install)
			/// </summary>
			RunTime     = 0x0002,
			/// <summary>
			/// When an install actually runs.
			/// </summary>
			InstallTime = 0x004,
			All         = DesignTime | RunTime | InstallTime
		}

		#endregion
}
