using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
namespace Selenium.Utilities
{
    /// <summary>
    /// Contains methods to create a listener for the ActivClient login window and automatically input a pin.
    /// </summary>
    public class CacLogin
    {
        private static SecureString pin = null;
        /// <summary>
        /// Sets up a listener for the ActivClient login window that will automatically input the specified pin.
        /// </summary>
        /// <param name="pin">A <see cref="SecureString"/> cotaining the CAC's pin number.</param>
        public CacLogin(SecureString pin)
        {
            CacLogin.pin = pin;
            Automation.AddAutomationEventHandler(WindowPattern.WindowOpenedEvent, AutomationElement.RootElement, TreeScope.Children, OnWindowOpened);
        }
        /// <summary>
        /// A listener function that will input the CAC's pin into the ActivClient login window.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="automationEventArgs">Event arguments.</param>
        private static void OnWindowOpened(Object sender, AutomationEventArgs automationEventArgs)
        {
            try
            {
                AutomationElement element = sender as AutomationElement;
                if (element != null && element.Current.Name == "ActivClient Login" && pin != null)
                {
                    element.SetFocus();
                    IntPtr pinPtr = Marshal.SecureStringToGlobalAllocUnicode(pin);
                    SendKeys.SendWait(Marshal.PtrToStringUni(pinPtr));
                    Marshal.ZeroFreeGlobalAllocUnicode(pinPtr);
                    SendKeys.SendWait("{ENTER}");
                }
            }
            catch (ElementNotAvailableException) { }
        }
    }
}
