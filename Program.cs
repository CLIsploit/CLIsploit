// Copyright Plextora 2022
// This file is licensed under GPL-3.0-or-later

using CommandLine;
using CommandLine.Text;
using KrnlAPI;
using System.Collections.Generic;
using System;
using WeAreDevs_API;
using System.Diagnostics;
using System.IO;

namespace CLIsploit
{
    internal class Program
    {
        static KrnlApi _krnlApi = new KrnlApi();
        static ExploitAPI _wrdApi = new ExploitAPI();

        public class Options
        {
            [Option("api", Required = false, HelpText = "Specify which exploit API you want to use")]
            public string Api { get; set; }

            [Option("customapi", Required = false, HelpText = "Specify the path of a custom exploit API you want to use")]
            public string CustomApi { get; set; }

            [Option('p', "scriptpath", Required = false, HelpText = "Specify the path of the script you want to use")]
            public string ScriptPath { get; set; }
        }

        static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
        {
            var helpText = HelpText.AutoBuild(result, h =>
            {
                h.AdditionalNewLineAfterOption = false;
                h.Heading = "CLIsploit - Exploit in ROBLOX the chad way";
                h.Copyright =
                    "Copyright 2022 Plextora\n\n" +
                    "This program comes with ABSOLUTELY NO WARRANTY.\n" +
                    "This is free (as in freedom) software, and you are welcome to redistribute it under certain conditions.\n" +
                    "Visit <https://www.gnu.org/licenses/gpl-3.0.en.html> for more information.";
                return HelpText.DefaultParsingErrorsHandler(result, h);
            }, e => e);
            Console.WriteLine(helpText);
        }

        static void Main(string[] args)
        {
            _krnlApi.Initialize();

            var parser = new CommandLine.Parser(with => with.HelpWriter = null);
            var parserResult = parser.ParseArguments<Options>(args);
            parserResult
                .WithParsed<Options>(options => Run(options))
                .WithNotParsed(errs => DisplayHelp(parserResult, errs));
        }

        public static void Run(Options options)
        {
            if (options.Api != null)
            {
                // inject exploit api
                switch (options.Api)
                {
                    case "krnl":
                        KrnlInject();
                        break;
                    case "wrd":
                        WrdInject();
                        break;
                }
            }
            else if (options.ScriptPath != null)
            {
                string script = File.ReadAllText(options.ScriptPath);

                if (_krnlApi.IsInjected())
                {
                    _krnlApi.Execute(script);
                    Console.WriteLine("Executed script with Krnl API!");
                }
                else if (_wrdApi.isAPIAttached())
                {
                    _wrdApi.SendLuaScript(script);
                    Console.WriteLine("Executed script with WRD API!");
                }
            }
        }

        public static void KrnlInject()
        {
            // if statement hell

            if (!IsRobloxRunning())
            {
                Console.WriteLine("ROBLOX isn't running! Please open ROBLOX and try again!");
            }

            if (_krnlApi.IsInjected())
            {
                Console.WriteLine("Krnl is already injected! Execute some scripts!");
            }

            if (IsRobloxRunning())
            {
                if (_krnlApi.IsInitialized())
                {
                    if (!_krnlApi.IsInjected())
                    {
                        _krnlApi.Inject();
                        Console.WriteLine("Injecting Krnl API...");
                    }
                }
            }
        }

        public static void WrdInject()
        {
            // if statement hell

            if (!IsRobloxRunning())
            {
                Console.WriteLine("ROBLOX isn't running! Please open ROBLOX and try again!");
            }

            if (_wrdApi.isAPIAttached())
            {
                Console.WriteLine("WRD is already injected! Execute some scripts!");
            }

            if (IsRobloxRunning())
            {
                if (!_wrdApi.isAPIAttached())
                {
                    _wrdApi.LaunchExploit();
                    Console.WriteLine("Injecting WRD API...");
                }
            }
        }

        public static bool IsRobloxRunning()
        {
            Process[] name = Process.GetProcessesByName("RobloxPlayerBeta");

            if (name.Length == 0)
            {
                return false;
            }

            return true;
        }
    }
}
