using System;
using System.Collections.Generic;
using System.IO;
using IntCoachAuswerter.Models;
using Newtonsoft.Json;

namespace IntCoachAuswerter.Pages.LoadingPage
{
    public static class PatientFileReaderService
    {
        public static List<PatientData> Read(string fileName)
        {
            if (fileName != null)
            {
                var jsonString = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + fileName);
                return JsonConvert.DeserializeObject<List<PatientData>>(jsonString);
            }
            return null;
        }
    }
}