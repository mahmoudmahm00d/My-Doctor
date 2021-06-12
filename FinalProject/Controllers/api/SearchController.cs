using AutoMapper;
using FinalProject.DTOs;
using FinalProject.Models;
using System;
using System.Data.Entity;
using System.Device.Location;
using System.Linq;
using System.Web.Http;

namespace FinalProject.Controllers.api
{
    public class SearchController : ApiController
    {
        private MyAppContext db = new MyAppContext();

        [HttpGet]
        [Route("api/search")]
        public IHttpActionResult Get()
        {
            var mapObjects = db.Clinics.Include(c => c.Location).Include(c => c.ForUser).Include(c => c.ClinicType).Select(Mapper.Map<Clinic, MapObject>);
            mapObjects.Union(db.Pharmacies.Select(Mapper.Map<Pharmacy, MapObject>));
            return Ok(mapObjects);
        }

        [HttpGet]
        [Route("api/search")]
        public IHttpActionResult GetObjects([FromBody]GeoCoordinate coordinate, int distance = 5000)
        {
            var mapObjects = db.Clinics.Include(c => c.Location).Include(c => c.ForUser).Include(c => c.ClinicType).Select(Mapper.Map<Clinic, MapObject>).ToList();
            var pharmacies = db.Pharmacies.Include(p => p.ForUser).Select(Mapper.Map<Pharmacy, MapObject>).ToList();
            mapObjects.Union(pharmacies);
            foreach (var item in mapObjects)
            {
                var distanceFromCoordinate = Math.Round(item.GetDistanceTo(coordinate));
                if (distanceFromCoordinate > 5000)
                    mapObjects.Remove(item);
            }

            return Ok(mapObjects);
        }

        //[HttpGet]
        //[Route("api/search/nearToMe")]
        //public IHttpActionResult Get([FromBody]GeoCoordinate coordinate)
        //{
        //    var mapObjects = db.Clinics.Select(Mapper.Map<Clinic, MapObject>);
        //    //Add pharmacies
        //    mapObjects.Union(db.Pharmacies.Select(Mapper.Map<Pharmacy, MapObject>));
        //    //Order by distance to device location
        //    mapObjects.OrderBy(o => coordinate.GetDistanceTo(new GeoCoordinate(o.Longitude, o.Latitude)));

        //    return Ok(mapObjects);
        //}

        [HttpGet]
        [Route("api/search/clinics")]
        public IHttpActionResult GetClinics()
        {
            var mapObjects = db.Clinics.Include(c => c.Location).Include(c => c.ForUser).Include(c => c.ClinicType)
                .Select(Mapper.Map<Clinic, MapObject>);

            return Ok(mapObjects);
        }

        [HttpGet]
        [Route("api/search/clinics/{clinicType}")]
        public IHttpActionResult GetClinics(byte? clinicType)
        {
            if (!clinicType.HasValue)
                return BadRequest();

            var mapObjects = db.Clinics.Where(c => c.ClinicTypeId == clinicType).Include(c => c.ClinicType)
                .Select(Mapper.Map<Clinic, MapObject>);

            return Ok(mapObjects);
        }

        [HttpGet]
        [Route("api/search/clinics/{clinicType}")]
        public IHttpActionResult GetClinics(byte? clinicType, [FromBody] GeoCoordinate coordinate)
        {
            if (!clinicType.HasValue)
                return BadRequest();

            var mapObjects = db.Clinics.Where(c => c.ClinicTypeId == clinicType)
                .Select(Mapper.Map<Clinic, MapObject>);

            mapObjects.OrderBy(o => coordinate.GetDistanceTo(new GeoCoordinate(o.Longitude, o.Latitude)));

            return Ok(mapObjects);
        }

        [HttpGet]
        [Route("api/search/pharmacies")]
        public IHttpActionResult GetPharmacies(GeoCoordinate coordinate)
        {
            var mapObjects = db.Pharmacies.Select(Mapper.Map<Pharmacy, MapObject>)
                .OrderBy(o => coordinate.GetDistanceTo(new GeoCoordinate(o.Longitude, o.Latitude)));

            return Ok(mapObjects);
        }

        [HttpGet]
        [Route("api/search/pharmacies")]
        public IHttpActionResult GetPharmacies()
        {
            var mapObjects = db.Pharmacies.Select(Mapper.Map<Pharmacy, MapObject>).ToList();
            var pharmacies = mapObjects.OrderBy(o => o.GetDistanceTo(new GeoCoordinate(o.Longitude, o.Latitude)));

            return Ok(mapObjects);
        }
    }
}
