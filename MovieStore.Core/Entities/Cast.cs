using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieStore.Core.Entities
{
    public class Cast
    {
        public int Id { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
        [MaxLength]
        public string Gender { get; set; }
        [MaxLength]
        public string TmdbUrl { get; set; }
        [MaxLength(2084)]
        public string ProfilePath { get; set; }
        public ICollection<MovieCast> MovieCasts { get; set; }
    }
}
