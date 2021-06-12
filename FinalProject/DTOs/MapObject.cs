using System.Device.Location;

namespace FinalProject.DTOs
{
    public class MapObject : GeoCoordinate
    {
        public int ObjectId { get; set; }

        public string ObjectName { get; set; }

        public ClinicTypeDTO ClinicType { get; set; }

        public Doctor ForUser { get; set; }
    }
}