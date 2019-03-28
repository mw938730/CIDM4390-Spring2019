using System;

namespace ridershipdemo.Models
{
    public class TripData
    {
        public int ID {get; set;}
        public int RiderID {get; set;}
        public int BusID {get; set;}
        public int RouteID {get; set;}

        public DateTime RideBegin{get; set;}
        public DateTime RideEnd {get; set;}
        
    }
}