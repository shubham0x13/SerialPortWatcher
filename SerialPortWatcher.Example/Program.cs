namespace SerialPortWatcher.Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SerialPortEventWatcher serialPortEventWatcher = new SerialPortEventWatcher();
            serialPortEventWatcher.PortConnected += SerialPortEventWatcher_PortConnected;
            serialPortEventWatcher.PortDisconnected += SerialPortEventWatcher_PortDisconnected;
            serialPortEventWatcher.Start();

            Console.WriteLine("Watching Serial Port Connect/Disconnect Events.");
            Console.ReadKey();

            serialPortEventWatcher.Stop();
            serialPortEventWatcher.Dispose();
        }

        private static void SerialPortEventWatcher_PortConnected(object? sender, SerialPortWatcherEventArgs e)
        {
            Console.WriteLine($"Serial Port Connected : {e.PortName}.");
        }
        private static void SerialPortEventWatcher_PortDisconnected(object? sender, SerialPortWatcherEventArgs e)
        {
            Console.WriteLine($"Serial Port Disconnected : {e.PortName}.");
        }
    }
}
