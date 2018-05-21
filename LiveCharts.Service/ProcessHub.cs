using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LiveCharts.Contracts;
using LiveCharts.Manager;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace LiveCharts.Service
{
    [HubName("processHub")]
    public class ProcessHub : Hub
    {
        private readonly StockTicker _stockTicker;
        private readonly IChartsManager _chartsManager;
        private ChangeNotificationCallback _changeNotificationCallback = null;
        private string clientInfo = null;

        public ProcessHub() : this(StockTicker.Instance) { }

        public ProcessHub(StockTicker stockTicker)
        {
            _stockTicker = stockTicker;
            _chartsManager = new ChartsManager();
            _changeNotificationCallback = _chartsManager.GetChangeNotificationCallback();
            _changeNotificationCallback.ItemChangedHandler += changeNotificationCallback_ItemChangedHandler;
        }


        void changeNotificationCallback_ItemChangedHandler(object sender, ChangeNotificationEventArgs e)
        {
            if (e.StepInstances != null && clientInfo != null)
            {
                List<StepInstance> changes = new List<StepInstance>();
                changes.AddRange(e.StepInstances);

                foreach (StepInstance stepInstance in changes)
                {
                    Push(stepInstance);
                }
            }
        }


        public void Push(StepInstance stepInstance)
        {
            var client = Clients.Client(clientInfo);

            if (client != null)
            {
                client.broadcastMessage(stepInstance);
            }

        }


        public override Task OnConnected()
        {
            clientInfo = Context.ConnectionId;
            //string appName = Context.QueryString["app_name"];
            //string appModule = Context.QueryString["app_module"];
            _chartsManager.ClientSubscription(clientInfo);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            _chartsManager.ClientUnSubscribe(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }


        public IEnumerable<Stock> GetAllStocks()
        {
            return _stockTicker.GetAllStocks();
        }
    }
}