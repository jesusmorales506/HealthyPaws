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
    [Table("Contacts")]
    public class Contact
    {
        [Key]
        public int contactId { get; set; }
        [DisplayName("Correo Electrónico")]
        public string Email { get; set; }
        [DisplayName("Número de Teléfono")]
        public string Phone{ get; set; }
        public bool WhatsApp { get; set; }
    }
}
