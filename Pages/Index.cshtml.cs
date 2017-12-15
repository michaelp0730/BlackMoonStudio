using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BlackMoonStudio.Models;

namespace BlackMoonStudio.Pages
{
    public class IndexModel : PageModel
    {
        public Lesson[] BeginnerLessons { get; private set; }
        public Lesson[] IntermediateLessons { get; private set; }
        public Lesson[] AdvancedLessons { get; private set; }
        public void OnGetAsync()
        {
            var lessonsCuration = Lesson.GetCurationList("Lessons");
            var beginnerCuration = lessonsCuration.FirstOrDefault(x => x.Slug == "Beginner");
            var intermediateCuration = lessonsCuration.FirstOrDefault(x => x.Slug == "Intermediate");
            var advancedCuration = lessonsCuration.FirstOrDefault(x => x.Slug == "Advanced");
            var allBeginnerLessons = Lesson.GetLessonsByCategory("Beginner");
            var allIntermediateLessons = Lesson.GetLessonsByCategory("Intermediate");
            var allAdvancedLessons = Lesson.GetLessonsByCategory("Advanced");

            BeginnerLessons = GetFirst3LessonsByCuration(allBeginnerLessons,
                beginnerCuration?.LessonSlugs.Take(3).ToArray());
            IntermediateLessons = GetFirst3LessonsByCuration(allIntermediateLessons,
                intermediateCuration?.LessonSlugs.Take(3).ToArray());
            AdvancedLessons = GetFirst3LessonsByCuration(allAdvancedLessons,
                advancedCuration?.LessonSlugs.Take(3).ToArray());
        }

        private static Lesson[] GetFirst3LessonsByCuration(IReadOnlyCollection<Lesson> lessonList, IEnumerable<string> curatedSlugs)
        {
            return curatedSlugs.Select(slug => lessonList.FirstOrDefault(x => slug == x.Slug)).ToArray();
        }
    }
}
