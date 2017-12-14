using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace BlackMoonStudio.Models
{
    public class Lesson
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Url { get; set; }
        public string ContentKey { get; set; }
        public Levels Level { get; set; }
        public int Stage { get; set; }
        public string[] Genres { get; set; }
        public IEnumerable<Video> Videos { get; set; }
        public Article[] Articles { get; set; }
        public string[] RelatedLessonSlugs { get; set; }

        public static List<Lesson> GetLessonsByCategory(string level)
        {
            List<Lesson> lessonList;

            using (var sr = new StreamReader(path: $"Json/Lessons/{level}.json"))
            {
                lessonList = JsonConvert.DeserializeObject<List<Lesson>>(sr.ReadToEnd());
            }

            return lessonList;
        }

        public static List<Curation> GetCurationList(string list)
        {
            List<Curation> curationList;

            using (var sr = new StreamReader(path: $"Json/Curation/{list}.json"))
            {
                curationList = JsonConvert.DeserializeObject<List<Curation>>(sr.ReadToEnd());
            }

            return curationList;
        }
    }

    public enum Levels
    {
        Beginner,
        Intermediate,
        Advanced
    }

    public static class LessonHelpers
    {
        public static Lesson GetAdjacentLesson(this Lesson currentLesson, string[] lessonSlugsCuration, List<Lesson> lessonList, bool getNextNotPrevious)
        {
            var currentLessonCurationIndex = Array.IndexOf(lessonSlugsCuration, currentLesson.Slug);
            if (currentLessonCurationIndex == -1) return new Lesson();

            if (getNextNotPrevious)
            {
                if (lessonList.Count <= currentLessonCurationIndex + 1) return new Lesson();
                if (string.IsNullOrEmpty(lessonSlugsCuration[currentLessonCurationIndex + 1])) return new Lesson();
                var nextLessonSlug = lessonSlugsCuration[currentLessonCurationIndex + 1];
                return lessonList.FirstOrDefault(x => x.Slug == nextLessonSlug);
            }

            if (currentLessonCurationIndex == 0) return new Lesson();
            if (string.IsNullOrEmpty(lessonSlugsCuration[currentLessonCurationIndex - 1])) return new Lesson();
            var previousLessonSlug = lessonSlugsCuration[currentLessonCurationIndex - 1];
            return lessonList.FirstOrDefault(x => x.Slug == previousLessonSlug);
        }

        public static List<Lesson> GetRelatedLessons(this Lesson currentLesson, List<Lesson> lessonList)
        {
            return (from relatedLessonSlug in currentLesson.RelatedLessonSlugs from lesson in lessonList where lesson.Slug == relatedLessonSlug select lesson).ToList();
        }
    }
}