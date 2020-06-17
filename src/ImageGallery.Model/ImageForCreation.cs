using System.ComponentModel.DataAnnotations;

namespace ImageGallery.Model
{
    public class ImageForCreation
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(150)]
        public string Description { get; set; }

        [Required]
        public byte[] Bytes { get; set; }
    }
}
