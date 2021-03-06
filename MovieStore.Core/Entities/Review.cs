﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieStore.Core.Entities
{
    public class Review
    {
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public decimal Rating { get; set; }
        [MaxLength]
        public string ReviewText { get; set; }
        public virtual User User { get; set; }
        public Movie Movie { get; set; }
    }
}
