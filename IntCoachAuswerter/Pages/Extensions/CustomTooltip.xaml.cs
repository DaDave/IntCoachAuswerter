using System.ComponentModel;
using LiveCharts;
using LiveCharts.Wpf;

namespace IntCoachAuswerter.Pages.Extensions
{
    public partial class CustomTooltip : IChartTooltip
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private TooltipData _data { get; set; }
        public TooltipSelectionMode? SelectionMode { get; set; }
        
        public CustomTooltip()
        {
            InitializeComponent();
            DataContext = this;
        }
        
        public TooltipData Data
        {
            get { return _data; }
            set
            {
                
                _data = value;
                OnPropertyChanged("Data");
            }
        }
        
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}