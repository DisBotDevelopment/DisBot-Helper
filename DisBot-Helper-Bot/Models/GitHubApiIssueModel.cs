using System.Text.Json.Serialization;

namespace DisBot_Helper_Bot.Models;

public class GitHubApiIssueModel
{
    public GitHubApiIssueModel(string title, string body, string[] assignees)
    {
        Title = title;
        Body = body;
        Assignees = assignees;
    }

    [JsonPropertyName("title")] public string Title { get; set; }
    [JsonPropertyName("body")] public string Body { get; set; }
    [JsonPropertyName("assignees")] public string[] Assignees { get; set; } = ["xyzjesper"];
}