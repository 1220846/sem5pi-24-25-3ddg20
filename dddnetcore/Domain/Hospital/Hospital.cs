using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dddnetcore.Domain.Hospital
{
    public class Hospital
    {
        public List<int> busyRooms {get; set;}
        public string GroundTextureUrl { get; set; }
        public string WallTextureUrl { get; set; }
        public Size Size { get; set; }
        public int[][] Map { get; set; }

        //! Does nothing, but breaks if doesnt exist
        public int[] initialPosition {get; set;}
        public double initialDirection {get; set;}
        public double[] exitLocation {get; set;}
    }
}