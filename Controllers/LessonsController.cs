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
            var beginnerLessonList = Lesson.GetLessonsByCategory("Beginner");
            var intermediateLessonList = Lesson.GetLessonsByCategory("Intermediate");
            var advancedLessonList = Lesson.GetLessonsByCategory("Advanced");
            var allLessons = beginnerLessonList.Concat(intermediateLessonList).Concat(advancedLessonList).ToList();
            var lessonsCuration = Lesson.GetCurationList("Lessons");
            var lessonContentsSerializer = new XmlSerializer(typeof(LessonContents));
            var relatedLessons = new List<Lesson>();
            var viewModel = new LessonViewModel();
            Lesson currentLesson;
            Lesson prevLesson;
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
                        currentLesson = advancedLessonList.FirstOrDefault(x => x.Slug == slug);

                        if (currentLesson == null)
                        {
                            return View("404");
                        }

                        prevLesson = currentLesson.GetAdjacentLesson(curation?.LessonSlugs, advancedLessonList, false);
                        nextLesson = currentLesson.GetAdjacentLesson(curation?.LessonSlugs, advancedLessonList, true);
                        fileStream = new FileStream("Xml/Lessons/Intermediate.xml", FileMode.Open);
                        lessonContents = (LessonContents)lessonContentsSerializer.Deserialize(fileStream);
                        fileStream.Dispose();
                        content = lessonContents.Contents.FirstOrDefault(x => x.Key == currentLesson?.ContentKey);

                        if (currentLesson?.RelatedLessonSlugs?.Any() == true)
                        {
                            relatedLessons = currentLesson.GetRelatedLessons(allLessons);
                        }

                        if (currentLesson != null)
                        {
                            viewModel.Slug = currentLesson.Slug;
                            viewModel.Title = currentLesson.Title;
                            viewModel.Summary = currentLesson.Summary;
                            viewModel.Url = currentLesson.Url;
                            viewModel.Content = content;
                            viewModel.Level = currentLesson.Level;
                            viewModel.Stage = currentLesson.Stage;
                            viewModel.Genres = currentLesson.Genres;
                            viewModel.Videos = currentLesson.Videos;
                            viewModel.Articles = currentLesson.Articles;
                            viewModel.RelatedLessons = relatedLessons.ToArray();

                            if (!string.IsNullOrEmpty(prevLesson.Title) && !string.IsNullOrEmpty(prevLesson.Url))
                            {
                                viewModel.PreviousLesson = prevLesson;
                            }

                            if (!string.IsNullOrEmpty(nextLesson.Title) && !string.IsNullOrEmpty(nextLesson.Url))
                            {
                                viewModel.NextLesson = nextLesson;
                            }
                            else if (currentLesson.IsLastLessonInCategory(curation?.LessonSlugs))
                            {
                                viewModel.IsLastLessonInCategory = true;
                            }

                            return View("~/Pages/lessons/_details.cshtml", viewModel);
                        }
                    }

                    return View("~/Pages/lessons/_landing.cshtml", new LessonCategoryIndex
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
                        currentLesson = intermediateLessonList.FirstOrDefault(x => x.Slug == slug);

                        if (currentLesson == null)
                        {
                            return View("404");
                        }

                        prevLesson = currentLesson.GetAdjacentLesson(curation?.LessonSlugs, intermediateLessonList, false);
                        nextLesson = currentLesson.GetAdjacentLesson(curation?.LessonSlugs, intermediateLessonList, true);
                        fileStream = new FileStream("Xml/Lessons/Intermediate.xml", FileMode.Open);
                        lessonContents = (LessonContents)lessonContentsSerializer.Deserialize(fileStream);
                        fileStream.Dispose();
                        content = lessonContents.Contents.FirstOrDefault(x => x.Key == currentLesson?.ContentKey);

                        if (currentLesson?.RelatedLessonSlugs?.Any() == true)
                        {
                            relatedLessons = currentLesson.GetRelatedLessons(allLessons);
                        }

                        if (currentLesson != null)
                        {
                            viewModel.Slug = currentLesson.Slug;
                            viewModel.Title = currentLesson.Title;
                            viewModel.Summary = currentLesson.Summary;
                            viewModel.Url = currentLesson.Url;
                            viewModel.Content = content;
                            viewModel.Level = currentLesson.Level;
                            viewModel.Stage = currentLesson.Stage;
                            viewModel.Genres = currentLesson.Genres;
                            viewModel.Videos = currentLesson.Videos;
                            viewModel.Articles = currentLesson.Articles;
                            viewModel.RelatedLessons = relatedLessons.ToArray();

                            if (!string.IsNullOrEmpty(prevLesson.Title) && !string.IsNullOrEmpty(prevLesson.Url))
                            {
                                viewModel.PreviousLesson = prevLesson;
                            }

                            if (!string.IsNullOrEmpty(nextLesson.Title) && !string.IsNullOrEmpty(nextLesson.Url))
                            {
                                viewModel.NextLesson = nextLesson;
                            }
                            else if (currentLesson.IsLastLessonInCategory(curation?.LessonSlugs))
                            {
                                viewModel.IsLastLessonInCategory = true;
                            }

                            return View("~/Pages/lessons/_details.cshtml", viewModel);
                        }
                    }

                    return View("~/Pages/lessons/_landing.cshtml", new LessonCategoryIndex
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
                        currentLesson = beginnerLessonList.FirstOrDefault(x => x.Slug == slug);

                        if (currentLesson == null)
                        {
                            return View("404");
                        }

                        prevLesson = currentLesson.GetAdjacentLesson(curation?.LessonSlugs, beginnerLessonList, false);
                        nextLesson = currentLesson.GetAdjacentLesson(curation?.LessonSlugs, beginnerLessonList, true);
                        fileStream = new FileStream("Xml/Lessons/Beginner.xml", FileMode.Open);
                        lessonContents = (LessonContents)lessonContentsSerializer.Deserialize(fileStream);
                        fileStream.Dispose();
                        content = lessonContents.Contents.FirstOrDefault(x => x.Key == currentLesson?.ContentKey);

                        if (currentLesson?.RelatedLessonSlugs?.Any() == true)
                        {
                            relatedLessons = currentLesson.GetRelatedLessons(allLessons);
                        }

                        if (currentLesson != null && content != null)
                        {
                            viewModel.Slug = currentLesson.Slug;
                            viewModel.Title = currentLesson.Title;
                            viewModel.Summary = currentLesson.Summary;
                            viewModel.Url = currentLesson.Url;
                            viewModel.Content = content;
                            viewModel.Level = currentLesson.Level;
                            viewModel.Stage = currentLesson.Stage;
                            viewModel.Genres = currentLesson.Genres;
                            viewModel.Videos = currentLesson.Videos;
                            viewModel.Articles = currentLesson.Articles;
                            viewModel.RelatedLessons = relatedLessons.ToArray();

                            if (!string.IsNullOrEmpty(prevLesson.Title) && !string.IsNullOrEmpty(prevLesson.Url))
                            {
                                viewModel.PreviousLesson = prevLesson;
                            }

                            if (!string.IsNullOrEmpty(nextLesson.Title) && !string.IsNullOrEmpty(nextLesson.Url))
                            {
                                viewModel.NextLesson = nextLesson;
                            }
                            else if (currentLesson.IsLastLessonInCategory(curation?.LessonSlugs))
                            {
                                viewModel.IsLastLessonInCategory = true;
                            }

                            return View("~/Pages/lessons/_details.cshtml", viewModel);
                        }
                    }

                    return View("~/Pages/lessons/_landing.cshtml", new LessonCategoryIndex
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
