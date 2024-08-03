using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test001.Models
{

        public class Staff
        {
            [Key] public int Id { get; set; }
        
            public string Username { get; set; }
            public string Password { get; set; }
    }
    
}
