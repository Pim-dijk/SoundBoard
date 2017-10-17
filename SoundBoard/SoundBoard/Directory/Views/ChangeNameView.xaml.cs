using System.Windows;

namespace SoundBoard.Views
{
    /// <summary>
    /// Interaction logic for ChangeNameView.xaml
    /// </summary>
    public partial class ChangeNameView : Window
    {
        public ChangeNameView(object dataContext)
        {
            this.DataContext = dataContext;
            InitializeComponent();
        }
    }
}
