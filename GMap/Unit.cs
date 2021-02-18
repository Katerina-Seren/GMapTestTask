using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMap
{
    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }

        public Unit(int id, string name, double lat, double lng)
        {
            Id = id;
            Name = name;
            Lat = lat;
            Lng = lng;
        }
    }
}
