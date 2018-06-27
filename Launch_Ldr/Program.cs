using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Script.Serialization;
using Celtic_Guardian;

namespace Launch_Ldr
{
    internal class Program
    {
        private static void Main(string[] Args)
        {
            var RequireMeta = Args.Any(Arg => Arg.ToLower() == "-agro");

            Utilities.Log("Looking For Plugins.", Utilities.Event.Information);
            var Plugins = new List<string>();

            foreach (var Plugin in Directory.GetFiles("Plugins/", "*.dll"))
            {
                if (File.Exists($"{Plugin.ToLower().Replace(".dll", string.Empty)}_info.json"))
                {
                    Utilities.Log($"Found Plugin: {Plugin}, Reading Plugin Information.", Utilities.Event.Information);
                    var MetaData =
                        new JavaScriptSerializer().Deserialize<Meta>(
                            File.ReadAllText(Plugin.Replace(".dll", string.Empty) + "_info.json"));
                    Utilities.Log($"Name: {MetaData.Name}", Utilities.Event.Meta);
                    Utilities.Log($"Description: {MetaData.Description}", Utilities.Event.Meta);
                    foreach (var Address in MetaData.Injection_Addresses)
                        Utilities.Log($"Injection Address: {Address}", Utilities.Event.Meta);
                    foreach (var Dependancy in MetaData.Depends)
                    {
                        Utilities.Log($"Depends On: {Dependancy}", Utilities.Event.Meta);
                        if (!File.Exists($"Plugins/{Dependancy}") && Dependancy.ToLower() != "nothing")
                            Utilities.Log($"I Can't Find {Dependancy}!", Utilities.Event.Error);
                    }
                }

                if (!RequireMeta)
                {
                    if (!File.Exists(Plugin.ToLower().Replace(".dll", string.Empty) + "_info.json")) Utilities.Log($"No {Plugin.ToLower().Replace(".dll", string.Empty)}_info.json Found, Loading Anyway.", Utilities.Event.Warning);
                    Plugins.Add(new FileInfo(Plugin).FullName);
                }
                else
                {
                    if (!File.Exists(Plugin.ToLower().Replace(".dll", string.Empty) + "_info.json")) Utilities.Log($"No {Plugin.ToLower().Replace(".dll", string.Empty)}_info.json Found, Agro Was Specified. I Won't Load This.", Utilities.Event.Error);
                }
            }

            Process.Start("steam://run/480650");
            Thread.Sleep(TimeSpan.FromSeconds(2.5));

            foreach (var Plugin in Plugins)
            {
                var Result = Injector.Inject("YuGiOh", Plugin);
                Utilities.Log($"{new FileInfo(Plugin).Name}'s Injection Status: {Result}", Utilities.Event.Information);
            }

            var InputCommand = "";
            do
            {
                Console.Write("Yu-Gi-Oh: ");
                InputCommand = Console.ReadLine().ToLower();

                switch (InputCommand.ToLower())
                {
                    case "set":
                        Console.Write("Address / Variable: ");
                        //Set Address Get from List of JSON Address -> Variable File
                        Console.Write("Value: ");
                        //Set Value
                        break;
                    case "get":
                        Console.Write("Address / Variable: ");
                        var Variable = "";
                        switch (Variable.ToLower())
                        {
                            case "p_duel_points":
                                //Call Naitive Func To Return Amount Of Points.
                                break;
                        }

                        break;
                }
            } while (InputCommand.ToLower() != "quit");
        }
    }
}