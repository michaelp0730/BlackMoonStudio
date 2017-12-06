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
            var lesson = new Lesson();
            LessonContents beginnerContents;
            var beginnerContentsSerializer = new XmlSerializer(typeof(LessonContents));
            var beginnerContentsFileStream = new FileStream("Xml/Lessons/Beginner.xml", FileMode.Open);

            beginnerContents = (LessonContents)beginnerContentsSerializer.Deserialize(beginnerContentsFileStream);
            beginnerContentsFileStream.Dispose();

            var lessonsCuration = lesson.GetCurationList("Lessons");
            var beginnerCuration = lessonsCuration.FirstOrDefault(x => x.Slug == "Beginner");
            var intermediateCuration = lessonsCuration.FirstOrDefault(x => x.Slug == "Intermediate");
            var advancedCuration = lessonsCuration.FirstOrDefault(x => x.Slug == "Advanced");

            BeginnerLessons = lesson.GetLessons("Beginner").OrderBy(x => 
                { return Array.IndexOf(beginnerCuration.LessonSlugs, x.Slug); } ).ToArray();

            BeginnerContents = beginnerContents.Contents;

            IntermediateLessons = lesson.GetLessons("Intermediate").OrderBy(x => 
                { return Array.IndexOf(intermediateCuration.LessonSlugs, x.Slug); } ).ToArray();

            AdvancedLessons = lesson.GetLessons("Advanced").OrderBy(x => 
                { return Array.IndexOf(advancedCuration.LessonSlugs, x.Slug); } ).ToArray();
        }
    }
}
