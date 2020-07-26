using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStore.Core.Models.Request
{
    public class FavoriteRequestModel
    {
        public int MovieId { get; set; }
        public int UserId { get; set; }
    }
}
