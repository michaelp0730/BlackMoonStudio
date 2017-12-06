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
        [HttpGet("lessons/beginner/{slug}/")]
        public IActionResult GetBeginnerLesson(string slug)
        {
            var lesson = new Lesson();
            var beginnerLessons = lesson.GetLessons("Beginner");
            lesson = beginnerLessons.Where(x => x.Slug == slug).FirstOrDefault();
            return View("LessonDetails", lesson); 
        }
    }
}