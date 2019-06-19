using System.Collections.Generic;
using System.Management;

namespace Fate.Utilities
{
    public static class USBInterface
    {
        public static List<USBDeviceInfo> GetDevices()
        {
            ManagementObjectCollection collection;
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_PnPEntity"))
            {
                collection = searcher.Get();
            }

            var devices = new List<USBDeviceInfo>();

            foreach (var device in collection)
            {
                devices.Add(
                    new USBDeviceInfo(
                        (string)device.GetPropertyValue("DeviceID"),
                        (string)device.GetPropertyValue("Description")));
            }

            collection.Dispose();
            return devices;
        }
    }
}
