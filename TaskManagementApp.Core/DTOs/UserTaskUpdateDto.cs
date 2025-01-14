﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementApp.Core.DTOs
{
    public class UserTaskUpdateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
