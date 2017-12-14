using System;
using System.Collections.Generic;
using System.IO;
using BlackMoonStudio.Models;
using Newtonsoft.Json;

namespace BlackMoonStudio.Models
{
    public class LessonViewModel
    {
        public string Slug { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public Content Content { get; set; }
        public Levels Level { get; set; }
        public int Stage { get; set; }
        public string[] Genres { get; set; }
        public IEnumerable<Video> Videos { get; set; }
        public Article[] Articles { get; set; }
        public Lesson[] RelatedLessons { get; set; }
        public Lesson PreviousLesson { get; set; }
        public Lesson NextLesson { get; set; }
        public bool IsLastLessonInCategory { get; set; }
    }
}