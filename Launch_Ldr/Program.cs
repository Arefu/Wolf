using Celtic_Guardian;
using System;
using System.Diagnostics;
using System.IO;
using System.Web.Script.Serialization;

namespace Launch_Ldr
{
    class Program
    {
        static void Main()
        {
            //Start game THROUGH STEAM!
            Process.Start("steam://run/480650");
            while (Process.GetProcessesByName("YuGiOh.exe") == null) //Wait For Game Launch
            { }

            Utilities.Log("Game Launched. Looking For Plugins.", Utilities.Event.Information);

            foreach (var Plugin in Directory.GetFiles("Plugins/", "*.dll"))
            {
                if (File.Exists($"{Plugin.Replace(".dll",string.Empty)}_meta.json"))
                {
                    Utilities.Log($"Found Plugin: {Plugin}, Reading Meta.", Utilities.Event.Information);
                    var MetaData = new JavaScriptSerializer().Deserialize<Meta>(File.ReadAllText(Plugin.Replace(".dll", string.Empty) + "_meta.json"));
                    Utilities.Log($"Name: {MetaData.Name}", Utilities.Event.Meta);
                    Utilities.Log($"Description: {MetaData.Description}", Utilities.Event.Meta);
                    foreach(var Address in MetaData.Injection_Addresses)
                        Utilities.Log($"Injection Address: {Address}",Utilities.Event.Meta);
                    foreach (var Dependancy in MetaData.Depends)
                    {
                        Utilities.Log($"Depends On: {Dependancy}", Utilities.Event.Meta);
                    }
                }
                else
                {
                    Utilities.Log($"No {Plugin}_meta.json Found, Loading Anyway.", Utilities.Event.Warning);
                }
            }
            Console.WriteLine("Game Launched!");
            Console.ReadLine();
        }
    }
}
