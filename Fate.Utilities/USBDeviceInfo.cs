namespace Fate.Utilities
{
    public class USBDeviceInfo
    {
        public USBDeviceInfo(string deviceId, string description)
        {
            DeviceId = deviceId;
            Description = description;
        }

        public string DeviceId { get; private set; }

        public string Description { get; private set; }
    }
}
