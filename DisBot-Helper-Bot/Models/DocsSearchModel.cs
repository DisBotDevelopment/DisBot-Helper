namespace DisBot_Helper_Bot.Models;

// This is only a rebuild no a C# Syntax conform model!!!

public class DocsSearchModel
{
    public Pagination pagination { get; set; }
    public Data[] data { get; set; }
}

public class Data
{
    public double ranking { get; set; }
    public string context { get; set; }
    public Document document { get; set; }
}

public class Document
{
    public string id { get; set; }
    public string context { get; set; }
    public string url { get; set; }
    public string urlId { get; set; }
    public string title { get; set; }
    public string text { get; set; }
}

public class Pagination
{
    public int limit { get; set; }
    public int offset { get; set; }
    public string nextPath { get; set; }
    public int total { get; set; }
}