using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Data.Models.Gallery
{
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int EthnoAreaId { get; set; }

        public EthnographicArea EthnoArea { get; set; }

        [Required]
        public int CostumeType { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
    }
}
