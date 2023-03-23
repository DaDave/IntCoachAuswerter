namespace IntCoachAuswerter.Pages.LoadingPage
{
    public class PatientFileImportService
    {
        public void Import(string patientFileString)
        {
            var patientFileTransformToJsonStringService = new PatientFileTransformToJsonStringService();
            var patientFileWriteToJsonFileService = new PatientFileWriteToJsonFileService();
            if (patientFileString != null)
            {
                var patientDataJsonString = patientFileTransformToJsonStringService.TransformPatientFileStringToJsonString(patientFileString);
                patientFileWriteToJsonFileService.WritePatientDataJsonStringToJsonFile(patientDataJsonString);
            }
        }
    }
}