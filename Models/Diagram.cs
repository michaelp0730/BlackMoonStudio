using System;
using Microsoft.AspNetCore.Html;

namespace BlackMoonStudio.Models
{
    public class Diagram
    {
        public string Title { get; set; }
        public string Src { get; set; }
        public HtmlString Caption { get; set; }
    }
}