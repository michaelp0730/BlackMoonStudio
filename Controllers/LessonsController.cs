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
        [HttpGet("lessons/{category}/{slug?}/")]
        public IActionResult GetLessons(string category, string slug)
        {
            var lesson = new Lesson();
            var lessonsCuration = lesson.GetCurationList("Lessons");
            switch (category)
            {
                case "advanced":
                    var advancedLessons = lesson.GetLessons("Advanced");
                    if (!string.IsNullOrEmpty(slug))
                    {
                        lesson = advancedLessons.FirstOrDefault(x => x.Slug == slug);
                        return View("Pages/lessons/_details.cshtml", lesson);
                    }

                    var advancedCuration = lessonsCuration.FirstOrDefault(x => x.Slug == "Advanced");
                    return View("Pages/lessons/_landing.cshtml", new LessonCategoryIndex
                    {
                        Heading = "Advanced Guitar Lessons",
                        Lessons = advancedLessons.OrderBy(x => 
                            { return Array.IndexOf(advancedCuration.LessonSlugs, x.Slug); } ).ToArray(),
                    });
                    break;
                case "intermediate":
                    var intermediateLessons = lesson.GetLessons("Intermediate");
                    if (!string.IsNullOrEmpty(slug))
                    {
                        lesson = intermediateLessons.FirstOrDefault(x => x.Slug == slug);
                        return View("Pages/lessons/_details.cshtml", lesson);
                    }

                    var intermediateCuration = lessonsCuration.FirstOrDefault(x => x.Slug == "Intermediate");
                    return View("Pages/lessons/_landing.cshtml", new LessonCategoryIndex
                    {
                        Heading = "Intermediate Guitar Lessons",
                        Lessons = intermediateLessons.OrderBy(x => 
                            { return Array.IndexOf(intermediateCuration.LessonSlugs, x.Slug); } ).ToArray(),
                    });
                    break;
                default:
                    var beginnerLessons = lesson.GetLessons("Beginner");
                    if (!string.IsNullOrEmpty(slug))
                    {
                        lesson = beginnerLessons.FirstOrDefault(x => x.Slug == slug);
                        return View("Pages/lessons/_details.cshtml", lesson);
                    }

                    var beginnerCuration = lessonsCuration.FirstOrDefault(x => x.Slug == "Beginner");
                    return View("Pages/lessons/_landing.cshtml", new LessonCategoryIndex
                    {
                        Heading = "Beginner Guitar Lessons",
                        Lessons = beginnerLessons.OrderBy(x => 
                            { return Array.IndexOf(beginnerCuration.LessonSlugs, x.Slug); } ).ToArray(),
                    });
                    break;
            }
        }
    }
}