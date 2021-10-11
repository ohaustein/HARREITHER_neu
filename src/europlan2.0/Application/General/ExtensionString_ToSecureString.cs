using System;
using System.Security;

public static partial class ExtensionString
{

    public static SecureString ToSecureString(this String text)
    {
        if (text == null) throw new ArgumentNullException("text");
        var secureString = new SecureString();
        foreach (var character in text)
        {
            secureString.AppendChar(character);
        }
        return secureString;
    }

}
