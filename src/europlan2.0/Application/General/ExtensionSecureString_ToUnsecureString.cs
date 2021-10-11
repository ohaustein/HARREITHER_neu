using System;
using System.Security;

public static class ExtensionSecureString
{


    public static String ToUnsecureString(this SecureString secureString)
    {
        if (secureString == null) throw new ArgumentNullException("secureString");

        var unmanagedString = IntPtr.Zero;
        try
        {
            unmanagedString = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(secureString);
            return System.Runtime.InteropServices.Marshal.PtrToStringUni(unmanagedString);
        }
        finally
        {
            System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
        }
    }


}
