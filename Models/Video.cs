using System;
using Microsoft.AspNetCore.Html;

namespace BlackMoonStudio.Models
{
    public class Video
    {
        public string Slug { get; set; }
        public HtmlString Caption { get; set; }
    }
}