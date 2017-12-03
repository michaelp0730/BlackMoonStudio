using System;
using System.Collections.Generic;
using BlackMoonStudio.Models;

namespace BlackMoonStudio.Models
{
    public class Lesson
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string ContentKey { get; set; }
        public Levels Level { get; set; }
        public string Genre { get; set; }
        public IEnumerable<Video> Videos { get; set; }
    }

    public enum Levels
    {
        Beginner,
        Intermediate,
        Advanced
    }
}