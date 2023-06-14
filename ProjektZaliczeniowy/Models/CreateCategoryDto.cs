﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektZaliczeniowy.Models
{
    public class CreateCategoryDto
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
