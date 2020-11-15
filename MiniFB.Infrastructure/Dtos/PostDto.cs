using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniFB.Infrastructure.Dtos
{
    public class PostDto
    {
        public string Text { get; set; }
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }
    }
}
