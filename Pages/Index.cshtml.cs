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
            BeginnerLessons = GetLessons("Beginner");
            IntermediateLessons = GetLessons("Intermediate");
            AdvancedLessons = GetLessons("Advanced");
        }

        private Lesson[] GetLessons(string level)
        {
            var lessonList = new List<Lesson>();

            using (StreamReader sr = new StreamReader(path: $"Json/Lessons/{level}.json"))
            {
                lessonList = JsonConvert.DeserializeObject<List<Lesson>>(sr.ReadToEnd());
            }

            return lessonList.ToArray();
        }
    }
}
