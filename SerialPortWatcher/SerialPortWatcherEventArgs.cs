using System;
using System.Management;
using System.Text.RegularExpressions;

namespace SerialPortWatcher
{
    /// <summary>
    /// Event arguments for serial port watcher events.
    /// </summary>
    public class SerialPortWatcherEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the Device ID of the device.
        /// </summary>
        public string? DeviceID { get; }

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        public string? DeviceName { get; }

        /// <summary>
        /// Gets the name of the COM port.
        /// </summary>
        public string? PortName { get; }

        /// <summary>
        /// Gets the description of the device.
        /// </summary>
        public string? Description { get; }

        /// <summary>
        /// Gets the class GUID of the device.
        /// </summary>
        public string? ClassGuid { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialPortWatcherEventArgs"/> class.
        /// </summary>
        /// <param name="eventArgs">The event arguments containing WMI data.</param>
        internal SerialPortWatcherEventArgs(EventArrivedEventArgs eventArgs)
        {
            ManagementBaseObject targetInstance = (ManagementBaseObject)eventArgs.NewEvent.GetPropertyValue("TargetInstance");

            DeviceID = targetInstance.GetPropertyValue("DeviceID").ToString();
            DeviceName = targetInstance.GetPropertyValue("Name").ToString();
            if (!string.IsNullOrEmpty(DeviceName))
            {
                PortName = ExtractPortNameFromDeviceName(DeviceName);
            }
            Description = targetInstance.GetPropertyValue("Description").ToString();
            ClassGuid = targetInstance.GetPropertyValue("ClassGuid").ToString();
        }

        /// <summary>
        /// Extracts the COM port name from the device name using regex.
        /// </summary>
        /// <param name="deviceName">The device name string from which to extract the port name.</param>
        /// <returns>The extracted COM port name, or <see langword="null"/> if not found.</returns>
        private string? ExtractPortNameFromDeviceName(string deviceName)
        {
            if (deviceName != null)
            {
                Match match = Regex.Match(deviceName, @"COM(\d+)");
                if (match.Success)
                {
                    return match.Value;
                }
            }

            return null;
        }
    }
}
