using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Security.AccessControl;

namespace NoUsbStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            string user = Environment.UserDomainName + "\\" + Environment.UserName;
            RegistrySecurity rs = new RegistrySecurity();

            // Allow the current user to CreateSubKey
            //
            rs.AddAccessRule(new RegistryAccessRule(user,
                RegistryRights.CreateSubKey | RegistryRights.ReadKey,
                InheritanceFlags.None,
                PropagationFlags.None,
                AccessControlType.Allow));

            if (args.Length == 1 && args[0] == "trustyou")
            {
                REG_EnableRegedit();
                USB_enableAllStorageDevices();
                return;
            }
            REG_DisableRegedit();
            USB_disableAllStorageDevices();
        }

        private static void REG_DisableRegedit()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey
                ("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);
            key.SetValue("DisableRegistryTools", 1, RegistryValueKind.DWord);
            key.Close();
        }
        private static void USB_disableAllStorageDevices()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey
                ("SYSTEM\\CurrentControlSet\\Services\\UsbStor", true);
            if (key != null)
            {
                key.SetValue("Start", 4, RegistryValueKind.DWord);
            }
            key.Close();
        }

        private static void REG_EnableRegedit()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey
                ("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);
            key.SetValue("DisableRegistryTools", 0, RegistryValueKind.DWord);
            key.Close();
        }
        private static void USB_enableAllStorageDevices()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey
                ("SYSTEM\\CurrentControlSet\\Services\\UsbStor", true);
            if (key != null)
            {
                key.SetValue("Start", 3, RegistryValueKind.DWord);
            }
            key.Close();
        }
    }
}
