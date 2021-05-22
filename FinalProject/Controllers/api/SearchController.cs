using AutoMapper;
using FinalProject.DTOs;
using FinalProject.Models;
using System;
using System.Device.Location;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FinalProject.Controllers.api
{
    public class SearchController : ApiController
    {
        private MyAppContext db = new MyAppContext();

        [HttpGet]
        public IHttpActionResult Get()
        {
            var mapObjects = db.Clinics.Select(Mapper.Map<Clinic, MapObject>);
            mapObjects.Union(db.Pharmacies.Select(Mapper.Map<Pharmacy, MapObject>));
            return Ok(mapObjects);
        }

        [HttpGet]
        public IHttpActionResult Get(GeoCoordinate coordinate)
        {
            var mapObjects = db.Clinics.Select(Mapper.Map<Clinic, MapObject>);
            //Add pharmacies
            mapObjects.Union(db.Pharmacies.Select(Mapper.Map<Pharmacy, MapObject>));
            //Order by distance to device location
            mapObjects.OrderBy(o => coordinate.GetDistanceTo(new GeoCoordinate(o.Langtude, o.Latitude)));

            return Ok(mapObjects);
        }

        [HttpGet]
        public IHttpActionResult GetClinics(byte? clinicType)
        {
            if (!clinicType.HasValue)
                return BadRequest();

            var mapObjects = db.Clinics.Where(c => c.ClinicTypeId == clinicType)
                .Select(Mapper.Map<Clinic, MapObject>);


            return Ok(mapObjects);
        }

        [HttpGet]
        public IHttpActionResult GetClinics(byte? clinicType, GeoCoordinate coordinate)
        {
            if (!clinicType.HasValue)
                return BadRequest();

            var mapObjects = db.Clinics.Where(c => c.ClinicTypeId == clinicType)
                .Select(Mapper.Map<Clinic, MapObject>);

            mapObjects.OrderBy(o => coordinate.GetDistanceTo(new GeoCoordinate(o.Langtude, o.Latitude)));

            return Ok(mapObjects);
        }

        [HttpGet]
        public IHttpActionResult GetPharmacies(GeoCoordinate coordinate)
        {
            var mapObjects = db.Pharmacies.Select(Mapper.Map<Pharmacy, MapObject>)
                .OrderBy(o => coordinate.GetDistanceTo(new GeoCoordinate(o.Langtude, o.Latitude)));

            return Ok(mapObjects);
        }
    }
}
