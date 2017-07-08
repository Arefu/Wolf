using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Embargo
{
    public enum InjectionStatus
    {
        DllNotFound,
        GameProcessNotFound,
        InjectionFailed,
        Success
    }

    public static class Injector
    {
        private static readonly IntPtr StaticZero = (IntPtr) 0;

        public static InjectionStatus Inject(string ProcName, string DllPath)
        {
            if (!File.Exists(DllPath))
                return InjectionStatus.DllNotFound;

            var Proc = (from Process in Process.GetProcesses() where Process.ProcessName == ProcName select Process.Id).FirstOrDefault();

            if (Proc == 0)
                return InjectionStatus.GameProcessNotFound;

            return !InjectNow(Proc, DllPath) ? InjectionStatus.InjectionFailed : InjectionStatus.Success;
        }

        private static bool InjectNow(int ProcToBeInjected, string Dll)
        {
            var Window = NaitiveMethods.OpenProcess(0x001F0FFF, 1, (uint) ProcToBeInjected);
            if (Window == StaticZero)
                return false;

            var LoadAddress = NaitiveMethods.GetProcAddress(NaitiveMethods.GetModuleHandle("kernel32.dll"),
                "LoadLibraryA");
            if (LoadAddress == StaticZero)
                return false;

            var Address = NaitiveMethods.VirtualAllocEx(Window, (IntPtr) null, (IntPtr) Dll.Length, 0x1000 | 0x2000,
                0X40);
            if (Address == StaticZero)
                return false;

            var BytesToInject = Encoding.ASCII.GetBytes(Dll);

            if (NaitiveMethods.WriteProcessMemory(Window, Address, BytesToInject, (uint) BytesToInject.Length, 0) == 0)
                return false;

            if (NaitiveMethods.CreateRemoteThread(Window, (IntPtr) null, StaticZero, LoadAddress, Address, 0,
                    (IntPtr) null) == StaticZero)
                return false;

            NaitiveMethods.CloseHandle(Window);
            return true;
        }
    }
}