# SerialPortWatcher

[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

Detects serial port connection and disconnection events in Windows using WMI (Windows Management Instrumentation). This library monitors serial port availability in real-time, enabling dynamic response to new port connections and disconnections.

## Installation
Install the NuGet package from [here](https://www.nuget.org/packages/SerialPortWatcher).

## Usage/Examples

Here's an example of how to use `SerialPortWatcher` in your application:

```cs
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
```
