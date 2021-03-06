﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Net.NetworkInformation;

namespace Charts
{
     class CommandManager
    {
        static List<Tuple<string, string>> cryptoValues;
        static List<Tuple<string, string>> equityValues;
        static List<Tuple<string, string>> currencyValues;
        static Dictionary<string, Function> functions;
        static Dictionary<string, Parameter> parameters;
        private const string apiKey = "&apikey=YEKXVUQ5WPDNH6Z2";
        static public void load()
        {
            loadCrypto();
            loadEquity();
            loadCurrency();
            loadFunctions();
            loadParameters();

        }

        private static void loadParameters()
        {
            StreamReader sr = new StreamReader("parameters.json");

            parameters = JsonConvert.DeserializeObject<Dictionary<string, Parameter>>(sr.ReadToEnd());
        }

        private static void loadFunctions()
        {
            StreamReader sr = new StreamReader("funkcije.json");

            functions = JsonConvert.DeserializeObject<Dictionary<string, Function>>(sr.ReadToEnd());
        }

        private static void loadCurrency()
        {
            currencyValues = new List<Tuple<string, string>>();
            StreamReader sr = new StreamReader("physical_currency_list.csv");
            String line = sr.ReadLine();
            while ((line = sr.ReadLine()) != null)
            {
                string[] currency = line.Split(',');
                currencyValues.Add(new Tuple<string, string>(currency[0], currency[1]));
            }

        }

        private static void loadEquity()
        {
            equityValues = new List<Tuple<string, string>>();
            StreamReader sr = new StreamReader("equitysymbol.csv");
            String line = sr.ReadLine();
            while ((line = sr.ReadLine()) != null)
            {
                string[] equity = line.Split(',');
                equityValues.Add(new Tuple<string, string>(equity[1], equity[0]));
            }
        }

        private static void loadCrypto()
        {
            cryptoValues = new List<Tuple<string, string>>();
            StreamReader sr = new StreamReader("crypto.csv");
            String line = sr.ReadLine();
            while ((line = sr.ReadLine()) != null)
            {
                string[] crypto = line.Split(',');
                cryptoValues.Add(new Tuple<string, string>(crypto[0], crypto[1]));
            }
           
            
        }

        public static List<Tuple<string, string>> getCryptos()
        {
            return new List<Tuple<string, string>>(cryptoValues);
        }
        public static List<Tuple<string, string>> getCurrency()
        {
            return new List<Tuple<string, string>>(currencyValues);
        }
        public static List<Tuple<string, string>> getEquity()
        {
            return new List<Tuple<string, string>>(equityValues);
        }

        static public List<string> getAvailableFunctions()
        {
           return new List<string>(functions.Keys);

        }
        public static string getFunctionNameByfunction(Function fun)
        {
            foreach (KeyValuePair<string,Function> pair in functions) 
            {
                if (pair.Value.Equals(fun))
                {
                    return pair.Key;
                }
            }
            return "";
        }
        public static Function GetFunctionByfunctionString(string fun)
        {
            foreach(Function function in functions.Values)
            {
                if (function.function.Equals(fun))
                {
                    return function;
                }
            }
            return null;
        }

        public static Function GetFunction(string functionName)
        {
            return functions[functionName]; 

        }

        public static Parameter getParameter(string parameterKey)
        { 
            return parameters[parameterKey];
        }


        static private string getCommandAsString(Dictionary<string, string> parameters)
        {
            bool isFirst = true;
            string url="";
            foreach (KeyValuePair<string, string> entry in parameters)
            {
                if (isFirst)
                    isFirst = false;
                else
                    url += "&";

                url += entry.Key + "=" + entry.Value;
            }
            return url;
        }

        public string excuteCommand(string command,ref string message) 
        {
            string url = "https://www.alphavantage.co/query?";
            url += command;
            url += apiKey;
            //string a = load("");
            bool isMessageLoaded = false;
            string json = "";

            using (WebClient client = new WebClient())
            {
                Console.WriteLine("Starting download");
                try
                {
                    json = client.DownloadString(url);
                }
                catch (Exception)
                {
                    isMessageLoaded = true;
                    message = "Unable to contact server. Please check your connection.";
                }
                
              
                Console.WriteLine("Download done");
            }
            if (!isJsonCorrect(json))
            {
                if (!isMessageLoaded)
                    message = "Data is not available at the moment or doesn't exists.";// ovu poruku poslati nazad
                json = load(command, ref message);
                //Console.Write(json);
            }
            else
            {
                save(command, json);
                message = "Successfully downloaded new data.";
            }
                
            Console.WriteLine("JSON lenght = " + json.Length);
            
            return json;
        }
        private void save(string name,string json)
        {
            if (createFolder())
            {
                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Charts\" + name, json);
                Console.WriteLine("Fajl je sacuvan {0}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Charts\" + name);
            }

        }

        private bool createFolder()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Charts";
            Console.WriteLine("The directory was created successfully at {0}.", path);
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {
                    Console.WriteLine("That path exists already.");
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));
                Console.WriteLine("The directory was created successfully at {0}.", path);
                // Delete the directory.
                //di.Delete();
                //Console.WriteLine("The directory was deleted successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Nece da napravi folder :(");
                return false;
            }
            return true;

        }

        private string load(string name,ref string message)
        {
            try
            {
                string[] fileEntries = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Charts");
                foreach (string fileName in fileEntries)
                {
                   if (name.Equals(Path.GetFileName(fileName)))
                   {
                        message = "Could not download data from the server. Loaded data from local disk.";
                        return System.IO.File.ReadAllText(fileName);
                   }
                }

            }
            catch (Exception)
            {
                Console.WriteLine("Couldn't download data from the server. No data found on local disk.");
                message = "Couldn't download data from the server. No data found on local disk.";
            }
            return "";
        }
        private bool isJsonCorrect(string json)
        {
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            reader.Read();
            reader.Read();
            if (json.Length < 500)
                return false;

            return true;
        }

        public static string getId(Dictionary<string, string> parameters)
        {

            return getCommandAsString(parameters);

        }





        public class Function
        {
            public string function { get; set; }
            public List<string> RequiredPara { get; set; }
            public List<string> OptionalPara { get; set; }
            public bool isOHLC { get; set; }
        }
        public class Parameter
        {
            public string name { get; set; }
            public string displayName { get; set; }
            public string tooltip { get; set; }
            public List<string> options { get; set; }
        }
    }
    
}
