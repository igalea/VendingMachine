using System;
using System.Runtime.InteropServices;
using System.Security;

namespace VendingMachine.Auxilliary
{
    public static class Extensions
        {
             public static string ConvertToUnsecureString(this SecureString securePassword)
            {
                if (securePassword == null)
                    return string.Empty;

                IntPtr unmanagedString = IntPtr.Zero;
                try
                {
                    unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                    return Marshal.PtrToStringUni(unmanagedString);
                }
                finally
                {
                    Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
                }
            }
        }
}
