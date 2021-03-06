﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESMS.ViewModels.System.Employees
{
    public class EmpUpdateRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}