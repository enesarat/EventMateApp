using EventMate.Core.Model.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Core.DTO.Concrete.Event
{
    public class EventUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public int CityId { get; set; }
        public string Address { get; set; }
        public int CategoryId { get; set; }
        public int Quota { get; set; }
        public bool IsApproved { get; set; } = false;
    }
}
