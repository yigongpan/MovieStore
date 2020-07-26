using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieStore.Core.Entities
{
    public class Role
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
