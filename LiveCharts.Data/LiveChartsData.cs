using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts.Contracts;
using System.Threading;

namespace LiveCharts.Data
{
    public class LiveChartsData : ILiveChartsData
    {
        private List<long> mockRunInstanceIDs = new List<long>();

        public LiveChartsData()
        {
            MockSeedData();
        }

        private void MockSeedData()
        {
            mockRunInstanceIDs.Add(1000);
            mockRunInstanceIDs.Add(1001);
            mockRunInstanceIDs.Add(1002);
        }

        public StepInstance[] GetStepInstances(long runInstanceId)
        {
            throw new NotImplementedException();
        }
    }
}
