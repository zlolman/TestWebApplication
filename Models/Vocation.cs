﻿using System;

namespace TestWebApplication.Models
{
    public class Vocation
    {
        public int id { get; set; }
        public int employeeId { get; set; }
        public DateTimeOffset startDate { get; set; }
        public DateTimeOffset endDate { get; set; }
    }
}