using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using BlackMoonStudio.Models;

namespace BlackMoonStudio.Controllers
{
    public class LessonsController : Controller
    {
        [HttpGet("lessons/{category}/{slug?}/")]
        public IActionResult GetLessons(string category, string slug)
        {
            var lesson = new Lesson();
            var beginnerLessonList = Lesson.GetLessonsByCategory("Beginner");
            var intermediateLessonList = Lesson.GetLessonsByCategory("Intermediate");
            var advancedLessonList = Lesson.GetLessonsByCategory("Advanced");
            var allLessons = beginnerLessonList.Concat(intermediateLessonList).Concat(advancedLessonList).ToList();
            var lessonsCuration = Lesson.GetCurationList("Lessons");
            var lessonContentsSerializer = new XmlSerializer(typeof(LessonContents));
            var relatedLessons = new List<Lesson>();
            var viewModel = new LessonViewModel();
            Lesson nextLesson;
            Curation curation;
            LessonContents lessonContents;
            Content content;
            FileStream fileStream;

            switch (category)
            {
                case "advanced":
                    curation = lessonsCuration.FirstOrDefault(x => x.Slug == "Advanced");

                    if (!string.IsNullOrEmpty(slug))
                    {
                        lesson = advancedLessonList.FirstOrDefault(x => x.Slug == slug);
                        nextLesson = lesson.GetNextLesson(curation?.LessonSlugs, advancedLessonList);
                        fileStream = new FileStream("Xml/Lessons/Intermediate.xml", FileMode.Open);
                        lessonContents = (LessonContents)lessonContentsSerializer.Deserialize(fileStream);
                        fileStream.Dispose();
                        content = lessonContents.Contents.FirstOrDefault(x => x.Key == lesson?.ContentKey);

                        if (lesson?.RelatedLessonSlugs?.Any() == true)
                        {
                            relatedLessons = lesson.GetRelatedLessons(allLessons);
                        }

                        if (lesson != null)
                        {
                            viewModel.Slug = lesson.Slug;
                            viewModel.Title = lesson.Title;
                            viewModel.Summary = lesson.Summary;
                            viewModel.Url = lesson.Url;
                            viewModel.Content = content;
                            viewModel.Level = lesson.Level;
                            viewModel.Stage = lesson.Stage;
                            viewModel.Genres = lesson.Genres;
                            viewModel.Videos = lesson.Videos;
                            viewModel.Articles = lesson.Articles;
                            viewModel.RelatedLessons = relatedLessons.ToArray();

                            if (!string.IsNullOrEmpty(nextLesson.Title) && !string.IsNullOrEmpty(nextLesson.Url))
                            {
                                viewModel.NextLesson = nextLesson;
                            }

                            return View("Pages/lessons/_details.cshtml", viewModel);
                        }
                    }

                    return View("Pages/lessons/_landing.cshtml", new LessonCategoryIndex
                    {
                        Level = Levels.Advanced,
                        Heading = "Advanced Guitar Lessons",
                        Summary = "Time to take your game to a whole new level. The advanced lessons are more focused on techniques than anything else. Be prepared to practice - a lot - as these techniques require significant muscle memory, and rhythm. As always, you'll be better off if you practice with a metronome.",
                        Path = "/lessons/advanced/",
                        Lessons = advancedLessonList.OrderBy(x => Array.IndexOf(curation?.LessonSlugs, x.Slug)).ToArray(),
                    });
                case "intermediate":
                    curation = lessonsCuration.FirstOrDefault(x => x.Slug == "Intermediate");

                    if (!string.IsNullOrEmpty(slug))
                    {
                        lesson = intermediateLessonList.FirstOrDefault(x => x.Slug == slug);
                        nextLesson = lesson.GetNextLesson(curation?.LessonSlugs, intermediateLessonList);
                        fileStream = new FileStream("Xml/Lessons/Intermediate.xml", FileMode.Open);
                        lessonContents = (LessonContents)lessonContentsSerializer.Deserialize(fileStream);
                        fileStream.Dispose();
                        content = lessonContents.Contents.FirstOrDefault(x => x.Key == lesson?.ContentKey);

                        if (lesson?.RelatedLessonSlugs?.Any() == true)
                        {
                            relatedLessons = lesson.GetRelatedLessons(allLessons);
                        }

                        if (lesson != null)
                        {
                            viewModel.Slug = lesson.Slug;
                            viewModel.Title = lesson.Title;
                            viewModel.Summary = lesson.Summary;
                            viewModel.Url = lesson.Url;
                            viewModel.Content = content;
                            viewModel.Level = lesson.Level;
                            viewModel.Stage = lesson.Stage;
                            viewModel.Genres = lesson.Genres;
                            viewModel.Videos = lesson.Videos;
                            viewModel.Articles = lesson.Articles;
                            viewModel.RelatedLessons = relatedLessons.ToArray();

                            if (!string.IsNullOrEmpty(nextLesson.Title) && !string.IsNullOrEmpty(nextLesson.Url))
                            {
                                viewModel.NextLesson = nextLesson;
                            }

                            return View("Pages/lessons/_details.cshtml", viewModel);
                        }
                    }

                    return View("Pages/lessons/_landing.cshtml", new LessonCategoryIndex
                    {
                        Level = Levels.Intermediate,
                        Heading = "Intermediate Guitar Lessons",
                        Summary = "Time to take the basic concepts from the Beginner category and start learning lots of fun new stuff. Soon you will feel comfortable playing leads and solos in any key, anywhere on the neck. Your understanding of music theory, and your comfort level navigating the fretboard, are about to grow significantly. Let's get started!",
                        Path = "/lessons/intermediate/",
                        Lessons = intermediateLessonList.OrderBy(x => Array.IndexOf(curation?.LessonSlugs, x.Slug)).ToArray(),
                    });
                default:
                    curation = lessonsCuration.FirstOrDefault(x => x.Slug == "Beginner");

                    if (!string.IsNullOrEmpty(slug))
                    {
                        lesson = beginnerLessonList.FirstOrDefault(x => x.Slug == slug);
                        nextLesson = lesson.GetNextLesson(curation?.LessonSlugs, beginnerLessonList);
                        fileStream = new FileStream("Xml/Lessons/Beginner.xml", FileMode.Open);
                        lessonContents = (LessonContents)lessonContentsSerializer.Deserialize(fileStream);
                        fileStream.Dispose();
                        content = lessonContents.Contents.FirstOrDefault(x => x.Key == lesson?.ContentKey);

                        if (lesson?.RelatedLessonSlugs?.Any() == true)
                        {
                            relatedLessons = lesson.GetRelatedLessons(allLessons);
                        }

                        if (lesson != null && content != null)
                        {
                            viewModel.Slug = lesson.Slug;
                            viewModel.Title = lesson.Title;
                            viewModel.Summary = lesson.Summary;
                            viewModel.Url = lesson.Url;
                            viewModel.Content = content;
                            viewModel.Level = lesson.Level;
                            viewModel.Stage = lesson.Stage;
                            viewModel.Genres = lesson.Genres;
                            viewModel.Videos = lesson.Videos;
                            viewModel.Articles = lesson.Articles;
                            viewModel.RelatedLessons = relatedLessons.ToArray();

                            if (!string.IsNullOrEmpty(nextLesson.Title) && !string.IsNullOrEmpty(nextLesson.Url))
                            {
                                viewModel.NextLesson = nextLesson;
                            }

                            return View("Pages/lessons/_details.cshtml", viewModel);
                        }
                    }

                    return View("Pages/lessons/_landing.cshtml", new LessonCategoryIndex
                    {
                        Level = Levels.Beginner,
                        Heading = "Beginner Guitar Lessons",
                        Summary = "These lessons will take the absolute beginner to a point where they are ready to start exploring various playing techniques, and genre-specific licks. Like all categories, the lessons are broken down into 3 stages, so you can think of the stages as mini categories.",
                        Path = "/lessons/beginner/",
                        Lessons = beginnerLessonList.OrderBy(x => Array.IndexOf(curation?.LessonSlugs, x.Slug)).ToArray(),
                    });
            }
        }
    }
}