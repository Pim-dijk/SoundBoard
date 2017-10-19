using System.Windows;

namespace SoundBoard.Views
{
    /// <summary>
    /// Interaction logic for ChangeNameView.xaml
    /// </summary>
    public partial class AddStreamView : Window
    {
        public AddStreamView(object dataContext)
        {
            this.DataContext = dataContext;
            InitializeComponent();
        }
    }
}
