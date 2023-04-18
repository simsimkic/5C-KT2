using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_Projekat.Service
{
    public class LocationRepository
    {
        public List<string> Countries = new List<string>();
        private JObject json;

        public LocationRepository()
        {
            json = JObject.Parse(File.ReadAllText(@"../../../Resources/Data/geo.json"));
            Countries = json.Root.Cast<JProperty>().Select(x => x.Name).ToList();
        }

        public List<string> GetCountryCities(string country)
        {
            return json[country].ToObject<List<string>>();
        }
    }
}
