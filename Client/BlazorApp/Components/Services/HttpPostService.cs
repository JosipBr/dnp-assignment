using System.Text.Json;
using ApiContracts.Post;
using ApiContracts.Comment;

namespace BlazorApp.Components.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("posts", request);
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<List<PostDto>> GetPostsAsync()
    {
        HttpResponseMessage httpResponse = await client.GetAsync("posts");
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<List<PostDto>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<PostDto> GetPostByIdAsync(int postId)
    {
        HttpResponseMessage httpResponse = await client.GetAsync($"posts/{postId}");
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<List<CommentDto>> GetCommentsByPostIdAsync(int postId)
    {
        HttpResponseMessage httpResponse = await client.GetAsync($"posts/{postId}/comments");
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<List<CommentDto>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<CommentDto> AddCommentToPostAsync(int postId, CommentDto comment)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync($"posts/{postId}/comments", comment);
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        
        return JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
}
