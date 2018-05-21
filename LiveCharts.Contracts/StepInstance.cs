using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LiveCharts.Contracts
{
    [DataContract]
    public class StepInstance
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long ParentRunInstanceId { get; set; }
        [DataMember]
        public DateTime? StartTime { get; set; }
        [DataMember]
        public DateTime? EndTime { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Message { get; set; }
    }
}
