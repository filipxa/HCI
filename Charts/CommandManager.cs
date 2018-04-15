using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

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

        public static Function GetFunction(string functionName)
        {
            return functions[functionName]; 

        }

        public static Parameter getParameter(string parameterKey)
        { 
            return parameters[parameterKey];
        }

        public string excuteCommand(Dictionary<string,string> parameters) 
        {
            string url = "https://www.alphavantage.co/query?";
            foreach (KeyValuePair<string, string> entry in parameters)
            {
                url += "&" + entry.Key + "=" + entry.Value;
            }
            url+= apiKey;
            string json = "";
            using (WebClient client = new WebClient())
            {
                 json = client.DownloadString(url);
            }
            return json;
        }

        public static string getId(Dictionary<string, string> parameters)
        {
            string url = "";
            foreach (KeyValuePair<string, string> entry in parameters)
            {
                url += "&" + entry.Key + "=" + entry.Value;
            }
            return url;

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
