using AutoMapper;
using FinalProject.Authentication;
using FinalProject.DTOs;
using FinalProject.Models;
using FinalProject.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace FinalProject.Controllers.api
{
    //todo
    //add Authorization
    public class ClinicsController : ApiController
    {
        private MyAppContext db = new MyAppContext();

        #region Management
        [HttpGet]
        [Route("api/clinics")]
        //[UsersAuthentication]
        public IHttpActionResult Get()
        {
            return Ok(db.Clinics.Include(c => c.ClinicType).Include(c=> c.ForUser).ToList().Select(Mapper.Map<Clinic, ClinicInfoDTO>));
        }

        [HttpGet]
        [Route("api/clinics/{id}")]
        public IHttpActionResult Get(int id)
        {
            var clinic = db.Clinics.Include(c=> c.ClinicType).FirstOrDefault(c => c.ClinicId == id);
            if (clinic == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<Clinic, ClinicDTO>(clinic));
        }
        #endregion

        [HttpGet]
        [Route("api/clinics/patients/{id}")]
        public IHttpActionResult GetPatients(int id)
        {
            var patient = db.Appointments.Include(a => a.User).Where(a => a.ClinicId == id).Select(a => a).ToList();
            if (patient.Count() == 0)
                return Ok(patient);

            var res = patient.Select(a => new
            {
                appointmentId = a.AppointmentId,
                patientName = $"{a.User.FirstName} {a.User.LastName}",
                date = a.Date.ToShortDateString()
            });

            return Ok(res);
        }
        [HttpGet]
        [Route("api/clinics/UpcommingAppointment/{id}")]
        public IHttpActionResult UpcommingAppointment(int id)
        {
            var appointments = db.Appointments.Include(a => a.User)
                .Where(a => a.ClinicId == id)
                .Select(a => a).ToList();
            
            if (appointments.Count() == 0)
                return Ok(appointments);

            var result = appointments.Where(a => a.Date.CompareTo(DateTime.Now) >= 0).Select(a => new
            {
                appointmentId = a.AppointmentId,
                patientName = $"{a.User.FirstName} {a.User.LastName}",
                date = a.Date.ToShortDateString(),
                time = a.Time
            });

            return Ok(result);
        }

        [HttpGet]
        [Route("api/clinics/UnconfirmedAppointments/{id}")]
        public IHttpActionResult GetUnconirmedAppointments(int id)
        {
            var appointments = db.Appointments.Include(a => a.User).Where(a => a.ClinicId == id).Select(a => a).ToList();

            //Returns Empty List
            if (appointments.Count() == 0)
                return Ok(appointments);
            //Delete All Appointments Not Confirmed Before Today
            foreach (var item in appointments)
            {
                if(item.Date < DateTime.Now)
                {
                    db.Appointments.Remove(item);
                    appointments.Remove(item);
                }
            }
            //Foramting Result
            var result = appointments.Where(a => a.Confirmed == false).Select(a => new
            {
                appointmentId = a.AppointmentId,
                patientName = $"{a.User.FirstName} {a.User.LastName}",
                date = a.Date.ToShortDateString(),
                time = a.Time
            });
            return Ok(result);
        }

        [HttpPost]
        [Route("api/clinics/ConfirmedAppointments/{id}")]
        public IHttpActionResult ConfirmAppointment(int id)
        {
            var appointment = db.Appointments.Where(a => a.Confirmed == false)
                .FirstOrDefault(a => a.AppointmentId == id);

            if (appointment == null)
                return NotFound();

            appointment.Confirmed = true;
            db.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("api/clinics/Appointments/{id}")]
        public IHttpActionResult DeleteAppointment(int id)
        {
            var appointment = db.Appointments.Where(a => a.Confirmed == false)
                .FirstOrDefault(a => a.AppointmentId == id);

            if (appointment == null)
                return NotFound();

            db.Appointments.Remove(appointment);
            db.SaveChanges();
            return Ok();
        }


        [HttpDelete]
        [Route("api/doctors/certificates/{id}")]
        public IHttpActionResult DeleteCertificate( int id)
        {
            var certificate = db.Certifcates.FirstOrDefault(c=>c.CertifcateID == id);

            if (certificate == null)
                return NotFound();

            db.Certifcates.Remove(certificate);
            db.SaveChanges();
            return Ok();
        }
        [HttpGet]
        [Route("api/clinics/Reports/{id}")]
        public IHttpActionResult GetReports(int id)
        {
            var reports = db.Appointments.Include(a => a.User).Where(a => a.UserId == id).Select(a => a).ToList();
            if (reports.Count() == 0)
                return Ok(reports);

            var result = reports.Select(a => new
            {
                appointmentId = a.AppointmentId,
                date = a.Date,
                symptoms = a.Symptoms,
                remarks = a.Remarks
            });

            return Ok(result);
        }

        [HttpDelete]
        [Route("api/clinics/Appointment/Prescriptions/{id}")]
        public IHttpActionResult GetAppointmentPrescriptions(string id)
        {
            string[] compositeKey = id.Split(',');
            int appointmentId = int.Parse(compositeKey[0]);
            int medicineId = int.Parse(compositeKey[1]);
            var prescription = db.Prescriptions.FirstOrDefault(p => p.AppointmentId == appointmentId && p.MedicineId == medicineId);
            if (prescription == null)
                return NotFound();

            db.Prescriptions.Remove(prescription);
            db.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("api/clinics/Appointment/Prescriptions/{id}")]
        public IHttpActionResult GetAppointmentPrescriptions(int id)
        {
            var prescriptions = db.Prescriptions.Include(p => p.Appointment).Where(p => p.AppointmentId == id).Select(p => p).ToList();
            if (prescriptions.Count() == 0)
                return Ok(prescriptions);

            var result = prescriptions.Select(p => new
            {
                compositId = $"{p.AppointmentId},{p.MedicineId}",
                visitDate = p.Appointment.Date,
                medicineNameAr = p.Medicine.NameAR,
                medicineNameEn = p.Medicine.NameEN,
                medicineType = p.Medicine.MedicineType.MedicineTypeName,
                dosage = p.Dosage,
                every = p.Every,
                @for = p.For,
                timespan = p.TimeSpan
            });

            return Ok(result);
        }

        [HttpGet]
        [Route("api/clinics/Prescriptions/{id}")]
        public IHttpActionResult GetPrescriptions(int id)
        {
            var prescriptions = db.Prescriptions.Include(p => p.Appointment).Where(p => p.Appointment.UserId == id).Select(p => p).ToList();
            if (prescriptions.Count() == 0)
                return Ok(prescriptions);

            var result = prescriptions.Select(p => new
            {
                compositId = $"{p.AppointmentId},{p.MedicineId}",
                visitDate = p.Appointment.Date,
                medicineNameAr = p.Medicine.NameAR,
                medicineNameEn = p.Medicine.NameEN,
                medicineType = p.Medicine.MedicineType.MedicineTypeName,
                dosage = p.Dosage,
                every = p.Every,
                @for = p.For,
                timespan = p.TimeSpan
            });

            return Ok(result);
        }

        //[HttpPost]
        //public IHttpActionResult AddDay(int clinicId, ScheduleDTO day)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    var clinic = db.Clinics.FirstOrDefault(c => c.ClinicId == clinicId);
        //    clinic.Schedules.Add(Mapper.Map<ScheduleDTO, Schedule>(day));
        //    db.SaveChanges();

        //    return Ok(day);
        //}

        [HttpGet]
        [Route("api/clinics/Schedule/{id}")]
        public IHttpActionResult Schedule(int id)
        {
            var schedule = from s in db.Schedules where s.ClinicId == id select s;
            //var schedule = db.Schedules.Where(s=> s.ClinicId == id ).Select(s=>s).ToList();

            if (schedule.Count() == 0)
                return Ok(schedule);

            var result = from s in db.Schedules
                         where s.ClinicId == id 
                         select new
                         {
                             scheduleId = s.ScheduleId,
                             day = s.Day,
                             fromTime = s.FromTime,
                             toTime = s.ToTime
                         };

            return Ok(result);
        }

        [HttpDelete]
        [Route("api/clinics/Schedule/{id}")]
        public IHttpActionResult DeleteSchedule(int id)
        {
            var schedule = db.Schedules.Where(s => s.ScheduleId == id)
                .FirstOrDefault();

            if (schedule == null)
                return NotFound();


            db.Schedules.Remove(schedule);
            db.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("api/clinics/AvailableAppointment/{id}")]
        [UsersAuthentication]
        public IHttpActionResult AvailableAppointment(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var clinic = db.Clinics.Where(c => c.IsActiveClinic == false).FirstOrDefault(c => c.ClinicId == id);
            if (clinic == null)
                return NotFound();

            var availableAppointments = GetAvailableAppointments(clinic.ClinicId, clinic.VisitDuration);
            var appointmentInDb = db.Appointments.Where(c => c.ClinicId == clinic.ClinicId).Select(c => c);

            availableAppointments = RemoveAppointmentInDbFromAvailableAppointments(availableAppointments, appointmentInDb);

            return Ok(availableAppointments);
        }

        private AvailableAppointments RemoveAppointmentInDbFromAvailableAppointments(AvailableAppointments availableAppointments, IQueryable<Appointment> appointmentInDb)
        {
            if (!appointmentInDb.Any())
                return availableAppointments;

            List<Appointment> q = appointmentInDb.ToList();
            for (int i = 0; i < availableAppointments.AvailableDays.Count; i++)
            {
                var appointment = availableAppointments.AvailableDays[i];

                for (int j = 0; j < q.Count(); j++)
                {
                    if (q[j].Date.DayOfWeek == appointment.Day)
                    {
                        if (appointment.Times.Contains(q[j].Time))
                        {
                            availableAppointments.AvailableDays[i].Times.Remove(q[j].Time);
                        }
                    }
                }
            }
            return availableAppointments;
        }


        private AvailableAppointments GetAvailableAppointments(int clinicId, byte visitDuration)
        {
            IQueryable<Schedule> schedules = db.Schedules.Where(c => c.ClinicId == clinicId).Select(s => s);
            AvailableAppointments appointments = new AvailableAppointments { AvailableDays = new List<AvailableDay>() };
            foreach (var item in schedules)
            {
                AvailableDay day = new AvailableDay();
                day.Day = item.Day;

                var times = GetTime(item.FromTime, item.ToTime, visitDuration);
                AvailableDay dayInAvailable = null;
                if (appointments.AvailableDays.Any())
                    dayInAvailable = appointments.AvailableDays.Where(d => d.Day == item.Day).FirstOrDefault();//If there is any repeated day

                if (dayInAvailable != null)
                {
                    day.Times = dayInAvailable.Times.Union(times).ToList<string>();
                    appointments.AvailableDays.Remove(dayInAvailable);
                    appointments.AvailableDays.Add(day);
                }
                else
                {
                    day.Times = times;
                    appointments.AvailableDays.Add(day);
                }
            }
            return appointments;
        }

        private List<string> GetTime(string fromTime, string toTime, byte visitDuration)
        {
            List<string> times = new List<string>();
            Time from = new Time(fromTime);
            Time to = new Time(toTime);
            int duration = Time.Differance(from, to);
            int count = duration / visitDuration;
            for (int i = 0; i <= count; i++)
            {
                times.Add(from.ToString());
                from = from + visitDuration;
            }
            return times;
        }

        #region For Users
        [HttpPost]
        [Route("api/clinics/MakeAppointment")]
        [UsersAuthentication]
        //public IHttpActionResult MakeAppointment([FromBody]int? userId, [FromBody]int? clinicId, [FromBody] DateTime date, [FromBody] string time)
        public IHttpActionResult MakeAppointment([FromBody]PickAppointment request)
        {
            if (!request.userId.HasValue || !request.clinicId.HasValue || request.date == null || request.time == null)
                return BadRequest();

            if (request.date.Date < DateTime.Now.Date)
                return BadRequest("Invalid Date");

            if (!AppServices.IsValidTime(request.time))
                return BadRequest("Invalid time format");

            bool timeExists = CheckTimeIfExists(request.clinicId.Value, request.date, request.time);
            if (timeExists)
                return BadRequest("Invalid Time");

            Appointment Appointment = new Appointment { UserId = request.userId.Value, ClinicId = request.clinicId.Value, Date = request.date.Date, Time = request.time, Symptoms = "" };
            db.Appointments.Add(Appointment);
            db.SaveChanges();
            return Ok();
        }

        private bool CheckTimeIfExists(int clinicId, DateTime date, string time)
        {
            IQueryable<Appointment> appointments = db.Appointments.Where(a => a.ClinicId == clinicId).Select(a => a);
            appointments = from a in appointments where a.Date == date select a;
            foreach (var item in appointments)
            {
                if (item.Time == time)
                    return true;
            }
            return false;
        }

        #endregion

        //[HttpPut]
        //public IHttpActionResult Edit(int id, ClinicDTO clinic)
        //{
        //    if (!ModelState.IsValid || clinic.ClinicId != id)
        //        return BadRequest("Invalid Clinic Properties");

        //    var clinicInDb = db.Clinics.Find(id);

        //    if (clinicInDb == null)
        //        return NotFound();

        //    Mapper.Map(clinic, clinicInDb);
        //    db.SaveChanges();
        //    return Ok(clinic);
        //}

        //[HttpPut]
        //public IHttpActionResult EditLocation(int id, LocationDTO location)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    var clinicToUpdate = db.Clinics.Find(id);

        //    if (clinicToUpdate == null)
        //        return NotFound();

        //    clinicToUpdate.Location = Mapper.Map<LocationDTO, Location>(location);

        //    db.SaveChanges();
        //    return Ok(location);
        //}

        //[HttpPut]
        //public IHttpActionResult EditSchedule(int clinicId, int scheduleId, ScheduleDTO schedule)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    var clinicToUpdate = db.Clinics.Find(clinicId);

        //    if (clinicToUpdate == null)
        //        return NotFound();

        //    var scheduleInDb = clinicToUpdate.Schedules.FirstOrDefault(s => s.ScheduleId == scheduleId);

        //    if (scheduleInDb == null)
        //        return NotFound();

        //    Mapper.Map(schedule, scheduleInDb);

        //    db.SaveChanges();
        //    return Ok(schedule);
        //}

        //[HttpDelete]
        //public IHttpActionResult DeleteSchedule(int clinicId, int scheduleId, ScheduleDTO schedule)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    var clinicToUpdate = db.Clinics.Find(clinicId);

        //    if (clinicToUpdate == null)
        //        return NotFound();

        //    var scheduleInDb = clinicToUpdate.Schedules.FirstOrDefault(s => s.ScheduleId == scheduleId);

        //    if (scheduleInDb == null)
        //        return NotFound();

        //    clinicToUpdate.Schedules.Remove(Mapper.Map(schedule, scheduleInDb));

        //    db.SaveChanges();
        //    return Ok(schedule);
        //}

        //[HttpDelete]
        //public IHttpActionResult Delete(int id)
        //{
        //    var clinicInDb = db.Clinics.Find(id);

        //    if (clinicInDb == null)
        //        return NotFound();

        //    if (clinicInDb.Appointments == null)
        //    {
        //        db.Clinics.Remove(clinicInDb);
        //        db.SaveChanges();
        //        return Ok();
        //    }

        //    return BadRequest();
        //}
    }
}
