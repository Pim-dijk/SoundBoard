using System.Windows;

namespace WpfPractice.Views
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
