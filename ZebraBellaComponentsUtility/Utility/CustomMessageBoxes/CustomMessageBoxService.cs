using System.Threading.Tasks;
using System.Windows;

namespace ZebraBellaComponentsUtility.Utility.CustomMessageBoxes
{
    public class CustomMessageBoxService : ICustomMessageBoxService
    {
        public void Info(string content, string caption)
        {
            var customMessageBox = new CustomMessageBox(content, caption);

            Application.Current.Dispatcher.Invoke(() =>
            {
                customMessageBox.Show();
            });
        }
    }
}