using System;

namespace IntCoachAuswerter.Pages.LoadingPage
{
    public class PatientFileWriteToJsonFileService
    {
        public void WritePatientDataJsonStringToJsonFile(string patientDataJsonString)
        {
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "PatientData.json", patientDataJsonString);
        }
    }
}