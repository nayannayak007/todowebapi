﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoAppWebAPI.Models
{
    public class ChangePasswordModel
    {
        public string Username { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
