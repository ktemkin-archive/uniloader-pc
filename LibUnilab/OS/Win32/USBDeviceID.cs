using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using LibUsbDotNet;
using LibUsbDotNet.Main;
using System.Management;

namespace Unilab.OS.Win32
{
    public class USBDeviceID
    {
        /// <summary>
        ///     The Vendor ID associated with the USB device.
        /// </summary>
        public readonly int VendorID;

        /// <summary>
        ///     The Product ID associated with the USB device.
        /// </summary>
        public readonly int ProductID;

        /// <summary>
        ///     The revision number associated with the USB device.
        /// </summary>
        public readonly int Revision;
        
        /// <summary>
        ///     Creates a new USB Device ID from a WMI device ID string.
        /// </summary>
        /// <param name="wmiDeviceID"></param>
        public USBDeviceID(string wmiDeviceID)
        {
            Regex wmiPattern = new Regex("USB\\\\VID_([0-9a-fA-F]+)&PID_([0-9a-fA-F]+)\\\\[0-9a-fA-F]+&[0-9a-fA-F]+&[0-9a-fA-F]+&([0-9a-fA-F]+)", RegexOptions.IgnoreCase);
            Match match = wmiPattern.Match(wmiDeviceID);

            //Handle failed matches
            if (!match.Success)
                throw new ArgumentException("Pattern did not match a standard WMI USB device string. (Check for the correct device first?)");

            VendorID = Convert.ToInt32(match.Groups[1].Value, 16);
            ProductID = Convert.ToInt32(match.Groups[2].Value, 16);

            //[PPL] FIXME research firmware revision style
            //This may have to be changed if my assumptions about the style of Firmware IDs are wrong.
            Revision = int.Parse(GetDeviceFirmwareRevision(wmiDeviceID).Replace(".", ""));

            //FIXME NRE happened after device unplug replug @ above line
        }

        internal static string GetDeviceFirmwareRevision(string deviceID)
        {
            //Attempt to get a WMI object that knows the device's firmware ID
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"\\.\root\WMI", "SELECT * FROM MSDeviceUI_FirmwareRevision where InstanceName = '" + deviceID.Replace(@"\", @"\\") + "_0'");


            //and attempt to extract that firmware ID from the device
            foreach (var device in searcher.Get())
            {
                try
                {
                    return device.GetPropertyValue("FirmwareRevision") as string;
                }
                catch (ManagementException)
                {
                    continue;
                }
                
            }

            //if we didn't find the firmware revision, return -1
            return "-1";
        }

        

        

    }
}
