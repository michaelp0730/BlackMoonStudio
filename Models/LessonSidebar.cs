namespace BlackMoonStudio.Models
{
    public class LessonSidebar
    {
        public Lesson[] RelatedLessons { get; set; }
        public Lesson NextLesson { get; set; }
    }
}