using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStore.Core.Models.Request
{
        public class PurchaseRequestModel
        {
            public PurchaseRequestModel()
            {
                PurchaseDate = DateTime.UtcNow;
                PurchaseNumber = Guid.NewGuid();
            }
            public int UserId { get; set; }
            public int MovieId { get; set; }
            public Guid? PurchaseNumber { get; set; }
            public decimal? TotalPrice { get; set; }
            public DateTime? PurchaseDate { get; set; }
        }
    }