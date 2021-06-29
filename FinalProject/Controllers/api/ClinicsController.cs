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
    public class ClinicsController : ApiController
    {
        private MyAppContext db = new MyAppContext();

        #region Management
        [HttpGet]
        [Route("api/clinics")]
        [ManagerAuthentication]
        public IHttpActionResult Get()
        {
            return Ok(db.Clinics.Include(c => c.ClinicType).Include(c => c.ForUser).ToList().Select(Mapper.Map<Clinic, ClinicInfoDTO>));
        }

        [HttpGet]
        [Route("api/clinics/{id}")]
        [ManagerAuthentication]
        public IHttpActionResult Get(int id)
        {
            var clinic = db.Clinics.Include(c => c.ClinicType).FirstOrDefault(c => c.ClinicId == id);
            if (clinic == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<Clinic, ClinicDTO>(clinic));
        }
        #endregion

        #region For Clinics
        [HttpGet]
        [Route("api/clinics/patients/{id}")]
        [ClinicAuthentication]
        public IHttpActionResult GetPatients(int id)
        {
            if (!IsClinicIdBelongsToToken(id))
                return BadRequest();

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
        [ClinicAuthentication]
        public IHttpActionResult UpcommingAppointment(int id)
        {
            if (!IsClinicIdBelongsToToken(id))
                return BadRequest();

            var appointments = db.Appointments.Include(a => a.User)
                .Where(a => a.ClinicId == id)
                .Select(a => a).ToList();

            if (appointments.Count() == 0)
                return Ok(appointments);

            var result = appointments.Where(a => a.Date.Date.CompareTo(DateTime.Now.Date) >= 0).Select(a => new
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
        [ClinicAuthentication]
        public IHttpActionResult GetUnconirmedAppointments(int id)
        {
            if (!IsClinicIdBelongsToToken(id))
                return BadRequest();

            var appointments = db.Appointments.Include(a => a.User).Where(a => a.ClinicId == id).Select(a => a).ToList();

            //Returns Empty List
            if (appointments.Count() == 0)
                return Ok(appointments);

            var appointmentsToDelete = new List<Appointment>();

            //Delete All Appointments Not Confirmed Before Today
            foreach (var item in appointments)
            {
                if (item.Confirmed == false && item.Date.Date < DateTime.Now.Date)
                {
                    appointmentsToDelete.Add(item);
                }
            }

            foreach (var appointment in appointmentsToDelete)
            {
                db.Appointments.Remove(appointment);
                appointments.Remove(appointment);
            }
            db.SaveChanges();
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
        [ClinicAuthentication]
        public IHttpActionResult ConfirmAppointment(int id)
        {
            if (!IsAppointmentBelongsToClinic(id))
                return BadRequest();

            var appointment = db.Appointments.Where(a => a.Confirmed == false)
                .FirstOrDefault(a => a.AppointmentId == id);

            if (appointment == null)
                return NotFound();

            appointment.Confirmed = true;
            db.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("api/clinics/Appointments/{id}")]//Refuse appointment
        [ClinicAuthentication]
        public IHttpActionResult DeleteAppointment(int id)//Appointment id
        {
            if (!IsAppointmentBelongsToClinic(id))
                return BadRequest();

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
        [ClinicAuthentication]
        public IHttpActionResult DeleteCertificate(int id)//Certitfcete id
        {
            if (!IsCertificateBelongsToClinicDoctor(id))
                return BadRequest();

            var certificate = db.Certifcates.FirstOrDefault(c => c.CertifcateID == id);

            if (certificate == null)
                return NotFound();

            db.Certifcates.Remove(certificate);
            db.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("api/clinics/Reports/{id}")]
        [ClinicAuthentication]
        public IHttpActionResult GetReports(int id)
        {
            if (!IsUserHaveAppointmentInClinic(id))
                return BadRequest();

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
        [ClinicAuthentication]
        public IHttpActionResult GetAppointmentPrescription(string id)
        {
            if (id == null)
                return BadRequest();

            string[] compositeKey = id.Split(',');
            int appointmentId = int.Parse(compositeKey[0]);
            int medicineId = int.Parse(compositeKey[1]);

            if (!IsPrescriptionBelongsToClinic(appointmentId, medicineId))
                return BadRequest();

            var prescription = db.Prescriptions.FirstOrDefault(p => p.AppointmentId == appointmentId && p.MedicineId == medicineId);
            if (prescription == null)
                return NotFound();

            db.Prescriptions.Remove(prescription);
            db.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("api/clinics/Appointment/Prescriptions/{id}")]
        [ClinicAuthentication]
        public IHttpActionResult GetAppointmentPrescriptions(int id)//appointemnt id
        {
            if (!IsAppointmentBelongsToClinic(id))
                return BadRequest();

            var prescriptions = db.Prescriptions.Where(p => p.AppointmentId == id).Select(p => p);
            if (prescriptions.Count() == 0)
                return Ok(prescriptions);


            var result = from prescription in db.Prescriptions
                             //join appointment in db.Appointments on prescription.AppointmentId equals appointment.AppointmentId
                         join medicine in db.Medicines on prescription.MedicineId equals medicine.MedicineId
                         join type in db.MedicineTypes on medicine.MedicineTypeId equals type.MedicineTypeId
                         where prescription.AppointmentId == id
                         select new PrescriptionDTO
                         {
                             CompositId = prescription.AppointmentId + "," + prescription.MedicineId,
                             Dosage = prescription.Dosage,
                             Every = prescription.Every,
                             For = prescription.For,
                             MedicineNameAr = medicine.NameAR,
                             MedicineNameEn = medicine.NameEN,
                             MedicineType = type.MedicineTypeName,
                             Timespan = prescription.TimeSpan
                         };

            return Ok(result);
        }

        [HttpGet]
        [Route("api/clinics/Prescriptions/{id}")]
        [ClinicAuthentication]
        public IHttpActionResult GetPrescriptions(int id)//user id
        {
            if (!IsUserHaveAppointmentInClinic(id))
                return BadRequest();

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

        [HttpGet]
        [Route("api/clinics/Schedule/{id}")]
        [ClinicAuthentication]
        public IHttpActionResult Schedule(int id)//Clinic id
        {
            if (!IsClinicIdBelongsToToken(id))
                return BadRequest();

            var schedule = from s in db.Schedules where s.ClinicId == id select s;

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
        [ClinicAuthentication]
        public IHttpActionResult DeleteSchedule(int id)//Schedule id
        {
            if (!IsScheduleBelongsToClinic(id))
                return BadRequest();

            var schedule = db.Schedules.Where(s => s.ScheduleId == id)
                .FirstOrDefault();

            if (schedule == null)
                return NotFound();

            db.Schedules.Remove(schedule);
            db.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("api/clinics/vacation/{id}")]
        [ClinicAuthentication]
        public IHttpActionResult DeleteVacation(int id)//vacation id
        {
            if (!IsVacationBelongsToClinic(id))
                return BadRequest();

            var vacation = db.Vacations.Where(s => s.VacationId == id)
                .FirstOrDefault();

            if (vacation == null)
                return NotFound();

            if (vacation.ToDate < DateTime.Now)
                return BadRequest();

            db.Vacations.Remove(vacation);
            db.SaveChanges();
            return Ok();
        }

        //ToDo For Future
        //Check available Times at real Time
        //private AvailableAppointments RemoveAppointmentInDbFromAvailableAppointments(AvailableAppointments availableAppointments, IQueryable<Appointment> appointmentInDb)
        //{
        //    if (!appointmentInDb.Any())
        //        return availableAppointments;

        //    List<Appointment> q = appointmentInDb.ToList();
        //    for (int i = 0; i < availableAppointments.AvailableDays.Count; i++)
        //    {
        //        var appointment = availableAppointments.AvailableDays[i];

        //        for (int j = 0; j < q.Count(); j++)
        //        {
        //            if (q[j].Date.DayOfWeek == appointment.Day)
        //            {
        //                if (appointment.Times.Contains(q[j].Time))
        //                {
        //                    availableAppointments.AvailableDays[i].Times.Remove(q[j].Time);
        //                }
        //            }
        //        }
        //    }
        //    return availableAppointments;
        //}

        private AvailableAppointments GetAvailableAppointments(int clinicId, byte visitDuration)
        {
            IQueryable<Schedule> schedules = db.Schedules.Where(c => c.ClinicId == clinicId).Select(s => s);
            AvailableAppointments appointments = new AvailableAppointments { AvailableDays = new List<AvailableDay>() };
            foreach (var item in schedules)
            {
                AvailableDay day = new AvailableDay();
                day.Day = item.Day;

                var times = GetTimes(item.FromTime, item.ToTime, visitDuration);
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

        private List<string> GetTimes(string fromTime, string toTime, byte visitDuration)
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

        [HttpGet]
        [Route("api/clinics/vacations/{id}")]
        public IHttpActionResult GetVacations(int id)//Clinic id
        {
            if (!IsClinicIdBelongsToToken(id))
                return BadRequest();

            var vacations = db.Vacations.Where(v => v.ClinicId == id).ToList().Select(v => new
            {
                fromDate = v.FromDate.ToShortDateString(),
                toDate = v.ToDate.ToShortDateString(),
                status = v.Statue,
                vacId = v.VacationId
            });

            return Ok(vacations);
        }
        #endregion

        #region For Users
        [HttpGet]
        [Route("api/clinics/AvailableAppointment/{id}")]
        [UsersAuthentication]
        public IHttpActionResult AvailableAppointment(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var clinic = db.Clinics.Where(c => c.IsActiveClinic == true).FirstOrDefault(c => c.ClinicId == id);
            if (clinic == null)
                return NotFound();

            var availableAppointments = GetAvailableAppointments(clinic.ClinicId, clinic.VisitDuration);
            var appointmentInDb = db.Appointments.Where(c => c.ClinicId == clinic.ClinicId).Select(c => c);
            //availableAppointments = RemoveAppointmentInDbFromAvailableAppointments(availableAppointments, appointmentInDb);
            return Ok(availableAppointments);
        }

        //All Visits that made by specific user
        [HttpGet]
        [Route("api/clinics/Visits/{id}")]
        [UsersAuthentication]
        public IHttpActionResult GetAppointment(int? id)//userId
        {
            if (!id.HasValue)
                return BadRequest();

            var appointments = db.Appointments.Where(a => a.Confirmed && a.UserId == id)
                .ToList().Where(a => !string.IsNullOrEmpty(a.Symptoms))
                .Select(c => c);

            if (appointments.Count() == 0)
                return NotFound();

            var result = from a in db.Appointments
                         join c in db.Clinics on a.ClinicId equals c.ClinicId
                         join u in db.Users on c.UserId equals u.UserId
                         where a.UserId == id && a.Symptoms != null
                         select new VisitListItemDTO
                         {
                             ClinicId = a.ClinicId,
                             ClinicName = c.ClinicName,
                             Doctor = u.FirstName + " " + u.FatherName + " " + u.LastName,
                             Symptoms = a.Symptoms,
                             Date = a.Date
                         };

            return Ok(result);
        }

        //An confirmed visit info
        [HttpGet]
        [Route("api/clinics/VisitInfo/{id}")]
        [UsersAuthentication]
        public IHttpActionResult GetAppointmentInfo(int? id)//Visit Id
        {
            if (!id.HasValue)
                return BadRequest();

            var appointment = db.Appointments
                .Where(a => a.Confirmed && a.AppointmentId == id)
                .ToList().Where(a => !string.IsNullOrEmpty(a.Symptoms))
                .FirstOrDefault();

            if (appointment == null)
                return NotFound();

            var prescritions = db.Prescriptions.Include(p => p.Medicine).Where(p => p.AppointmentId == id).Select(Mapper.Map<Prescription, PrescriptionDTO>);

            var result = from prescription in db.Prescriptions
                             //join appointment in db.Appointments on prescription.AppointmentId equals appointment.AppointmentId
                         join medicine in db.Medicines on prescription.MedicineId equals medicine.MedicineId
                         join type in db.MedicineTypes on medicine.MedicineTypeId equals type.MedicineTypeId
                         where prescription.AppointmentId == id
                         select new PrescriptionDTO
                         {
                             CompositId = prescription.AppointmentId + "," + prescription.MedicineId,
                             Dosage = prescription.Dosage,
                             Every = prescription.Every,
                             For = prescription.For,
                             MedicineNameAr = medicine.NameAR,
                             MedicineNameEn = medicine.NameEN,
                             MedicineType = type.MedicineTypeName,
                             Timespan = prescription.TimeSpan
                         };

            var appointmentInfo = new AppointmentInfoDTO
            {
                Appointment = Mapper.Map<Appointment, AppointmentDTO>(appointment),
                Prescriptions = result.ToList()
            };

            return Ok(appointmentInfo);
        }

        [HttpGet]
        [Route("api/clinics/Serach/Id/{id}")]
        [UsersAuthentication]
        public IHttpActionResult SearchByName(int id)
        {
            var clinic = from c in db.Clinics
                         join ct in db.ClinicTypes on c.ClinicTypeId equals ct.ClinicTypeId
                         join u in db.Users on c.UserId equals u.UserId
                         join l in db.Locations on c.ClinicId equals l.ClinicId
                         join s in db.Schedules on c.ClinicId equals s.ClinicId
                         where c.IsActiveClinic == true && c.ClinicId == id
                         select new ClinicListItem
                         {
                             clinicId = c.ClinicId,
                             clinicName = c.ClinicName,
                             doctor = u.FirstName + " " + u.FatherName + " " + u.LastName,
                             clinicType = ct.ClinicTypeName,
                             location = l.Longtude + "," + l.Latitude
                         };

            if (clinic.Count() == 0)
                return NotFound();

            return Ok(clinic.FirstOrDefault());
        }

        [HttpGet]
        [Route("api/clinics/Serach/{name}")]
        [UsersAuthentication]
        public IHttpActionResult SearchByName(string name)
        {
            var clinics = db.Clinics
                .Where(c => c.IsActiveClinic == true);
            if (clinics.Count() == 0)
                return Ok(clinics.ToList());

            var list = GetClinicsList();
            var result = new List<ClinicListItem>();
            foreach (var item in list)
            {
                if (item.clinicName.Contains(name))
                    result.Add(item);
            }
            if (result.Count() == 0)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        [Route("api/clinics/Serach/type/{name}")]
        [UsersAuthentication]
        public IHttpActionResult SearchByTypeName(string name)
        {
            var clinics = db.Clinics
                .Where(c => c.IsActiveClinic == true);
            if (clinics.Count() == 0)
                return Ok(clinics.ToList());

            var list = GetClinicsList();
            var result = new List<ClinicListItem>();
            foreach (var item in list)
            {
                if (item.clinicType.Contains(name))
                    result.Add(item);
            }
            if (result.Count() == 0)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        [Route("api/clinics/Serach/doctor/{name}")]
        [UsersAuthentication]
        public IHttpActionResult SearchByTypeDoctorName(string name)
        {
            var clinics = db.Clinics
                .Where(c => c.IsActiveClinic == true);
            if (clinics.Count() == 0)
                return Ok(clinics.ToList());

            var list = GetClinicsList();
            var result = new List<ClinicListItem>();
            foreach (var item in list)
            {
                if (item.doctor.Contains(name))
                    result.Add(item);
            }
            if (result.Count() == 0)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        [Route("api/clinics/list")]
        [UsersAuthentication]
        public IHttpActionResult GetClinics()
        {
            var clinics = db.Clinics
                .Where(c => c.IsActiveClinic == true);

            if (clinics.Count() == 0)
                return Ok(clinics.ToList());//Empty list

            return Ok(GetClinicsList());
        }

        [HttpGet]
        [Route("api/clinics/list/today")]
        [UsersAuthentication]
        public IHttpActionResult GetClinicsToday()
        {
            var clinics = db.Clinics
                .Where(c => c.IsActiveClinic == true);

            if (clinics.Count() == 0)
                return Ok(clinics.ToList());//Empty list

            DayOfWeek today = DateTime.Now.DayOfWeek;
            //Get all clinics That have location and schedula at today
            var result = from c in db.Clinics
                         join ct in db.ClinicTypes on c.ClinicTypeId equals ct.ClinicTypeId
                         join u in db.Users on c.UserId equals u.UserId
                         join l in db.Locations on c.ClinicId equals l.ClinicId
                         join s in db.Schedules on c.ClinicId equals s.ClinicId
                         where c.IsActiveClinic == true && s.Day == today
                         select new
                         {
                             clinicId = c.ClinicId,
                             clinicName = c.ClinicName,
                             doctor = u.FirstName + " " + u.FatherName + " " + u.LastName,
                             clinicType = ct.ClinicTypeName,
                             location = l.Longtude + "," + l.Latitude,
                             activeHour = s.FromTime + " - " + s.ToTime
                         };

            return Ok(result.Distinct().ToList());
        }

        [HttpPost]
        [Route("api/clinics/MakeAppointment")]
        [UsersAuthentication]
        public IHttpActionResult MakeAppointment([FromBody]PickAppointment request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (request.date.Date < DateTime.Now.Date)
                return BadRequest("Invalid Date");

            if (!AppServices.IsValidTime(request.time))
                return BadRequest("Invalid time format");

            if (!IsValidDate(request.clinicId, request.date))
                return Ok("This Date Is Not Avaliable");

            bool timeExists = CheckTimeIfExists(request.clinicId, request.date, request.time);
            if (timeExists)
                return Ok("Time Exsits");

            Appointment Appointment = new Appointment
            {
                UserId = request.userId,
                ClinicId = request.clinicId,
                Date = request.date.Date,
                Time = request.time
            };

            db.Appointments.Add(Appointment);
            db.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("api/clinics/Appointments/d/{id}")]//Delete appointment
        [UsersAuthentication]
        public IHttpActionResult UserDeleteAppointment(int id)//Appointment id
        {
            if (!IsAppointmentForUser(id))
                return BadRequest();

            var appointment = db.Appointments.Where(a => a.Confirmed == false)
                .FirstOrDefault(a => a.AppointmentId == id);

            if (appointment == null)
                return NotFound();

            db.Appointments.Remove(appointment);
            db.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("api/clinics/Appointments/e/{id}")]//Delete appointment
        [UsersAuthentication]
        public IHttpActionResult UserEditAppointment(int id,AppointmentDTO appointmentDTO)//Appointment id
        {
            if (!IsAppointmentForUser(id))
                return BadRequest();

            if(AppServices.IsValidTime(appointmentDTO.Time))
                return BadRequest();

            var appointment = db.Appointments.Where(a => a.Confirmed == false)
                .FirstOrDefault(a => a.AppointmentId == id);

            if (appointment == null)
                return NotFound();
            try
            {

                appointment.Date = DateTime.Parse(appointmentDTO.Date);
                appointment.Time = appointmentDTO.Time;
                db.SaveChanges();
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }

        private List<ClinicListItem> GetClinicsList()
        {
            var result = from c in db.Clinics
                         join ct in db.ClinicTypes on c.ClinicTypeId equals ct.ClinicTypeId
                         join u in db.Users on c.UserId equals u.UserId
                         join l in db.Locations on c.ClinicId equals l.ClinicId
                         join s in db.Schedules on c.ClinicId equals s.ClinicId
                         where c.IsActiveClinic == true
                         select new ClinicListItem
                         {
                             clinicId = c.ClinicId,
                             clinicName = c.ClinicName,
                             doctor = u.FirstName + " " + u.FatherName + " " + u.LastName,
                             clinicType = ct.ClinicTypeName,
                             location = l.Longtude + "," + l.Latitude
                         };

            return result.Distinct().ToList();
        }

        private bool CheckTimeIfExists(int clinicId, DateTime date, string time)
        {
            var appointments = db.Appointments
                .Where(a => a.ClinicId == clinicId)
                .ToList().Where(a => a.Date.Date == date.Date)
                .Select(a => a);

            foreach (var item in appointments)
            {
                if (item.Time == time)
                    return true;
            }
            return false;
        }

        //Check if the day of week for given date is in clinic schedule
        private bool IsValidDate(int clinicId, DateTime date)
        {
            var schedule = db.Schedules.Where(a => a.ClinicId == clinicId).Select(a => a);
            if (schedule.Count() == 0)
                return false;

            foreach (var day in schedule)
            {
                if (day.Day == date.DayOfWeek)
                    return true;
            }

            return false;
        }

        #endregion

        public bool IsClinicIdBelongsToToken(int? clinicId)
        {
            int? tokenClinicId = AppServices.GetClinicIdFromToken(GetToken);

            if (tokenClinicId != clinicId)
                return false;
            return true;
        }

        public bool IsAppointmentBelongsToClinic(int? appointmentId)
        {
            int? tokenClinicId = AppServices.GetClinicIdFromToken(GetToken);
            var appointment = db.Appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
            if (appointment == null || tokenClinicId != appointment.ClinicId)
                return false;
            return true;
        }

        public bool IsScheduleBelongsToClinic(int? scheduleId)
        {
            int? tokenClinicId = AppServices.GetClinicIdFromToken(GetToken);
            var schedule = db.Schedules.FirstOrDefault(a => a.ScheduleId == scheduleId);
            if (schedule == null || tokenClinicId != schedule.ClinicId)
                return false;
            return true;
        }

        public bool IsVacationBelongsToClinic(int? vacationId)
        {
            int? tokenClinicId = AppServices.GetClinicIdFromToken(GetToken);
            var vacation = db.Vacations.FirstOrDefault(a => a.VacationId == vacationId);
            if (vacation == null || tokenClinicId != vacation.ClinicId)
                return false;
            return true;
        }

        public bool IsUserHaveAppointmentInClinic(int? userId)
        {
            int? tokenClinicId = AppServices.GetClinicIdFromToken(GetToken);
            var appointment = db.Appointments.FirstOrDefault(a => a.UserId == userId);//Have at least one appointment
            if (appointment == null || tokenClinicId != appointment.ClinicId)
                return false;
            return true;
        }

        public bool IsAppointmentForUser(int? appointmentId)
        {
            int? tokenUserId = AppServices.GetUserIdFromToken(GetToken);
            var appointment = db.Appointments.FirstOrDefault(a => a.AppointmentId == appointmentId 
                && a.UserId == tokenUserId);//Have at least one appointment

            if (appointment == null)
                return false;
            return true;
        }


        public bool IsPrescriptionBelongsToClinic(int appointmentId, int medicineId)
        {
            int? tokenClinicId = AppServices.GetClinicIdFromToken(GetToken);
            var prescrption = db.Prescriptions.Include(a => a.Appointment)
                .FirstOrDefault(a => a.AppointmentId == appointmentId
                && a.MedicineId == medicineId);

            if (prescrption == null || tokenClinicId != prescrption.Appointment.ClinicId)
                return false;
            return true;
        }

        public bool IsCertificateBelongsToClinicDoctor(int certificateId)
        {
            int? tokenClinicId = AppServices.GetClinicIdFromToken(GetToken);
            var certificate = db.Certifcates.FirstOrDefault(a => a.CertifcateID == certificateId);//Find certificate

            if (certificate == null)
                return false;

            var clinic = db.Clinics.FirstOrDefault(a => a.UserId == certificate.UserId);//Find certificate owner 
            if (clinic == null || clinic.ClinicId != tokenClinicId)//Check if user have clinic and it have the same id
                return false;

            return true;
        }

        public string GetToken => Request.Headers.Authorization.Parameter;
    }
}
