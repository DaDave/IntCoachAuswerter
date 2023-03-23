namespace IntCoachAuswerter.Pages.LoadingPage
{
    public class PatientFileValidateService
    {
        public bool Validate(string patientFileString)
        {
            var patientFileTransformToJsonStringService = new PatientFileTransformToJsonStringService();
            if (patientFileString != null)
            {
                try
                {
                    patientFileTransformToJsonStringService.TransformPatientFileStringToJsonString(patientFileString);
                }
                catch
                {
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}