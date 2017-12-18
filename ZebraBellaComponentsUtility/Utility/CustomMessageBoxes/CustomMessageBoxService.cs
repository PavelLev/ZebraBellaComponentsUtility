using System.Windows;

namespace ZebraBellaComponentsUtility.Utility.CustomMessageBoxes
{
    public class CustomMessageBoxService : ICustomMessageBoxService
    {
        public void Info(string content, string caption)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var customMessageBox = new CustomMessageBox(content, caption);

                customMessageBox.Show();
            });
        }
    }
}