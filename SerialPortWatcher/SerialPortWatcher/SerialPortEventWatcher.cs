using System;
using System.Management;

namespace SerialPortWatcher
{
    /// <summary>
    /// Monitors serial port connection and disconnection events using WMI (Windows Management Instrumentation).
    /// </summary>
    public class SerialPortEventWatcher : IDisposable
    {
        private ManagementEventWatcher _connectWatcher;
        private ManagementEventWatcher _disconnectWatcher;
        private bool _disposed;

        /// <summary>
        /// Event raised when a serial port is connected. Provides the details of the serial port that was connected.
        /// </summary>
        public event EventHandler<SerialPortWatcherEventArgs>? PortConnected;

        /// <summary>
        /// Event raised when a serial port is disconnected. Provides the details of the serial port that was disconnected.
        /// </summary>
        public event EventHandler<SerialPortWatcherEventArgs>? PortDisconnected;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialPortEventWatcher"/> class.
        /// <param name="pollingInterval">The polling interval, in seconds, to check for serial port events. Must be greater than 0.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the polling interval is less than or equal to 0.</exception>
        /// </summary>
        public SerialPortEventWatcher(int pollingInterval = 1)
        {
            if (pollingInterval <= 0)
            {
                throw new ArgumentOutOfRangeException("Polling interval must be greater than 0.");
            }

            string connectQuery = $"SELECT * FROM __InstanceCreationEvent WITHIN {pollingInterval} WHERE TargetInstance ISA 'Win32_PnPEntity' AND TargetInstance.Name LIKE '%(COM%'";
            _connectWatcher = CreateWatcher(connectQuery, OnComPortConnected);

            string disconnectQuery = $"SELECT * FROM __InstanceDeletionEvent  WITHIN {pollingInterval} WHERE TargetInstance ISA 'Win32_PnPEntity' AND TargetInstance.Name LIKE '%(COM%'";
            _disconnectWatcher = CreateWatcher(disconnectQuery, OnComPortDisconnected);
        }

        private ManagementEventWatcher CreateWatcher(string query, EventArrivedEventHandler handler)
        {
            ManagementEventWatcher watcher = new ManagementEventWatcher(new WqlEventQuery(query));
            watcher.EventArrived += handler;
            return watcher;
        }

        /// <summary>
        /// Starts monitoring serial port events.
        /// </summary>
        public void Start()
        {
            _connectWatcher.Start();
            _disconnectWatcher.Start();
        }

        /// <summary>
        /// Stops monitoring serial port events.
        /// </summary>
        public void Stop()
        {
            _connectWatcher.Stop();
            _disconnectWatcher.Stop();
        }

        private void OnComPortConnected(object sender, EventArrivedEventArgs e)
        {
            PortConnected?.Invoke(this, new SerialPortWatcherEventArgs(e));
        }

        private void OnComPortDisconnected(object sender, EventArrivedEventArgs e)
        {
            PortDisconnected?.Invoke(this, new SerialPortWatcherEventArgs(e));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _connectWatcher?.Dispose();
                    _disconnectWatcher?.Dispose();
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="SerialPortEventWatcher"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
