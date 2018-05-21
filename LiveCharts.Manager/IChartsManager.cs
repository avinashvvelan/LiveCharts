using LiveCharts.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveCharts.Manager
{
    public interface IChartsManager
    {
        StepInstance[] GetStepInstances(long runInstanceId);
        bool IsRunning();
        void StartService();
        void StopService();
        string ClientSubscription(string clientId);
        bool ClientUnSubscribe(string clientId);
        ChangeNotificationCallback GetChangeNotificationCallback();
    }
}
