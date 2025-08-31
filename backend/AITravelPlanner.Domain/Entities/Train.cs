using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITravelPlanner.Domain.Entities
{
    public class Train
    {
        public DateTime DepartureDate { get; set; }


        public int Id { get; set; }
        public string TrainName { get; set; }
        public string TrainNumber { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Price { get; set; }
    }
}
