using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace USB_Watcher
{
    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("Listening for devices...");

            //INSERT
            var insertWatcher = new ManagementEventWatcher();
            insertWatcher.EventArrived += (s, e) =>
            {
                var instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
                Console.WriteLine($"[+] Inserted: {instance["Name"]}");
            };
            insertWatcher.Query = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'");
            insertWatcher.Start();

            //REMOVE
            var removeWatcher = new ManagementEventWatcher();
            removeWatcher.EventArrived += (s, e) =>
            {
                var instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
                Console.WriteLine($"[-] Removed: {instance["Name"]}");
            };
            removeWatcher.Query = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'");
            removeWatcher.Start();

            Console.ReadLine(); //Keeps app running
            insertWatcher.Stop();
            removeWatcher.Stop();
        }

        public static void PrintDeviceInfo()
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_USBHub");
            foreach (ManagementObject device in searcher.Get())
            {
                Console.WriteLine("  Name: " + device["Name"]);
                //Console.WriteLine("  DeviceID: " + device["DeviceID"]);
            }
            Console.WriteLine();
        }
    }

}
