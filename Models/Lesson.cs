using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace BlackMoonStudio.Models
{
    public class Lesson
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string ContentKey { get; set; }
        public Levels Level { get; set; }
        public int Stage { get; set; }
        public string[] Genres { get; set; }
        public IEnumerable<Video> Videos { get; set; }
        public Article[] Articles { get; set; }
        public string[] RelatedLessonSlugs { get; set; }

        public List<Lesson> GetLessonsByCategory(string level)
        {
            var lessonList = new List<Lesson>();

            using (var sr = new StreamReader(path: $"Json/Lessons/{level}.json"))
            {
                lessonList = JsonConvert.DeserializeObject<List<Lesson>>(sr.ReadToEnd());
            }

            return lessonList;
        }

        public List<Curation> GetCurationList(string list)
        {
            var curationList = new List<Curation>();

            using (var sr = new StreamReader(path: $"Json/Curation/{list}.json"))
            {
                curationList = JsonConvert.DeserializeObject<List<Curation>>(sr.ReadToEnd());
            }

            return curationList;
        }
    }

    public enum Levels
    {
        Beginner,
        Intermediate,
        Advanced
    }
}