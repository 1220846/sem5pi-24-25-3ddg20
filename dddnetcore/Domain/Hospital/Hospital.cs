using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dddnetcore.Domain.Hospital
{
    public class Hospital
    {
        public int[] busyRooms {get; set;}
        public string GroundTextureUrl { get; set; }
        public string WallTextureUrl { get; set; }
        public Size Size { get; set; }
        public int[][] Map { get; set; }
    }
}