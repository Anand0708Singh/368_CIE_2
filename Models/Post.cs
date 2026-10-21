namespace _368_CIE_2.Models;

// Represents one post received from the JSONPlaceholder API.
public class Post
{
    // Stores the unique ID of the post.
    public int Id { get; set; }

    // Stores the title of the post.
    public string Title { get; set; } = string.Empty;
}