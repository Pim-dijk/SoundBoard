namespace SoundBoard
{
    public class DevicesViewModel : BaseViewModel
    {
        public string deviceName { get; set; }

        public int deviceId { get; set; }

        public bool isChecked { get; set; }

        public DevicesViewModel(string deviceName, int deviceId)
        {
            this.deviceName = deviceName;
            this.deviceId = deviceId;
        }
    }
}