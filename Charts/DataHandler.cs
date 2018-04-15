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
            if (/*isOHLC*/false)
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
            List<PointModel> allPoints = new List<PointModel>();
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            string comparingRefresh = "";
            ValuePointModel point;
            int maxSize = 0;
            double value;

            reader.Read();
            while (reader.Read())
            {
                if (maxSize > 99)
                    break;
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
                        DateTime dateCurent = DateTime.ParseExact(comparingRefresh, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                        reader.Read();//otvorena zagrada
                        reader.Read();//naziv
                        reader.Read();//Vrednost <- potrebno
                        value = Convert.ToDouble(reader.Value.ToString());

                        
                        point = new ValuePointModel(value, dateCurent);
                        allPoints.Add(point);
                        maxSize++;
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
            throw new NotImplementedException();
        }
    }
}
