using EventMate.Core.DTO.Abstract;
using EventMate.Core.DTO.Concrete.Category;
using EventMate.Core.DTO.Concrete.City;
using EventMate.Core.Model.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Core.DTO.Concrete.Event
{
    public class EventDto : BaseDto
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public CityDto CityProp { get; set; }
        public string Address { get; set; }
        public string Category { get; set; }
        public CategoryDto CategoryProp { get; set; }
        public int Quota { get; set; }
        public bool IsApproved { get; set; } = false;
    }
}
