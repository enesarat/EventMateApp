using EventMate.Core.Model.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Core.Model.Concrete
{
    public class Event : BaseModel
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public string Address { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int Quota { get; set; }
        public bool IsApproved { get; set; } = false;
        public ICollection<Ticket> Tickets { get; set; }

    }
}
