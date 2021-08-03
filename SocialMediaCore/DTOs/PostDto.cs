using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaCore.DTOs
{
   public  class PostDto
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public DateTime? Date { get; set; } //al agregar ? estoy haciendo un campo nuleable
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
