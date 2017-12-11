using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
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
            var lessonList = new List<Lesson>();
            var curation = new Curation();
            var lessonsCuration = lesson.GetCurationList("Lessons");
            var lessonContentsSerializer = new XmlSerializer(typeof(LessonContents));
            LessonContents lessonContents;
            FileStream fileStream;

            switch (category)
            {
                case "advanced":
                    lessonList = lesson.GetLessons("Advanced");
                    if (!string.IsNullOrEmpty(slug))
                    {
                        lesson = lessonList.FirstOrDefault(x => x.Slug == slug);
                        var viewModel = new LessonViewModel
                        {
                            Slug = lesson.Slug,
                            Title = lesson.Title,
                            Summary = lesson.Summary,
                            Level = lesson.Level,
                            Stage = lesson.Stage,
                            Genres = lesson.Genres,
                            Videos = lesson.Videos,
                            Articles = lesson.Articles,
                            RelatedLessonSlugs = lesson.RelatedLessonSlugs,
                        };
                        return View("Pages/lessons/_details.cshtml", viewModel);
                    }

                    curation = lessonsCuration.FirstOrDefault(x => x.Slug == "Advanced");
                    return View("Pages/lessons/_landing.cshtml", new LessonCategoryIndex
                    {
                        Level = Levels.Advanced,
                        Heading = "Advanced Guitar Lessons",
                        Summary = "Time to take your game to a whole new level. The advanced lessons are more focused on techniques than anything else. Be prepared to practice - a lot - as these techniques require significant muscle memory, and rhythm. As always, you'll be better off if you practice with a metronome.",
                        Path = "/lessons/advanced/",
                        Lessons = lessonList.OrderBy(x => 
                            { return Array.IndexOf(curation.LessonSlugs, x.Slug); } ).ToArray(),
                    });
                case "intermediate":
                    lessonList = lesson.GetLessons("Intermediate");
                    if (!string.IsNullOrEmpty(slug))
                    {
                        lesson = lessonList.FirstOrDefault(x => x.Slug == slug);
                        var viewModel = new LessonViewModel
                        {
                            Slug = lesson.Slug,
                            Title = lesson.Title,
                            Summary = lesson.Summary,
                            Level = lesson.Level,
                            Stage = lesson.Stage,
                            Genres = lesson.Genres,
                            Videos = lesson.Videos,
                            Articles = lesson.Articles,
                            RelatedLessonSlugs = lesson.RelatedLessonSlugs,
                        };
                        return View("Pages/lessons/_details.cshtml", viewModel);
                    }

                    curation = lessonsCuration.FirstOrDefault(x => x.Slug == "Intermediate");
                    return View("Pages/lessons/_landing.cshtml", new LessonCategoryIndex
                    {
                        Level = Levels.Intermediate,
                        Heading = "Intermediate Guitar Lessons",
                        Summary = "Time to take the basic concepts from the Beginner category and start learning lots of fun new stuff. Soon you will feel comfortable playing leads and solos in any key, anywhere on the neck. Your understanding of music theory, and your comfort level navigating the fretboard, are about to grow significantly. Let's get started!",
                        Path = "/lessons/intermediate/",
                        Lessons = lessonList.OrderBy(x => 
                            { return Array.IndexOf(curation.LessonSlugs, x.Slug); } ).ToArray(),
                    });
                default:
                    lessonList = lesson.GetLessons("Beginner");
                    if (!string.IsNullOrEmpty(slug))
                    {
                        lesson = lessonList.FirstOrDefault(x => x.Slug == slug);
                        fileStream = new FileStream("Xml/Lessons/Beginner.xml", FileMode.Open);
                        lessonContents = (LessonContents)lessonContentsSerializer.Deserialize(fileStream);
                        fileStream.Dispose();
                        var content = lessonContents.Contents.FirstOrDefault(x => x.Key == lesson.ContentKey);
                        var viewModel = new LessonViewModel
                        {
                            Slug = lesson.Slug,
                            Title = lesson.Title,
                            Summary = lesson.Summary,
                            Content = content,
                            Level = lesson.Level,
                            Stage = lesson.Stage,
                            Genres = lesson.Genres,
                            Videos = lesson.Videos,
                            Articles = lesson.Articles,
                            RelatedLessonSlugs = lesson.RelatedLessonSlugs,
                        };
                        return View("Pages/lessons/_details.cshtml", viewModel);
                    }

                    curation = lessonsCuration.FirstOrDefault(x => x.Slug == "Beginner");
                    return View("Pages/lessons/_landing.cshtml", new LessonCategoryIndex
                    {
                        Level = Levels.Beginner,
                        Heading = "Beginner Guitar Lessons",
                        Summary = "These lessons will take the absolute beginner to a point where they are ready to start exploring various playing techniques, and genre-specific licks. Like all categories, the lessons are broken down into 3 stages, so you can think of the stages as mini categories.",
                        Path = "/lessons/beginner/",
                        Lessons = lessonList.OrderBy(x => 
                            { return Array.IndexOf(curation.LessonSlugs, x.Slug); } ).ToArray(),
                    });
            }
        }
    }
}