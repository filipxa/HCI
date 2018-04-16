using Newtonsoft.Json;
using Newtonsoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Charts
{
    static class DataHandler
    {
        public static List<PointModel> JSONtoPoint(string json, ref string lastRefresh, bool isOHLC)
        {
            List<PointModel> tacke;
            if (isOHLC)
            {
                tacke = OHLCToPoint(json, ref lastRefresh);
            }
            else
                tacke = ValueToPoint(json, ref lastRefresh);
            return tacke;
        }

        private static List<PointModel> ValueToPoint(string json, ref string lastRefresh)
        {
            //Dictionary<string, string> MetaData = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);//sve nije samo meta data
            bool isOverMetaData = false;
            bool isOverPoint = true;
            bool isFirstPoint = true; // za datum proverva da li je prva tacka
            DateTime dateCurent;

            List<PointModel> allPoints = new List<PointModel>();
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            string comparingRefresh = "";
            ValuePointModel point;
            double value;

            reader.Read();
            while (reader.Read())
            {
                //Newtonsoft.Json.JsonToken end = JsonToken.pr;
                if (reader.TokenType == JsonToken.EndObject && !isOverMetaData)
                {
                    isOverMetaData = true;
                    reader.Read();//naziv treba da se preskoci samo prvi put
                    reader.Read();//otovrena zagrada
                    reader.Read();
                }

                if (isOverMetaData && isOverPoint)
                {
                    if (reader.Value != null)
                    {    

                        comparingRefresh = reader.Value.ToString();
                        if (comparingRefresh.Length > 16)// sekunde da se odseku uvek su 00
                        {
                            comparingRefresh = comparingRefresh.Substring(0, 16);
                            dateCurent = DateTime.ParseExact(comparingRefresh, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                        }
                        else if (comparingRefresh.Length == 10)
                        {
                            dateCurent = DateTime.ParseExact(comparingRefresh, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            dateCurent = DateTime.ParseExact(comparingRefresh, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                        }
                        //lastRefresh = comparingRefresh
                        if (lastRefresh.Equals(comparingRefresh)) {
                            break;
                        }
                        
                        //dateCurent = DateTime.ParseExact(comparingRefresh, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                        if (isFirstPoint)
                        {
                            lastRefresh = comparingRefresh;
                            isFirstPoint = false;
                        }
                        reader.Read();//otvorena zagrada
                        reader.Read();//naziv
                        reader.Read();//Vrednost <- potrebno
                        value = double.Parse(reader.Value.ToString(), CultureInfo.InvariantCulture);
                        //Console.WriteLine(value);

                        
                        point = new ValuePointModel(value, dateCurent);
                        if (allPoints.Count > 200)
                            break;
                        allPoints.Add(point);
                        isOverPoint = false;
                    }
                }
                else
                {
                    if (reader.TokenType == JsonToken.EndObject)
                    {
                        isOverPoint = true;
                    }
                }

            }
            return allPoints;
        }

        private static List<PointModel> OHLCToPoint(string json, ref string lastRefresh)
        {
            bool isOverMetaData = false;
            bool isOverPoint = true;
            bool isFirstPoint = true; // za datum proverva da li je prva tacka
            DateTime dateCurent;
   
            string[] nameOfDataOHLC = { "open", "high", "low", "close" };
            int conterForOHLC = 0;

            List<PointModel> allPoints = new List<PointModel>();
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            string comparingRefresh = "";
            OHLCPointModel point;
            double value;

            string readerAsString;
            reader.Read();
            while (reader.Read())
            {
                //Newtonsoft.Json.JsonToken end = JsonToken.pr;
                if (reader.TokenType == JsonToken.EndObject && !isOverMetaData)
                {
                    isOverMetaData = true;
                    reader.Read();//naziv treba da se preskoci samo prvi put
                    reader.Read();//otovrena zagrada
                    reader.Read();
                }

                if (isOverMetaData && isOverPoint)
                {
                    if (reader.Value != null)
                    {

                        comparingRefresh = reader.Value.ToString();
                        if (comparingRefresh.Length > 16)// sekunde da se odseku uvek su 00
                        {
                            comparingRefresh = comparingRefresh.Substring(0, 16);
                            dateCurent = DateTime.ParseExact(comparingRefresh, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                        }
                        else if (comparingRefresh.Length == 10)
                        {
                            dateCurent = DateTime.ParseExact(comparingRefresh, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            dateCurent = DateTime.ParseExact(comparingRefresh, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                        }
                        //lastRefresh = comparingRefresh
                        if (lastRefresh.Equals(comparingRefresh))
                        {
                            break;
                        }

                        

                        if (isFirstPoint)
                        {
                            lastRefresh = comparingRefresh;
                            isFirstPoint = false;
                        }
                        reader.Read();//otvorena zagrada

                        point = new OHLCPointModel();
                        point.DateTime = dateCurent;
                        reader.Read();
                        while (reader.TokenType != JsonToken.EndObject)
                        {
                            
                            readerAsString = reader.Value.ToString();
                            if (readerAsString.Contains(nameOfDataOHLC[conterForOHLC])&& conterForOHLC == 0)
                            {
                                reader.Read();
                                point.Open = double.Parse(reader.Value.ToString(), CultureInfo.InvariantCulture);
                                conterForOHLC++;
                            }
                            else if(readerAsString.Contains(nameOfDataOHLC[conterForOHLC]) && conterForOHLC == 1)
                            {
                                reader.Read();
                                point.High = double.Parse(reader.Value.ToString(), CultureInfo.InvariantCulture);
                                conterForOHLC++;
                            }
                            else if (readerAsString.Contains(nameOfDataOHLC[conterForOHLC]) && conterForOHLC == 2)
                            {
                                reader.Read();
                                point.Low = double.Parse(reader.Value.ToString(), CultureInfo.InvariantCulture);
                                conterForOHLC++;
                            }
                            else if (readerAsString.Contains(nameOfDataOHLC[conterForOHLC]) && conterForOHLC == 3)
                            {
                                reader.Read();
                                point.Close = double.Parse(reader.Value.ToString(), CultureInfo.InvariantCulture);
                            }
                            reader.Read();

                        }
                        
                        //reader.Read();//naziv


                        
                        allPoints.Add(point);
                        conterForOHLC = 0;
                        isOverPoint = false;
                        if (reader.TokenType == JsonToken.EndObject)
                        {
                            isOverPoint = true;
                        }
                    }
                }
                else
                {
                    if (reader.TokenType == JsonToken.EndObject)
                    {
                        isOverPoint = true;
                    }
                }

            }
            return allPoints;
        }
    
    }
}
