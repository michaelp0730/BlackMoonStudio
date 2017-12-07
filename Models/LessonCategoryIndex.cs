namespace BlackMoonStudio.Models
{
    public class LessonCategoryIndex
    {
        public Levels Level { get; set; }
        public string Heading { get; set; }
        public string Summary { get; set; }
        public string Path { get; set; }
        public Lesson[] Lessons { get; set; }
    }
}