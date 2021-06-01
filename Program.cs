using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Vlec
{
    class Program
    {
        enum Mode
        {
            View,
            Edit,
            Exiting
        }
        static void Main(string[] args)
        {
            string commandList = "Commands: Ctrl+X-Exit  E-Edit  I-Insert Line  D-Delete Line";
            string fileLocation;
            int currentline = 0;
            Mode currentMode = Mode.View;
            Console.WriteLine(args.Length);
            if(args.Length == 0)
            {
                Console.WriteLine("Please enter the file to edit/create");
                SendKeys.SendWait(Directory.GetCurrentDirectory());
                fileLocation = Console.ReadLine();
            }
            else
            {
                fileLocation = args[0];
            }
            foreach (char item in Path.GetInvalidPathChars())
            {
                if (fileLocation.Contains(item))
                {
                    Console.WriteLine("Invalid Path");
                    return;
                } 
            }
            if (!File.Exists(fileLocation))
            {
                File.Create(fileLocation);
            }
            string[] fileContentsString = File.ReadAllLines(fileLocation);
            List<string> fileContents = new List<string>();
            foreach (string item in fileContentsString)
            {
                fileContents.Add(item);
            }

            printfile();
            while (currentMode!=Mode.Exiting)
            {
                ConsoleKeyInfo cki;
                cki = Console.ReadKey(false);
                if ((cki.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    if (cki.Key.ToString().Equals("X", StringComparison.OrdinalIgnoreCase))
                    {
                        exit();
                    }
                }

                if (cki.Key.ToString().Equals("E", StringComparison.OrdinalIgnoreCase))
                {
                    currentMode = Mode.Edit;
                }
                else if (cki.Key.ToString().Equals("I", StringComparison.OrdinalIgnoreCase) && currentMode == Mode.View)
                {
                    insert();
                }
                else if (cki.Key.ToString().Equals("D", StringComparison.OrdinalIgnoreCase) && currentMode == Mode.View)
                {
                    delete();
                }

                if (currentMode == Mode.View)
                {
                    if (cki.Key == ConsoleKey.UpArrow)
                    {
                        if (currentline == 0)
                        {
                            printfile();
                        }
                        else
                        {
                            currentline--;
                            printfile();
                        }
                    }
                    else if (cki.Key == ConsoleKey.DownArrow)
                    {
                        if (currentline == fileContents.Count)
                        {
                            printfile();
                        }
                        else
                        {
                            currentline++;
                            printfile();
                        }
                    }
                } else if (currentMode == Mode.Edit)
                {
                    Console.Clear();
                    Console.WriteLine("Commands: Enter-Finish Editing");
                    SendKeys.SendWait(fileContents[currentline]);
                    fileContents[currentline] = Console.ReadLine();
                    currentMode = Mode.View;
                    printfile();
                }
            }

            void printfile()
            {
                fileContents.TrimExcess();
                Console.Clear();
                int i;
                int j = 0;
                for (i = currentline; i <= fileContents.Count-1 ; i++)
                {
                    if(j <= Console.WindowHeight - 3)
                    {
                        Console.WriteLine(fileContents[i]);
                    }
                    j++;
                }
                Console.WriteLine(commandList);
            }

            void insert()
            {
                fileContents.Insert(currentline, "");
                printfile();
            }

            void delete()
            {
                fileContents.RemoveAt(currentline);
                if (currentline != 0)
                {
                    currentline--;
                }
                printfile();
            }

            void exit()
            {
                currentMode = Mode.Exiting;
                Console.Clear();
            Prompt:
                Console.WriteLine("Save? Y n");
                string save = Console.ReadLine();
                if (save.Contains("y") || save.Contains("Y"))
                {
                    File.WriteAllLines(fileLocation, fileContents);
                    Console.WriteLine("Saved");
                    return;
                } else if (save.Contains("n")||save.Contains("N"))
                {
                    return;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Invalid Answer");
                    goto Prompt;
                }
            }
        }
    }
}