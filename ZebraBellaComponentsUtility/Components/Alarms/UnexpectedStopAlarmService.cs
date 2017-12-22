using System;
using System.Collections.Generic;
using System.Threading;
using ZebraBellaComponentsUtility.ConfigurationSections.UserData.ConfigurationElements;
using ZebraBellaComponentsUtility.Utility.CustomMessageBoxes;

namespace ZebraBellaComponentsUtility.Components.Alarms
{
    public class UnexpectedStopAlarmService : IUnexpectedStopAlarmService
    {
        private readonly ICustomMessageBoxService _customMessageBoxService;
        private readonly Miscellaneous _miscellaneous;
        private readonly List<string> _componentNamesToDisplay = new List<string>();

        private readonly Timer _timer;

        private readonly object _syncRoot = new object();

        public UnexpectedStopAlarmService(ICustomMessageBoxService customMessageBoxService, Miscellaneous miscellaneous)
        {
            _customMessageBoxService = customMessageBoxService;
            _miscellaneous = miscellaneous;

            _timer = new Timer(TimerCallback, null, Timeout.Infinite, Timeout.Infinite);
        }

        public void Alarm(string componentName)
        {
            lock (_syncRoot)
            {
                _componentNamesToDisplay.Add(componentName);

                _timer.Change(_miscellaneous.AlarmDelay, Timeout.Infinite);
            }
        }

        private void TimerCallback(object state)
        {
            lock (_syncRoot)
            {
                _componentNamesToDisplay.Sort();

                var content = string.Join("\n", _componentNamesToDisplay);
                var caption = "R.I.P.";
                
                _customMessageBoxService.Info(content, caption);

                _componentNamesToDisplay.Clear();
            }
        }
    }
}