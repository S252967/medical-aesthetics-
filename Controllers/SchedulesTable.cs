using _0611_2.Data;
using _0611_2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _0611_2.Controllers
{
    public class SchedulesTable : Controller
    {
        private readonly _0611_2Context _context;

        public SchedulesTable(_0611_2Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(DateTime? startDate)
        {
<<<<<<< HEAD
            DateTime monthStart = startDate ?? DateTime.Today;
            ViewData["StartDate"] = monthStart;

            var model = await GetMonthlySchedules(monthStart);
            return View(model);
        }

        private async Task<Dictionary<(DateTime Date, int TimeSlot), List<(int DoctorId, string DoctorName)>>>
            GetMonthlySchedules(DateTime startDate)
        {
            var schedules = new Dictionary<(DateTime Date, int TimeSlot), List<(int DoctorId, string DoctorName)>>();
            var daysInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);

            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateTime(startDate.Year, startDate.Month, day);

                var appointments = await _context.Schedule
                    .Include(s => s.Doctor)
                    .Where(s => s.Date == date)
                    .ToListAsync();

                foreach (var appointment in appointments)
                {
                    var key = (date, appointment.TimeSlot);
                    if (!schedules.ContainsKey(key))
                    {
                        schedules[key] = new List<(int, string)>();
                    }
                    schedules[key].Add((appointment.DoctorId.Value, appointment.Doctor.DoctorName));
                }
            }

            return schedules;
=======
            DateTime startOfWeek = startDate ?? DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            ViewData["StartDate"] = startOfWeek;

            var doctorSchedules = await _context.Schedule
                .Include(ds => ds.Doctor)
                .Where(ds => ds.Date >= startOfWeek && ds.Date < startOfWeek.AddDays(7))
                .ToListAsync();

            var model = new Dictionary<(DateTime Date, int TimeSlot), List<(int DoctorId, string DoctorName)>>();
            foreach (var schedule in doctorSchedules)
            {
                var key = (schedule.Date.Date, schedule.TimeSlot);
                if (!model.ContainsKey(key))
                {
                    model[key] = new List<(int DoctorId, string DoctorName)>();
                }
                model[key].Add((schedule.DoctorId.Value, schedule.Doctor.DoctorName));
            }

            return View(model);
>>>>>>> 06a83381d66912a0c9f65193a3096182526fcd0e
        }

        public async Task<IActionResult> Appointment(int doctorId, DateTime date, int timeSlot)
        {
            var customers = await _context.Customer.ToListAsync();
            var model = new Appointment
            {
                Schedule = new Schedule
                {
                    DoctorId = doctorId,
                    Date = date,
                    TimeSlot = timeSlot
                },
                Customers = customers
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MakeAppointment(int doctorId, DateTime date, int timeSlot, int customerId)
        {
<<<<<<< HEAD
=======
            // 查找现有的预约项
>>>>>>> 06a83381d66912a0c9f65193a3096182526fcd0e
            var schedule = await _context.Schedule
                .FirstOrDefaultAsync(s => s.DoctorId == doctorId && s.Date == date && s.TimeSlot == timeSlot);

            if (schedule != null)
            {
<<<<<<< HEAD
                schedule.CustomerId = customerId;
=======
                // 更新现有的预约项
                schedule.CustomerId = customerId;

>>>>>>> 06a83381d66912a0c9f65193a3096182526fcd0e
                _context.Schedule.Update(schedule);
                await _context.SaveChangesAsync();
            }
            else
            {
<<<<<<< HEAD
=======
                // 如果找不到现有的预约项，可以选择抛出异常或处理错误
>>>>>>> 06a83381d66912a0c9f65193a3096182526fcd0e
                return NotFound("预约项未找到");
            }

            return RedirectToAction("Index");
        }
    }
}
