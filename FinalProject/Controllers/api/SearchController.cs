using AutoMapper;
using FinalProject.Authentication;
using FinalProject.DTOs;
using FinalProject.Models;
using System;
using System.Data.Entity;
using System.Device.Location;
using System.Linq;
using System.Web.Http;

namespace FinalProject.Controllers.api
{
    //For map
    public class SearchController : ApiController
    {
        private MyAppContext db = new MyAppContext();

        [HttpGet]
        [Route("api/search/clinics/{name}")]
        [UsersAuthentication]
        public IHttpActionResult Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest();

            var clinic = db.Clinics.FirstOrDefault();
            var mapObjects = db.Clinics.Include(c => c.Location)
                .Include(c => c.ForUser)
                .Include(c => c.ClinicType)
                .Where(c=>c.Location != null)
                .Select(Mapper.Map<Clinic, MapObject>);

            return Ok(mapObjects);
        }

        [HttpGet]
        [Route("api/search")]
        [UsersAuthentication]
        public IHttpActionResult GetObjects(GeoCoordinate coordinate, int distance = 5000)
        {
            if (coordinate == null)
                return BadRequest();

            var mapObjects = db.Clinics.Include(c => c.Location)
                .Include(c => c.ForUser)
                .Include(c => c.ClinicType)
                .Where(c=>c.Location != null)
                .Select(Mapper.Map<Clinic, MapObject>).ToList();

            var pharmacies = db.Pharmacies.Include(p => p.ForUser)
                .Where(p=>p.Longtude != 0)
                .Select(Mapper.Map<Pharmacy, MapObject>).ToList();

            mapObjects.AddRange(pharmacies);

            var result = mapObjects.ToList();
            foreach (var item in mapObjects)
            {
                var distanceFromCoordinate = Math.Round(item.GetDistanceTo(coordinate));
                if (distanceFromCoordinate <= distance)
                    result.Remove(item);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("api/search/clinics")]
        [UsersAuthentication]
        public IHttpActionResult GetClinics()
        {
            var mapObjects = db.Clinics.Include(c => c.Location)
                .Include(c => c.ForUser)
                .Include(c => c.ClinicType)
                .Where(c => c.Location != null)
                .Select(Mapper.Map<Clinic, MapObject>);

            return Ok(mapObjects);
        }

        [HttpGet]
        [Route("api/search/clinics/{clinicType}")]
        [UsersAuthentication]
        public IHttpActionResult GetClinics(byte? clinicType)
        {
            if (!clinicType.HasValue)
                return BadRequest();

            var mapObjects = db.Clinics.Include(c => c.ForUser)
                .Include(c => c.ClinicType)
                .Where(c => c.ClinicTypeId == clinicType 
                && c.Location != null)
                .Select(Mapper.Map<Clinic, MapObject>);

            return Ok(mapObjects);
        }

        [HttpGet]
        [Route("api/search/clinics/{clinicType}")]
        [UsersAuthentication]
        public IHttpActionResult GetClinics(byte? clinicType, GeoCoordinate coordinate)
        {
            if (!clinicType.HasValue)
                return BadRequest();

            var mapObjects = db.Clinics
                .Where(c => c.ClinicTypeId == clinicType 
                && c.Location != null)
                .Select(Mapper.Map<Clinic, MapObject>)
                .ToList();

            mapObjects.OrderBy(o => coordinate.GetDistanceTo(new GeoCoordinate(o.Longitude, o.Latitude)));

            return Ok(mapObjects);
        }

        [HttpGet]
        [Route("api/search/pharmacies")]
        [UsersAuthentication]
        public IHttpActionResult GetPharmacies(GeoCoordinate coordinate)
        {
            var mapObjects = db.Pharmacies
                .Where(p=>p.Longtude != 0)
                .Select(Mapper.Map<Pharmacy, MapObject>)
                .ToList()
                .OrderBy(o => coordinate.GetDistanceTo(new GeoCoordinate(o.Longitude, o.Latitude)));

            return Ok(mapObjects);
        }

        [HttpGet]
        [Route("api/search/pharmacies")]
        [UsersAuthentication]
        public IHttpActionResult GetPharmacies()
        {
            var mapObjects = db.Pharmacies.Select(Mapper.Map<Pharmacy, MapObject>).ToList();
            var pharmacies = mapObjects
                .ToList()
                .OrderBy(o => o.GetDistanceTo(new GeoCoordinate(o.Longitude, o.Latitude)));

            return Ok(mapObjects);
        }

        public string GetToken => Request.Headers.Authorization.Parameter;
    }
}
