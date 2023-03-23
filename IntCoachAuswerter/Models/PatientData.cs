using System;
using System.Collections.Generic;

namespace IntCoachAuswerter.Models
{
    public class PatientData
    {
        public DateTime Date { get; set; }
        public List<double> EmotionValues { get; set; }
        public List<string> FeelingQuestions { get; set; }
    }
}