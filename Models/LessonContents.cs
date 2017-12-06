using System;
using Microsoft.AspNetCore.Html;

public class LessonContents
{
    public Content[] Contents { get; set; }
}

public class Content
{
    public string Key { get; set; }
    public string Text { get; set; }
}