namespace BlackMoonStudio.Models
{
    public class LessonsColumn
    {
        public string Title { get; set; }
        public string Path { get; set; }
        public Lesson[] Lessons { get; set; }
        public bool ShowSeeAllLink { get; set; }
        public string IconClass { get; set; }
    }
}