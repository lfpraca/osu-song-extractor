using System;
using System.Collections.Generic;
using System.Configuration;

namespace osu__song_extractor
{
    class Program
    {
        static void Main(string[] args)
        {
            #region arrays
            string[] blacklist =
            {
                "applause",
                "menuhit",
                "pause-loop",
                "spinnerspin",
                "back-button-click",
                "back-button-hover",
                "click-close",
                "click-short",
                "drum-hitfinish",
                "drum-hitclap",
                "click-short-confirm",
                "drum-hitnormal",
                "drum-hitwhistle",
                "menu-back-hover",
                "menuback",
                "menu-charts-click",
                "menu-direct-click",
                "menu-back-click",
                "menu-charts-hover",
                "menu-options-click",
                "menu-edit-hover",
                "menu-direct-hover",
                "menu-options-hover",
                "menu-exit-click",
                "menu-freeplay-click",
                "menuclick",
                "menu-edit-click",
                "menu-play-hover",
                "normal-hitfinish",
                "menu-freeplay2-click",
                "menu-multiplayer-click",
                "menu-exit-hover",
                "menu-freeplay-hover",
                "menu-multiplayer-hover",
                "pause-hover",
                "menuHit",
                "soft-hitfinish",
                "menu-play-click",
                "normal-hitclap2",
                "normal-hitclap",
                "normal-hitwhistle",
                "spinnerbonus",
                "soft-hitclap",
                "soft-hitnormal",
                "soft-hitclap2",
                "drum-sliderslide",
                "normal-slidertick",
                "normal-hitnormal",
                "drum-sliderwhistle",
                "menuclick",
                "soft-hitwhistle",
                "normal-sliderslide",
                "normal-sliderwhistle",
                "pause-hover",
                "pause-back-click",
                "drum-slidertick",
                "menuHit",
                "pause-retry-click",
                "pause-continue-click",
                "soft-slidertick",
                "soft-sliderslide",
                "soft-sliderwhistle",
                "combobreak",
                "failsound",
                "sectionfail",
                "sectionpass",
            };
            char[] numbers =
            {
                '1',
                '2',
                '3',
                '4',
                '5',
                '6',
                '7',
                '8',
                '9',
                '0',
                ' ',
            };
            #endregion

            Console.Write("Reading config file...");
            if (Settings1.Default.sourcePath == "" || Settings1.Default.targetPath == "")
            {
                Console.WriteLine("Failed");
                Console.WriteLine();
                modConfig();
            }
            else
            {
                Console.WriteLine("Done");
                Console.WriteLine();
                Console.WriteLine("Current osu! songs folder path: " + Settings1.Default.sourcePath);
                Console.WriteLine("Current destination songs folder path: " + Settings1.Default.targetPath);
                Console.WriteLine("Modify config file?(y/N)");
                string response1 = Console.ReadLine();
                if (response1 == "y" || response1 == "Y")
                    modConfig();
            }
            Console.Clear();

            Console.Write("Discovering files...");
            string[] folders = System.IO.Directory.GetDirectories(Settings1.Default.sourcePath);
            int total = 0;
            fileList fileList = new fileList();
            foreach (string f in folders)
            {
                string folder = System.IO.Path.GetFileName(f);
                string[] files = System.IO.Directory.GetFiles(f);
                foreach (string s in files)
                {
                    total++;
                    file file = new file()
                    {
                        count = total,
                        path = s,
                        folder = folder,
                    };
                    fileList.Add(file);
                }
            }
            Console.WriteLine("Done");
            System.Threading.Thread.Sleep(450);
            Console.Clear();

            int songs = 0;
            foreach (var item in fileList)
            {
                Console.Write("File " + item.count + " of " + total + "...");

                if (System.IO.Path.GetExtension(item.path) != ".mp3")
                {
                    Console.WriteLine("Discarded");
                    continue;
                }
                if (!validate(System.IO.Path.GetFileNameWithoutExtension(item.path)))
                {
                    Console.WriteLine("Discarded");
                    continue;
                }
                Console.Write("Copying...");
                System.IO.File.Copy(item.path, System.IO.Path.Combine(Settings1.Default.targetPath, item.folder.TrimStart(numbers) + ".mp3"), true);
                songs++;
                Console.WriteLine("Done");
            }

            Console.WriteLine("\n\n");
            Console.WriteLine("Finished copying " + songs + " songs. Thank you for using this program!");
            Console.ReadLine();

            void modConfig()
            {
                Console.WriteLine("New osu! songs folder path: ");
                string response1 = Console.ReadLine();
                if (response1 != "")
                    Settings1.Default.sourcePath = response1;
                Console.WriteLine("New destination folder path: ");
                string response2 = Console.ReadLine();
                if (response2 != "")
                    Settings1.Default.targetPath = response2;
                Settings1.Default.Save();
                Console.WriteLine("Settings Saved");
                System.Threading.Thread.Sleep(450);
            }
            bool validate(string item)
            {
                for (int i = 0; i < blacklist.Length - 1; i++)
                {
                    if (string.Equals(item, blacklist[i], StringComparison.OrdinalIgnoreCase))
                        return false;
                }
                return true;
            }
        }
    }
    public class fileList : List<file> { }
    public class file
    {
        public int count { get; set; }
        public string path { get; set; }
        public string folder { get; set; }
    }
}

