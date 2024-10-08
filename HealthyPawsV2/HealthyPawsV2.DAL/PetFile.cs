﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HealthyPawsV2.DAL
{
    [Table("PetFile")]
    public class PetFile
    {
        [Key]
        [DisplayName("Id Mascota")]
        public int petFileId { get; set; }

        [ForeignKey("PetBreed")]
        [DisplayName("Raza de Mascota")]
        public int petBreedId { get; set; }

        [DisplayName("Dueño")] 
        public string idNumber { get; set; }   

        [DisplayName("Nombre de Mascota")]
        public string name { get; set; }

        [DisplayName("Tipo de Mascota")]
        public int petTypeId { get; set; }

        [DisplayName("Género")]
        public string gender { get; set; }

        [DisplayName("Edad")]
        public int age { get; set; }

        [DisplayName("Peso")]
        public double weight { get; set; }


        [DisplayName("Fecha de Creación")]
        public DateTime creationDate { get; set; }
        [DisplayName("Fecha de Creación")]
        public string formattedCreationDate
        {
            get
            {
                return creationDate.ToString("dd/MM/yyyy HH:mm tt");
            }
        }

        [DisplayName("Historial de Vacunas")]
        public string vaccineHistory { get; set; }

        [DisplayName("Castrado")]
        public string  castration { get; set; }

        [DisplayName("Descripción")]
        public string description { get; set; }

        [DisplayName("Estado")]
        public bool status { get; set; }


        //Here we are pulling the collections from the root entitys
        public PetBreed? PetBreed { get; set; }

        public ApplicationUser? dueno { get; set; }


        //ICollection
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();







    }
}
