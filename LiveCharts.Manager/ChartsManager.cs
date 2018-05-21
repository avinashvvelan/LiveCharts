using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts.Contracts;

namespace LiveCharts.Manager
{
    public class ChartsManager : IChartsManager
    {
        private static ChangeNotificationCallback _changeNotificationCallback = null;
        private Data.ChartsDataNotification chartsDataNotification = null;

        private Data.LiveChartsData liveChartsData = null;
        private List<string> connectedClients = new List<string>();

        public ChartsManager()
        {
            this.liveChartsData = new Data.LiveChartsData();
            this.chartsDataNotification = Data.ChartsDataNotification.Instance();
            _changeNotificationCallback = chartsDataNotification.GetChangeNotificationCallback();
        }

        public bool IsRunning()
        {
            return chartsDataNotification.IsRunning();
        }

        public void StartService()
        {
            chartsDataNotification.StartService();
        }

        public void StopService()
        {
            chartsDataNotification.StopService();
        }

        public string ClientSubscription(string clientId)
        {
            if (connectedClients.Count == 0 && chartsDataNotification.IsRunning() == false)
            {
                chartsDataNotification.StartService();
            }
            connectedClients.Add(clientId);

            return clientId;
        }


        public bool ClientUnSubscribe(string clientId)
        {
            bool returnValue = false;

            var unSubscribedClient = connectedClients.FirstOrDefault(c => c == clientId);
            if (string.IsNullOrEmpty(unSubscribedClient) == false)
            {
                returnValue = connectedClients.Remove(unSubscribedClient);
            }

            if (connectedClients.Count == 0 && chartsDataNotification.IsRunning() == true)
            {
                chartsDataNotification.StopService();
            }

            return returnValue;
        }

        public StepInstance[] GetStepInstances(long runInstanceId)
        {
            throw new NotImplementedException();
        }

        public ChangeNotificationCallback GetChangeNotificationCallback()
        {
            return _changeNotificationCallback;
        }
    }
}
