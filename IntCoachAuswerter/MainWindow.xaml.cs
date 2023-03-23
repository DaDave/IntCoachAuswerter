using System.Windows.Media;
using IntCoachAuswerter.Pages.LoadingPage;

namespace IntCoachAuswerter
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Background = Brushes.DimGray;
            var loadingPage = new LoadingPage();
            _NavigationFrame.Navigate(loadingPage); 
        }
    }
}