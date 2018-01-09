namespace SoundBoard
{
    public class ListEntriesViewModel : BaseViewModel
    {
        public string message { get; set; }
        public string toolTip { get; set; }

        public ListEntriesViewModel(string message, string toolTip)
        {
            this.message = message;
            this.toolTip = toolTip;
        }
    }
}
