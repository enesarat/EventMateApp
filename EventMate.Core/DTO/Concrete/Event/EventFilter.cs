using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Core.DTO.Concrete.Event
{
    public class EventFilter
    {
        public int CityId { get; set; }
        public int CategoryId { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
