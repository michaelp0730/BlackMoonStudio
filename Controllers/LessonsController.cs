using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BlackMoonStudio.Models;

namespace BlackMoonStudio.Controllers
{
    public class LessonsController : Controller
    {
        [HttpGet("lessons/beginner/{slug?}/")]
        public IActionResult GetBeginnerLesson(string slug)
        {
            var lesson = new Lesson();
            var beginnerLessons = lesson.GetLessons("Beginner");
            if (!string.IsNullOrEmpty(slug))
            {
                lesson = beginnerLessons.FirstOrDefault(x => x.Slug == slug);
                return View("Pages/lessons/_details.cshtml", lesson);
            }

            var lessonsCuration = lesson.GetCurationList("Lessons");
            var beginnerCuration = lessonsCuration.FirstOrDefault(x => x.Slug == "Beginner");
            var viewModel = beginnerLessons.OrderBy(x => 
                { return Array.IndexOf(beginnerCuration.LessonSlugs, x.Slug); } );

            return View("Pages/lessons/beginner.cshtml", viewModel.ToArray());
        }
    }
}