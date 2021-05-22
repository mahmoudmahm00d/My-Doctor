using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.DTOs
{
    public class MapObject
    {
        public int ObjectId { get; set; }

        public string ObjectName { get; set; }

        public UserDTO ForUser { get; set; }

        public double Langtude { get; set; }
        public double Latitude { get; set; }
    }
}