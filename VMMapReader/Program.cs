using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Diagnostics;

namespace VMMapReader
{
    class Program
    {
        static void Main(string[] args)
        {
            string vmmapLogsPath=@"G:\TestProj\VMMapReader\";
            Dictionary<int, Dictionary<string, int>> vmmapDataList=GetVMMapLogs(vmmapLogsPath, 20);
            DisplayVMMapData(null);
        }

        public static void DisplayVMMapData(Dictionary<string, int> vmmapData)
        {
            foreach (var key in vmmapData.Keys)
            {
                Console.WriteLine("{0} Size: {1}",key,vmmapData[key]);
            }
        }

        public static Dictionary<int, Dictionary<string, int>> GetVMMapLogs(string vmmapLogsPath,int iteration)
        {
            string[] vmmapFiles = Directory.GetFiles(vmmapLogsPath);

            Dictionary<int, Dictionary<string, int>> vmmapList = new Dictionary<int, Dictionary<string, int>>();

            for (int i=1;i<= vmmapFiles.Length; i++)
            {
                if (i % iteration == 0)
                {
                    vmmapList.Add(i, GetVMMapLog(string.Format("{0}{1}", vmmapLogsPath, vmmapFiles[i - 1])));
                }
            }

            return vmmapList;

        }

        public static void Logger(string message)
        {
            System.Diagnostics.Trace.WriteLine(message);
        }
        public static Dictionary<string, int> GetVMMapLog(string vmmapLog)
        {
            if (!File.Exists(vmmapLog))
            {
                Console.WriteLine("The VMMap log doesn't exist, please use a valid VMMap log.");
            }
            Dictionary<string, int> memType = new Dictionary<string, int>();

            XmlDocument document = new XmlDocument();

            document.Load(vmmapLog);

            XmlElement rootElement = document.DocumentElement;

            XmlNodeList regionNodes = rootElement.GetElementsByTagName("Region");

            foreach (XmlNode node in regionNodes)
            {
                if (node.InnerXml.Equals(string.Empty))
                {
                    string useType = ((XmlElement)node).GetAttribute("UseType");

                    int size = int.Parse(((XmlElement)node).GetAttribute("Size")) / 1024;

                    switch (useType)
                    {
                        case "0":
                            if (!memType.ContainsKey("Heap"))
                            {
                                memType.Add("Heap", size);
                            }
                            else
                            {
                                memType["Heap"] += size;
                            }
                            break;

                        case "1":
                            if (!memType.ContainsKey("Stack"))
                            {
                                memType.Add("Stack", size);
                            }
                            else
                            {
                                memType["Stack"] += size;
                            }
                            break;
                        case "2":
                            if (!memType.ContainsKey("Image"))
                            {
                                memType.Add("Image", size);
                            }
                            else
                            {
                                memType["Image"] += size;
                            }
                            break;
                        case "3":
                            if (!memType.ContainsKey("Mapped File"))
                            {
                                memType.Add("Mapped File", size);
                            }
                            else
                            {
                                memType["Mapped File"] += size;
                            }
                            break;
                        case "4":
                            if (!memType.ContainsKey("Private Data"))
                            {
                                memType.Add("Private Data", size);
                            }
                            else
                            {
                                memType["Private Data"] += size;
                            }
                            break;
                        case "5":
                            if (!memType.ContainsKey("Shareable"))
                            {
                                memType.Add("Shareable", size);
                            }
                            else
                            {
                                memType["Shareable"] += size;
                            }
                            break;
                        case "6":
                            if (!memType.ContainsKey("Free"))
                            {
                                memType.Add("Free", size);
                            }
                            else
                            {
                                memType["Free"] += size;
                            }
                            break;
                        case "8":
                            if (!memType.ContainsKey("Managed Heap"))
                            {
                                memType.Add("Managed Heap", size);
                            }
                            else
                            {
                                memType["Managed Heap"] += size;
                            }
                            break;
                        case "9":
                            if (!memType.ContainsKey("Page Table"))
                            {
                                memType.Add("Page Table", size);
                            }
                            else
                            {
                                memType["Page Table"] += size;
                            }
                            break;
                        case "10":
                            if (!memType.ContainsKey("Unusable"))
                            {
                                memType.Add("Unusable", size);
                            }
                            else
                            {
                                memType["Unusable"] += size;
                            }
                            break;
                    }
                }
            }
            
            return memType;
        }
    }
}
