using LiveCharts.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveCharts.Data
{
    public interface ILiveChartsData
    {
        StepInstance[] GetStepInstances(long runInstanceId);
    }
}
