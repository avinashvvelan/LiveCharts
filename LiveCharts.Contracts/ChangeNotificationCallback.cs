using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveCharts.Contracts
{
    public class ChangeNotificationCallback
    {

        public event EventHandler<ChangeNotificationEventArgs> ItemChangedHandler = delegate { };

        public void NotifyItemChanged(StepInstance[] stepInstances)
        {
            ChangeNotificationEventArgs changeNotificationEventArgs = new ChangeNotificationEventArgs()
            {
                StepInstances = stepInstances
            };
            this.ItemChangedHandler(this, changeNotificationEventArgs);
        }
    }
}
