using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LiveCharts.Contracts
{
    [DataContract]
    public class RunInstance
    {
        [DataMember]
        public long RunInstanceId { get; set; }
        [DataMember]
        public long ProcessId { get; set; }
        [DataMember]
        public DateTime StartTime { get; set; }
        [DataMember]
        public DateTime EndTime { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Messages { get; set; }
        [DataMember]
        public StepInstance[] StepInstances { get; set; }
    }
}
