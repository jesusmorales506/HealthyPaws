
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyPawsV2.DAL
{
    [Table("Document")]
    public class Document
    {
        [Key]
        [Required]
        public int documentId { get; set; }

        [ForeignKey("Appointment")]
        [Required]
        [DisplayName("Número de Cita")]
        public int appointmentId { get; set; }

        [Required]
        [DisplayName("Nombre")]
        public string name { get; set; }


        [DisplayName("Categoria")]
        public string category { get; set; }

        [Required]
        [DisplayName("Tipo de Archivo")]
        public byte fileType { get; set; }

        [DisplayName("Estado")]
        public bool status { get; set; }

        //ICollection
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();






    }
}
