using System;
using System.Linq;
using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VendingMachine.UnitTest.Helpers
{
    
    public class AssertException
    {
        public static void NoExceptionThrown<T>(Action actionToRun) where T : Exception
        {
            try
            {
                actionToRun();
            }
            catch (T)
            {
                Assert.Fail("Expected no {0} to be thrown", typeof(T).Name);
            }
        }

        public static void ExceptionThrown<T>(Action actionToRun) where T : Exception
        {
            try
            {
                actionToRun();
                
            }
            catch (T ex)
            {
                Assert.AreSame(ex.GetType().Name, typeof(T).Name);
            }
        }

    }

    public static class UnitTestHelper
    {
        public static SecureString FetchSecureString(string passwordString)
        {
            var secureString = new SecureString();
            Array.ForEach(passwordString.ToArray(), secureString.AppendChar);
            secureString.MakeReadOnly();
            return secureString;
        }
    }

    
}

