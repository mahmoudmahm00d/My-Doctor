using AutoMapper;
using FinalProject.Models;
using FinalProject.ViewModels;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FinalProject.DTOs;
using System.Web;
using System.IO;

namespace FinalProject.Controllers.api
{
    public class ClinicsController : ApiController
    {
        private MyAppContext db = new MyAppContext();

        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(db.Clinics.Include(c => c.ForUser).ToList().Select(Mapper.Map<Clinic, ClinicViewModel>));
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var clinic = db.Clinics.FirstOrDefault(c => c.ClinicId == id);
            if (clinic == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<Clinic, ClinicDTO>(clinic));
        }
        //TdDo
        //[HttpGet]
        //public IHttpActionResult GetAppointments(int id)
        //{
        //    var clinic = db.Clinics.FirstOrDefault(c => c.ClinicId == id);
        //    if (clinic == null)
        //    {
        //        return NotFound();
        //    }
        //    clinic.Appointments..Select();
        //    return Ok(Mapper.Map<Clinic, ClinicDTO>(clinic));
        //}

        [HttpPost]
        public IHttpActionResult AddDay(int clinicId, ScheduleDTO day)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var clinic = db.Clinics.FirstOrDefault(c => c.ClinicId == clinicId);
            clinic.Schedules.Add(Mapper.Map<ScheduleDTO, Schedule>(day));
            db.SaveChanges();

            return Ok(day);
        }

        [HttpPut]
        public IHttpActionResult Edit(int id, ClinicDTO clinic)
        {
            if (!ModelState.IsValid || clinic.ClinicId != id)
                return BadRequest("Invalid Clinic Properties");

            var clinicInDb = db.Clinics.Find(id);

            if (clinicInDb == null)
                return NotFound();

            Mapper.Map(clinic, clinicInDb);
            db.SaveChanges();
            return Ok(clinic);
        }

        [HttpPut]
        public IHttpActionResult EditLocation(int id, LocationDTO location)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var clinicToUpdate = db.Clinics.Find(id);

            if (clinicToUpdate == null)
                return NotFound();

            clinicToUpdate.Location = Mapper.Map<LocationDTO, Location>(location);

            db.SaveChanges();
            return Ok(location);
        }

        [HttpPut]
        public IHttpActionResult EditSchedule(int clinicId, int scheduleId, ScheduleDTO schedule)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var clinicToUpdate = db.Clinics.Find(clinicId);

            if (clinicToUpdate == null)
                return NotFound();

            var scheduleInDb = clinicToUpdate.Schedules.FirstOrDefault(s => s.ScheduleId == scheduleId);

            if (scheduleInDb == null)
                return NotFound();

            Mapper.Map(schedule, scheduleInDb);

            db.SaveChanges();
            return Ok(schedule);
        }

        [HttpDelete]
        public IHttpActionResult DeleteSchedule(int clinicId, int scheduleId, ScheduleDTO schedule)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var clinicToUpdate = db.Clinics.Find(clinicId);

            if (clinicToUpdate == null)
                return NotFound();

            var scheduleInDb = clinicToUpdate.Schedules.FirstOrDefault(s => s.ScheduleId == scheduleId);

            if (scheduleInDb == null)
                return NotFound();

            clinicToUpdate.Schedules.Remove(Mapper.Map(schedule, scheduleInDb));

            db.SaveChanges();
            return Ok(schedule);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var clinicInDb = db.Clinics.Find(id);

            if (clinicInDb == null)
                return NotFound();

            if (clinicInDb.Appointments == null)
            {
                db.Clinics.Remove(clinicInDb);
                db.SaveChanges();
                return Ok();
            }

            return BadRequest();
        }
    }
}
