using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StrongStart.Class
{
    public class GeoCode
    {
        private string siteAddress;

        public class GeoResponse
        {
            public string statusDescription { get; set; }
            public GeoResourceSets[] ResourceSets { get; set; }
        }

        public class GeoResourceSets
        {
            public int estimatedTotal { get; set; }
            public GeoResources[] Resources { get; set; }
        }

        public class GeoResources
        {
            public Address Address { get; set; }
            public GeoGeocodePoints[] GeocodePoints { get; set; }
        }

        public class Address
        {
            public string adminDistrict2 { get; set; }
        }

        public class GeoGeocodePoints
        {
            public string[] usageTypes { get; set; }
            public double[] coordinates { get; set; }
        }
        

        public GeoCode(string address, string city, string province)
        {
            siteAddress = address + ", " + city + ", " + province;
        }

        public GeoCode(string postalcode)
        {
            siteAddress = postalcode;
        }
        
        public GeoResources GetGeoResources()
        {
            string query = siteAddress;
            string key = "AgmOaJe9ZSfslIOAOXUb2iV4nE8ovVqaHxgW3KLNT8owGq0uXXQ0f1CBremFhjta";
            string url = string.Format("http://dev.virtualearth.net/REST/v1/Locations?q={0}&key={1}", query, key);
            
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";

            GeoResources geoResources = null;

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using(var reader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var response = reader.ReadToEnd();

                    var gResponse = JsonConvert.DeserializeObject<GeoResponse>(response);

                    if (gResponse.statusDescription.ToUpper()=="OK")
                    {
                        var geoResult = gResponse.ResourceSets.FirstOrDefault();

                        if (geoResult != null && geoResult.estimatedTotal != 0)
                        {
                            geoResources = geoResult.Resources[0];
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return geoResources;
        }
        
        const double RADIUS = 6378.16;

        public double Radians(double x)
        {
            return x * Math.PI / 180;
        }
        public double getDistance(double lat1,double lat2,double lng1,double lng2)
        {
            double dlon = Radians(lng1 - lng2);
            double dlat = Radians(lat1 - lat2);

            double a = (Math.Sin(dlat/2)*Math.Sin(dlat/2)) + Math.Cos(Radians(lat1))*Math.Cos(Radians(lat2))*(Math.Sin(dlon/2)*Math.Sin(dlon/2));
            double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return angle * RADIUS;
        }
        
    }
}
