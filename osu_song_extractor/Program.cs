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
            };
            int[] lang =
            {
                0,
                1,
            };
            string[] en_us =
            {
                "Reading config file...",
                "Failed",
                "Done",
                "Current osu! songs folder path: ",
                "Current destination songs folder path: ",
                "Modify config file?(y/N)",
                "Discovering files...",
                "File ",
                " of ",
                "Discarded",
                "Copying...",
                "Finished copying ",
                " songs. Thank you for using this program!",
                "New osu! songs folder path: ",
                "New destination folder path: ",
                "Settings saved",
                "Select language:",
            };
            string[] pt_br =
            {
                "Lendo arquivo config...",
                "Falha",
                "Pronto",
                "Caminho atual para pasta de músicas do osu!: ",
                "Caminho atual para pasta de destino: ",
                "Modificar arquivo config?(y/N)",
                "Descobrindo arquivos...",
                "Arquivo ",
                " de ",
                "Descartado",
                "Copiando...",
                "Foram copiadas ",
                " músicas. Obrigado por usar este programa!",
                "Novo caminho para pasta de músicas do osu!: ",
                "Novo caminho para pasta de destino: ",
                "Configurações salvas",
                "Selecione idioma:",
            };
            #endregion

            string[] text = { };

            loadLang();

            Console.Write(text[0]);
            if (Settings1.Default.sourcePath == "" || Settings1.Default.targetPath == "")
            {
                Console.WriteLine(text[1]);
                Console.WriteLine();
                modConfig();
            }
            else
            {
                Console.WriteLine(text[2]);
                Console.WriteLine();
                Console.WriteLine(text[3] + Settings1.Default.sourcePath);
                Console.WriteLine(text[4] + Settings1.Default.targetPath);
                Console.WriteLine(text[5]);
                string response1 = Console.ReadLine();
                if (response1 == "y" || response1 == "Y")
                    modConfig();
            }
            Console.Clear();

            Console.Write(text[6]);
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
            Console.WriteLine(text[2]);
            System.Threading.Thread.Sleep(450);
            Console.Clear();

            int songs = 0;
            foreach (var item in fileList)
            {
                Console.Write(text[7] + item.count + text[8] + total + "...");

                if (System.IO.Path.GetExtension(item.path) != ".mp3")
                {
                    Console.WriteLine(text[9]);
                    continue;
                }
                if (!validate(System.IO.Path.GetFileNameWithoutExtension(item.path)))
                {
                    Console.WriteLine(text[9]);
                    continue;
                }
                Console.Write(text[10]);
                item.folder = item.folder.TrimStart(numbers);
                System.IO.File.Copy(item.path, System.IO.Path.Combine(Settings1.Default.targetPath, item.folder.TrimStart(' ') + ".mp3"), true);
                songs++;
                Console.WriteLine(text[2]);
            }

            Console.WriteLine("\n\n");
            Console.WriteLine(text[11] + songs + text[12]);
            Console.ReadLine();

            void modConfig()
            {
                Console.WriteLine(text[16]);
                Console.WriteLine("0: EN(US)");
                Console.WriteLine("1: PT(BR)");
                string response0 = Console.ReadLine();
                if (validateLang(response0))
                {
                    switch (response0)
                    {
                        case "0":
                            Settings1.Default.lang = 0;
                            break;
                        case "1":
                            Settings1.Default.lang = 1;
                            break;
                        default:
                            Settings1.Default.lang = 0;
                            break;
                    }
                    loadLang();
                }
                Console.WriteLine(text[13]);
                string response1 = Console.ReadLine();
                if (System.IO.Path.GetFileName(response1) == "osu!")
                    response1 = System.IO.Path.Combine(response1, "Songs");
                if (response1 != "")
                    Settings1.Default.sourcePath = response1;
                Console.WriteLine(text[14]);
                string response2 = Console.ReadLine();
                if (response2 != "")
                    Settings1.Default.targetPath = response2;
                Settings1.Default.Save();
                Console.WriteLine(text[15]);
                System.Threading.Thread.Sleep(450);
            }
            void loadLang()
            {
                switch (Settings1.Default.lang)
                {
                    case 0:
                        text = en_us;
                        break;
                    case 1:
                        text = pt_br;
                        break;
                    default:
                        text = en_us;
                        break;
                }
            }
            bool validate(string item)
            {
                for (int i = 0; i < blacklist.Length; i++)
                {
                    if (string.Equals(item, blacklist[i], StringComparison.OrdinalIgnoreCase))
                        return false;
                }
                return true;
            }
            bool validateLang(string response)
            {
                for (int i = 0; i < lang.Length; i++)
                {
                    if (string.Equals(response, lang[i].ToString(), StringComparison.OrdinalIgnoreCase))
                        return true;
                }
                return false;
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

