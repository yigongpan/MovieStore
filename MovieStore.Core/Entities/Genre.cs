using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieStore.Core.Entities
{
    //1.using Data annotation
    //Table is an atrribute
    //after typing this, show potential fix and add using System.ComponentModel.DataAnnotations;
    [Table("Genre")]
    //Genre class represents our Domain Model, and we will create properties as table columns
    public class Genre
    {
        //By convention EF considers Id property in the entity class as Primary key
        public int Id { get; set; }
        //1.using Data annotation
        //use MaxLength attribute to change the length of datatype and Required to set not null
        [MaxLength(64)]
        [Required]
        public string Name { get; set; }
        public ICollection<MovieGenre> MovieGenres { get; set; }
    }
}
