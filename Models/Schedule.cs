using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace _0611_2.Models
{
    public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; } // 預約日期

        public int TimeSlot { get; set; } // 預約時段

        public int? DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; } // Changed to virtual single Doctor

        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; } // Changed to virtual single Customer
    }

    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; } // Added relationship to Schedules

    }

    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; } // Added relationship to Schedules
       

    }
    public class Appointment///預約
    {
        [Key]
        public int AppointmentId { get; set; }

        public int? DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; } // Changed to virtual single Doctor

        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; } // Changed to virtual single Customer

        public Schedule Schedule { get; set; } // Added Schedule relationship
        public List<Customer> Customers { get; set; } // Added list of customers for appointment

    }
    public enum TimeSlot
    {
        [Display(Name = "早上")]
        Morning = 1,

        [Display(Name = "中午")]
        Afternoon = 2,

        [Display(Name = "晚上")]
        Night = 3,
    }
}

