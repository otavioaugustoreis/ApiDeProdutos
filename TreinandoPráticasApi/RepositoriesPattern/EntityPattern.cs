﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TreinandoPráticasApi.Repositories
{
    public class EntityPattern
    {
        [Key]
        [Column("pk_id")]
        public int? Id { get; set; }

        [Required]
        [Column("dh_inclusao")]
        public DateTime DateOfInclusion { get; set; } = DateTime.Now;

        public EntityPattern() { }

        protected EntityPattern(int id, DateTime dateOfInclusion)
        {
            Id = id;
            DateOfInclusion = dateOfInclusion;
        }
    }
}
