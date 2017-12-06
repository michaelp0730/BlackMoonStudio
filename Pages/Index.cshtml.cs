using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using BlackMoonStudio.Models;

namespace BlackMoonStudio.Pages
{
    public class IndexModel : PageModel
    {
        public Lesson[] BeginnerLessons { get; set; }
        public Content[] BeginnerContents { get; set; }
        public Lesson[] IntermediateLessons { get; set; }
        public Lesson[] AdvancedLessons { get; set; }
        public void OnGetAsync()
        {
            LessonContents beginnerContents;
            var beginnerContentsSerializer = new XmlSerializer(typeof(LessonContents));
            var beginnerContentsFileStream = new FileStream("Xml/Lessons/Beginner.xml", FileMode.Open);

            beginnerContents = (LessonContents)beginnerContentsSerializer.Deserialize(beginnerContentsFileStream);
            beginnerContentsFileStream.Dispose();

            var lessonsCuration = GetCurationList("Lessons");
            var beginnerCuration = lessonsCuration.FirstOrDefault(x => x.Slug == "Beginner");
            var intermediateCuration = lessonsCuration.FirstOrDefault(x => x.Slug == "Intermediate");
            var advancedCuration = lessonsCuration.FirstOrDefault(x => x.Slug == "Advanced");

            BeginnerLessons = GetLessons("Beginner").OrderBy(x => 
                { return Array.IndexOf(beginnerCuration.LessonSlugs, x.Slug); } ).ToArray();

            BeginnerContents = beginnerContents.Contents;

            IntermediateLessons = GetLessons("Intermediate").OrderBy(x => 
                { return Array.IndexOf(intermediateCuration.LessonSlugs, x.Slug); } ).ToArray();

            AdvancedLessons = GetLessons("Advanced").OrderBy(x => 
                { return Array.IndexOf(advancedCuration.LessonSlugs, x.Slug); } ).ToArray();
        }

        private List<Lesson> GetLessons(string level)
        {
            var lessonList = new List<Lesson>();

            using (StreamReader sr = new StreamReader(path: $"Json/Lessons/{level}.json"))
            {
                lessonList = JsonConvert.DeserializeObject<List<Lesson>>(sr.ReadToEnd());
            }

            return lessonList;
        }

        private List<Curation> GetCurationList(string list)
        {
            var curationList = new List<Curation>();

            using (StreamReader sr = new StreamReader(path: $"Json/Curation/{list}.json"))
            {
                curationList = JsonConvert.DeserializeObject<List<Curation>>(sr.ReadToEnd());
            }

            return curationList;
        }
    }
}
