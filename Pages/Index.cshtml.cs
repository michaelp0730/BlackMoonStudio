using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using BlackMoonStudio.Models;

namespace BlackMoonStudio.Pages
{
    public class IndexModel : PageModel
    {
        public Lesson[] BeginnerLessons { get; set; }
        public Lesson[] IntermediateLessons { get; set; }
        public Lesson[] AdvancedLessons { get; set; }
        public void OnGetAsync()
        {
            var lessonsCuration = GetCurationList("Lessons");
            var beginnerCuration = lessonsCuration.FirstOrDefault(x => x.Slug == "Beginner");
            var intermediateCuration = lessonsCuration.FirstOrDefault(x => x.Slug == "Intermediate");
            var advancedCuration = lessonsCuration.FirstOrDefault(x => x.Slug == "Advanced");

            BeginnerLessons = GetLessons("Beginner").OrderBy(x => 
                { return Array.IndexOf(beginnerCuration.LessonSlugs, x.Slug); } ).ToArray();

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
