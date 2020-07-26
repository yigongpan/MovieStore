using System;
using MovieStore.Core.Entities;

namespace MovieStore.Core.Models.Response
{
    public class MovieDetailsModel
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public Guid? PurchaseNumber { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public bool CheckPurchased { get; set; }
        public bool CheckFavorited { get; set; }
        public bool CheckReviewed { get; set; }
        public Movie Movie { get; set; }
    }

}
