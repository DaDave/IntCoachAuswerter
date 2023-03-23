using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace IntCoachAuswerter.Pages.OverviewPage
{
    public static class EmotionNamesFileReader
    {
        public static List<string> Read()
        {
            var jsonString = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "IntCoachEmotionNames.json");
            return JsonConvert.DeserializeObject<List<string>>(jsonString);
        }
    }
}