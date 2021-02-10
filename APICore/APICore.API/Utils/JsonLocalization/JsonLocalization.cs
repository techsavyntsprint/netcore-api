using System.Collections.Generic;

namespace APICore.API.Utils.JsonLocalization
{
    public class JsonLocalization
    {
        public string Key { get; set; }

        public Dictionary<string, string> LocalizedValue = new Dictionary<string, string>();
    }
}