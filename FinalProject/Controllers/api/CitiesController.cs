using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace FinalProject.Controllers.api
{
    public class CitiesController : ApiController
    {
        private MyAppContext db = new MyAppContext();
        // GET api/<controller>
        public IHttpActionResult Get()
        {
            return Ok(db.Cities.ToList());
        }

        // GET api/<controller>/5
        public IHttpActionResult Get(int id)
        {
            var city = db.Cities.FirstOrDefault(c => c.CityId == id);
            if (city == null)
                return NotFound();

            return Ok(db.Cities.ToList());
        }

        // POST api/<controller>
        public IHttpActionResult Post([Bind(Exclude = "CityId")]City city)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            db.Cities.Add(city);
            db.SaveChanges();
            return Created($"{Request.RequestUri}/{city.CityId}", city);
        }

        // PUT api/<controller>/5
        public IHttpActionResult Put(int id, City city)
        {
            var cityInDb = db.Cities.FirstOrDefault(c => c.CityId == id);
            if (cityInDb == null)
                return NotFound();

            cityInDb.CityName = city.CityName;
            cityInDb.IsActiveCity = city.IsActiveCity;
            db.SaveChanges();

            return Ok(cityInDb);
        }

        // DELETE api/<controller>/5
        public IHttpActionResult Delete(int id)
        {
            City city = db.Cities.Find(id);
            if (city == null)
                return NotFound();

            if (city.Locations == null)
            {
                db.Cities.Remove(city);
                db.SaveChanges();
                return Ok(city);
            }
            return BadRequest("Can't delete city with location children");
        }
    }
}