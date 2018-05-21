using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveCharts.Contracts
{
    public class ChangeNotificationEventArgs : EventArgs
    {
        public StepInstance[] StepInstances { get; set; }
    }
}
