using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
namespace payload
{
    public class find
    {


        public static void getSystem()
        {
            //Command
            // info
            //  wm("RUN sudo dotnetrun", "g");
            string command = @"cd / && sudo ls -a -R >> /home/abdi/Documents/Project/linProject/payload/data/temp/largeList.txt";
            runCommand(command);
            //Command

            /* 
                        for the number of lines
                        while line.sub(0,2) not ./ direcory = current
                        else directory = new direcot

            */

            List<directory> directoryObjectList = new List<directory>();
            //     List<find> itemObjectList = new List<find>();
            // remove white lines
            string[] largeList = File.ReadAllLines("data/temp/largeList.txt");

            // store the first directory
            wm("/", "m");
            directoryObjectList.Add(new directory() { name = "/" });

            for (int i = 0; i < largeList.Length; i++)
            {
                // find
                if (largeList[i].Length >= 2 && largeList[i].Substring(0, 2) == "./")
                {
                    wm(largeList[i], "m", i, largeList.Length);
                    directoryObjectList.Add(new directory() { name = largeList[i] });
                }
                else if (largeList[i].Length > 0) // miss blank lines
                {

                    wm(largeList[i], "g", i, largeList.Length);



                    directoryObjectList[directoryObjectList.Count - 1].contentNames.Add(largeList[i]);


                }
            }

            // store the items
            string directoryObjectListJson = JsonConvert.SerializeObject(directoryObjectList, Formatting.Indented);
            //string itemObjectListJson = JsonConvert.SerializeObject(itemObjectList, Formatting.Indented);

            File.WriteAllText("data/directories/directories.json", directoryObjectListJson);
            //File.WriteAllText("data/items/items.json", itemObjectListJson);
        }



        public static void wm(string message = "working", string messageColor = "white", int stage = 0, int finish = 0)
        {
            switch (messageColor)
            {
                case "b":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case "r":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "g":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "m":
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case "y":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }

            if (finish > 0 & stage > 0)
            {
                System.Console.WriteLine("[" + DateTime.Now + " - " + ((int)Math.Round((double)(100 * stage) / finish)).ToString() + "%] --- " + message);
            }
            else
            {
                System.Console.WriteLine("[" + DateTime.Now + "] --- " + message);
            }

            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void runCommand(string command)
        {
            Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "/bin/bash";

            proc.StartInfo.RedirectStandardOutput = true;

            proc.StartInfo.Arguments = "-c \" " + command + " \"";
            proc.Start();
            string a = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();

        }

        public static void installFindItem()
        {
            // deserialize the objects
            List<directory> directories = JsonConvert.DeserializeObject<List<directory>>(File.ReadAllText("data/directories/directories.json"));

            // save the items in each directory in alphabetical order
            // item array format item name ?#  item parent
            List<string> itemsAndDirectoryNames = new List<string>();

            for (int i = 0; i < directories.Count; i++)
            {
                for (int j = 0; j < directories[i].contentNames.Count; j++)
                {
                    wm("installing find item", "g", i, directories.Count);
                    itemsAndDirectoryNames.Add(directories[i].contentNames[j] + "#dir#" + directories[i].name);

                }
            }
            // sort the array
            itemsAndDirectoryNames.Sort();

            // break up the strings into items and directories

            //Tuple<List<string>,List<string[]>> item = new Tuple<List<string>, List<string[]>>(new List<string>(), new List<string[]>());

            string[] items = new string[itemsAndDirectoryNames.Count];
            string[] direcoryNames = new string[itemsAndDirectoryNames.Count];
            for (int i = 0; i < itemsAndDirectoryNames.Count; i++)
            {

                // get the item name
                for (int j = 0; j < itemsAndDirectoryNames[i].Length; j++)
                {
                    if (itemsAndDirectoryNames[i].Substring(j, 5) == "#dir#")
                    {
                        break;
                    }
                    else
                    {
                        items[i] = items[i] + itemsAndDirectoryNames[i][j];
                    }
                }


                // get directory
                direcoryNames[i] = itemsAndDirectoryNames[i].Substring(items[i].Length + 5, itemsAndDirectoryNames[i].Length - (items[i].Length + 5));
            }


            File.WriteAllLines("data/find/allItems.txt", items);
            File.WriteAllLines("data/find/directoryNames.txt", direcoryNames);
        }
        public static void findItem(string item)
        {


            List<string> allItems = new List<string>(File.ReadAllLines("data/find/allItems.txt"));

            int indexFound = allItems.BinarySearch(item);
            // show this directory
            // get the dirctory

            // get directory
            if (indexFound >= 0)
            {
                wm(File.ReadAllLines("data/find/directoryNames.txt")[indexFound], "g");

                int stepBack = indexFound - 1;
                while (allItems[stepBack] == item)
                {
                    wm(File.ReadAllLines("data/find/directoryNames.txt")[stepBack], "g");
                    stepBack = stepBack - 1;
                }

                int stepForward = indexFound + 1;
                while (allItems[stepForward] == item)
                {
                    wm(File.ReadAllLines("data/find/directoryNames.txt")[stepForward], "g");
                    stepForward = stepForward - 1;
                }
            }
            else
            {
                wm("nothing found", "y");
            }



            // user will choose what to do next
            //   for (int i = 1; i < allItemsTrimmed.Count; i++)
            ///{
            //     if (allItemsTrimmed[indexFound - i] = item)
            //       {

            //     }
            // }

            /*
                List<string> allItemsList = new List<string>(allItems);
                int index = allItemsList.BinarySearch("Kconfig#./usr/src/linux-headers-4.15.0-30/drivers/gpu/drm/zte:");
                for (int i = 0; i < allItems.Count; i++)
                {
                    string name = "";
                    // get the item name
                    for (int j = 0; j < allItems[i].Length; j++)
                    {
                        if (allItems[i][j] == '#')
                        {
                            break;
                        }
                        else
                        {
                            name = name + allItems[i][j];
                        }
                    }

                    if (name == item)
                    {
                        // get directory
                        wm(allItems[i].Substring(name.Length, allItems[i].Length - name.Length));
                    }
                }
          */

        }

    }


}