using EventMate.Core.Model.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Core.Model.Concrete
{
    public class City : BaseModel
    {
        public string Name { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}
