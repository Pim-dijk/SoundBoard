using System.Windows;

namespace SoundBoard.Views
{
    /// <summary>
    /// Interaction logic for InfoView.xaml
    /// </summary>
    public partial class InfoView : Window
    {
        public InfoView(object dataContext)
        {
            this.DataContext = dataContext;
            InitializeComponent();
        }
    }
}
