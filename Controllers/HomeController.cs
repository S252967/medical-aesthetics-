<<<<<<< HEAD
﻿using _0611_2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _0611_2.Data;
using _0611_2.Models;
>>>>>>> 06a83381d66912a0c9f65193a3096182526fcd0e

namespace _0611_2.Controllers
{
    public class HomeController : Controller
    {
<<<<<<< HEAD
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
=======
        private readonly _0611_2Context _context;

        public HomeController(_0611_2Context context)
        {
            _context = context;
        }

        // GET: Home/Index
        public async Task<IActionResult> Index(DateTime? startDate)
        {
            if (!startDate.HasValue)
            {
                startDate = DateTime.Today;
            }

            var endDate = startDate.Value.AddMonths(1).AddDays(-1);

            // Retrieve schedules within the month range
            var schedules = await _context.Schedule
                .Include(s => s.Doctor)
                .Where(s => s.Date >= startDate && s.Date <= endDate)
                .ToListAsync();

            // Group schedules by date and time slot
            var scheduleDict = new Dictionary<(DateTime Date, int TimeSlot), List<(int DoctorId, string DoctorName)>>();

            foreach (var schedule in schedules)
            {
                var key = (schedule.Date, (int)schedule.TimeSlot);
                if (!scheduleDict.ContainsKey(key))
                {
                    scheduleDict[key] = new List<(int DoctorId, string DoctorName)>();
                }
                scheduleDict[key].Add((schedule.Doctor.DoctorId, schedule.Doctor.DoctorName));
            }

            ViewData["StartDate"] = startDate;

            return View(scheduleDict);
>>>>>>> 06a83381d66912a0c9f65193a3096182526fcd0e
        }
    }
}
