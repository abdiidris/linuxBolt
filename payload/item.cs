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



                    directoryObjectList[directoryObjectList.Count - 1].contentIds.Add(Guid.NewGuid().ToString("N"));
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
        public static void findItem(string item)
        {

            // deserialize the objects
            List<directory> directories = JsonConvert.DeserializeObject<List<directory>>(File.ReadAllText("data/directories/directories.json"));

            List<string> items = new List<string>();


            for (int i = 0; i < directories.Count; i++)
            {
                // look inside all content of this directory
                for (int j = 0; j < directories[i].contentNames.Count; j++)
                {
                    if(directories[i].contentNames[j] == item)
                    {
                        items.Add(directories[i].name);

                        // output result
                        wm("Found " + directories[i].name, "g");
                    }
                }
            }


            // user will choose what to do next
            

        }

    }


}