using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using IntCoachAuswerter.Models;
using Newtonsoft.Json;

namespace IntCoachAuswerter.Pages.LoadingPage
{
    public class PatientFileTransformToJsonStringService
    {
        public string TransformPatientFileStringToJsonString(string patientFileString)
        {
            var patientDataList = new List<PatientData>();
            var patientEntryLines = patientFileString.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for(var i = 0; i < patientEntryLines.Length; i++)
            {
                patientEntryLines[i] = patientEntryLines[i].Substring(1, patientEntryLines[i].Length - 2);
            }
            for (var j = 0; j < patientEntryLines.Length / 3; j++)
            {
                var patientDataEntry = patientEntryLines.Skip(j*3).Take(3).ToArray();
                var patientData = new PatientData
                {
                    Date = DateTime.Parse(patientDataEntry[0], CultureInfo.InvariantCulture),
                    EmotionValues = patientDataEntry[1]?.Split(',')?.Select(x => double.Parse(x, CultureInfo.InvariantCulture))?.ToList(),
                    FeelingQuestions = patientDataEntry[2].Split("-".ToCharArray()).ToList()
                };
                patientData.EmotionValues.RemoveAt(patientData.EmotionValues.Count - 1);
                patientData.EmotionValues.RemoveAt(patientData.EmotionValues.Count - 1);
                patientDataList.Add(patientData);
            }
            return JsonConvert.SerializeObject(patientDataList);
        }
    }
}