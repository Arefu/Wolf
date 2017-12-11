using Celtic_Guardian;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Web.Script.Serialization;

namespace Launch_Ldr
{
    class Program
    {
        static void Main()
        {
            Utilities.Log("Looking For Plugins.", Utilities.Event.Information);
            var Plugins = new List<string>();

            foreach (var Plugin in Directory.GetFiles("Plugins/", "*.dll"))
            {
                if (File.Exists($"{Plugin.ToLower().Replace(".dll", string.Empty)}_info.json"))
                {
                    Utilities.Log($"Found Plugin: {Plugin}, Reading Plugin Information.", Utilities.Event.Information);
                    var MetaData = new JavaScriptSerializer().Deserialize<Meta>(File.ReadAllText(Plugin.Replace(".dll", string.Empty) + "_info.json"));
                    Utilities.Log($"Name: {MetaData.Name}", Utilities.Event.Meta);
                    Utilities.Log($"Description: {MetaData.Description}", Utilities.Event.Meta);
                    foreach (var Address in MetaData.Injection_Addresses)
                        Utilities.Log($"Injection Address: {Address}", Utilities.Event.Meta);
                    foreach (var Dependancy in MetaData.Depends)
                    {
                        Utilities.Log($"Depends On: {Dependancy}", Utilities.Event.Meta);
                    }
                }
                else
                {
                    Utilities.Log($"No {Plugin.ToLower().Replace(".dll", string.Empty)}_info.json Found, Loading Anyway.", Utilities.Event.Warning);
                }
                Plugins.Add(new FileInfo(Plugin).FullName);
            }
            Process.Start("steam://run/480650");
            Thread.Sleep(TimeSpan.FromSeconds(2.5));

            foreach (var Plugin in Plugins)
            {
                var Result = Injector.Inject("YuGiOh", Plugin);
                Utilities.Log($"{new FileInfo(Plugin).Name}'s Injection Status: {Result}", Utilities.Event.Information);
            }
            Console.ReadLine();
        }
    }
}
