﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyPawsV2.DAL
{
    [Table("Appointment")]
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [ForeignKey("PetFile")]
        [Required]
        [DisplayName("Id de Mascota")]
        public int petFile { get; set; }

        [Required]
        [DisplayName("ID de Veterinario")]
        public string vetId { get; set; }

        [Required]
        [DisplayName("Id de Cliente")]
        public string ownerId { get; set; }

        [ForeignKey("Document")]
        [Required]
        [DisplayName("Id de Documento")]
        public int documentId { get; set; }

        public DateTime Date {get; set; }

        [DisplayName("Descripción")]
        public string description { get; set; }

        [DisplayName("Estado de Cita")]
        public string status { get; set; }


        [DisplayName("Diagnostico")]
        public string diagnostic { get; set; }

        [DisplayName("Observaciones Adicionales")]
        public string Additional { get; set; }


        public PetFile? petId { get; set; }

        public ApplicationUser? owner { get; set; }

        public ApplicationUser? veterinario { get; set; }

        public Document? document { get; set; }

        public ICollection<AppointmentInventory> AppointmentInventories { get; set; } = new List<AppointmentInventory>();












    }
}
