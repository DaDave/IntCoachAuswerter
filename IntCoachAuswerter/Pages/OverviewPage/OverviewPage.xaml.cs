using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using IntCoachAuswerter.Models;
using LiveCharts;
using LiveCharts.Wpf;
using ModernWpf.Controls;

namespace IntCoachAuswerter.Pages.OverviewPage
{
    public partial class OverviewPage
    {
        private List<PatientData> PatientDataList;
        private List<string> EmotionNames;
        private int GraphMode = 0;
        public SeriesCollection SeriesCollection { get; set; }
        public string[] XAxisValues { get; set; }
        public string[] YAxisValues { get; set; }

        public OverviewPage(List<PatientData> patientDataList)
        {
            EmotionNames = EmotionNamesFileReader.Read();
            PatientDataList = patientDataList;
            InitializeComponent();
            
            XAxisValues = GetXAxisValuesByDates(PatientDataList.Select(x => x.Date).ToList());
            YAxisValues = GetYAxisValuesByEmotionValues(PatientDataList.Select(x => x.EmotionValues).ToList());
            
            SeriesCollection = GetSeriesCollection(PatientDataList, XAxisValues);

            DataContext = this;
        }

        private string[] GetXAxisValuesByDates(List<DateTime> dates)
        {
            var minDate = dates.Min();
            var maxDate = dates.Max();
            return GetDaysInRange(minDate, maxDate).ToArray();
        }
        
        private string[] GetYAxisValuesByEmotionValues(List<List<double>> emotionValuesList)
        {
            var maxEmotionMValue = 0.0;
            foreach (var emotionValues in emotionValuesList)
            {
                var emotionValueMax = emotionValues.Max();
                if (emotionValueMax > maxEmotionMValue)
                {
                    maxEmotionMValue = emotionValueMax;
                }
            }
            var yAxisValues = new List<string>();
            for (var i = 0; i <= maxEmotionMValue; i++)
            {
                yAxisValues.Add(i.ToString());
            }
            return yAxisValues.ToArray();
        }
        
        private List<string> GetDaysInRange(DateTime startDate, DateTime endDate)
        {
            var dayStrings = new List<string>();
            if (endDate < startDate)
                throw new ArgumentException("endDate must be greater than or equal to startDate");

            while (startDate <= endDate)
            {
                dayStrings.Add(startDate.ToString("dd.MM.yyyy"));
                startDate = startDate.AddDays(1);
            }

            return dayStrings;
        }
        
        private SeriesCollection GetSeriesCollection(List<PatientData> patientDataList, string[] xAxisValues)
        {
            var amountOfLineSeries = patientDataList.First().EmotionValues.Count;
            var filledPatientDataList = GetFilledPatientDataList(patientDataList, xAxisValues);
            var seriesCollection = new SeriesCollection();
            for (var i = 0; i < amountOfLineSeries; i++)
            {
                var lineSeries = new LineSeries()
                {
                    Title = EmotionNames[i] ?? "Kein Name vergeben",
                    PointGeometrySize = 15,
                    
                };
                var emotionValues = new List<double>();
                foreach (var filledPatientData in filledPatientDataList)
                {
                    emotionValues.Add(filledPatientData.EmotionValues?[i] ?? Double.NaN);
                }

                var chartValues = GenerateEstimatedChartValues(emotionValues);
                
                lineSeries.Values = chartValues;
                seriesCollection.Add(lineSeries);
            }
            return seriesCollection;
        }

        private ChartValues<double> GenerateEstimatedChartValues(List<double> emotionValues)
        {
            for (var i = 0; i < emotionValues.Count; i++)
            {
                if (i == 0 && double.IsNaN(emotionValues[i]))
                {
                    emotionValues[i] = 0.0;
                }

                if (i != 0 && double.IsNaN(emotionValues[i]))
                {
                    var lastValue = emotionValues[i - 1];
                    var nextValue = 0.0;
                    for (var j = emotionValues.Count-1; j > i; j--)
                    {
                        if (!double.IsNaN(emotionValues[j]))
                        {
                            nextValue = emotionValues[j];
                        }
                    }
                    emotionValues[i] = (lastValue + nextValue) / 2;
                }
            }
            var chartValues = new ChartValues<double>();
            chartValues.AddRange(emotionValues);
            return chartValues;
        }

        private List<PatientData> GetFilledPatientDataList(List<PatientData> patientDataList, string[] xAxisValues)
        {
            foreach (var xAxisValue in xAxisValues)
            {
                var isInPatientDataList = patientDataList.Exists(x => x.Date.DayOfYear == Convert.ToDateTime(xAxisValue).DayOfYear);
                if (!isInPatientDataList)
                {
                    patientDataList.Add(new PatientData
                    {
                        Date = Convert.ToDateTime(xAxisValue)
                    });
                }
            }
            return patientDataList.OrderBy(x => x.Date).ToList();
        }
        
        private void ToggleSeries(object sender, EventArgs e)
        {
            var lineSeriesAmount = SeriesCollection.Count;
            for (var i = 0; i < lineSeriesAmount; i++)
            {
                ChangeLineSeriesVisibility(i, Visibility.Collapsed);
            }
            if (GraphMode == lineSeriesAmount)
            {
                for (var i = 0; i < lineSeriesAmount; i++)
                {
                    ChangeLineSeriesVisibility(i, Visibility.Visible);
                    GraphMode = 0;
                }
            }
            else
            {
                ChangeLineSeriesVisibility(GraphMode, Visibility.Visible);
                GraphMode++;
            }
        }

        private async void ShowQuestions(object sender, RoutedEventArgs e)
        {
            var questionsString = CreateQuestionString();
            await ShowQuestionContentDialog(questionsString);
        }

        private string CreateQuestionString()
        {
            var patientData = PatientDataList.OrderBy(x => x.Date);
            var questions = "";
            foreach (var patientDataEntry in patientData.Where(patientDataEntry => patientDataEntry.FeelingQuestions != null))
            {
                questions += patientDataEntry.Date;
                questions += String.Join("\n", patientDataEntry.FeelingQuestions);
                questions += "\n\n";
            }
            return questions;
        }

        private async Task ShowQuestionContentDialog(string questionsString)
        {
            var showQuestionContentDialog = new ContentDialog
            {
                Title = "Fragen",
                Content = new TextBox()
                {
                    TextAlignment = TextAlignment.Left, 
                    Background = Brushes.White, 
                    Foreground = Brushes.Black,
                    Margin = new Thickness(5), 
                    Text = questionsString
                },
                PrimaryButtonText = "OK",
                Background = Brushes.DimGray
            };

            await showQuestionContentDialog.ShowAsync();
        }

        private void ChangeLineSeriesVisibility(int lineSeriesIndex, Visibility visibility)
        {
            var lineSeriesToShow = (SeriesCollection[lineSeriesIndex] as LineSeries);
            lineSeriesToShow.Visibility = visibility;
        }

        private void PressResetButton(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}