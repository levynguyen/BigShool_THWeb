﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BigShool_ThWeb.Models
{
    public class Course
    {
        public int Id { get; set; }
        public bool IsCanceled { get; set; }

        public ApplicationUser Lecturer { get; set; }

        [Required]
        public string LecturerId { get; set; }

        [Required]
        [StringLength(255)]
        public string Place { get; set; }

        public DateTime DateTime { get; set; }
        public Category category { get; set; }

        [Required]
        public byte CategoryId { get; set; }
        [NotMapped]
        public bool isAttended { get; set; }

        [NotMapped]
        public bool isFollowed { get; set; }

    }
}