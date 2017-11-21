using System.Collections;
using System.Collections.Generic;

namespace ZebraBellaComponentsUtility.Components
{
    public interface IComponentsService
    {
        void Start();
        void Restart();
        void Stop();
        void ClearStorage();
        void ClearLogs();
    }
}