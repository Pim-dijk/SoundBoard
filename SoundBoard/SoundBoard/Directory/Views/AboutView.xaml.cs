using System.Windows;

namespace SoundBoard.Views
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    public partial class AboutView : Window
    {
        public AboutView(object dataContext)
        {
            this.DataContext = DataContext;
            InitializeComponent();
        }
    }
}
