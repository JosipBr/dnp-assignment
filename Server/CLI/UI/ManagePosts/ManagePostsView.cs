using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostsView
{
     private readonly CreatePostView _createPostView;
    private readonly ListPostsView _listPostsView;
    private readonly SinglePostView _singlePostView;
    private readonly AddCommentView _addCommentView;

    public ManagePostsView(IPostRepository postRepository, ICommentRepository commentRepository, IUserRepository userRepository)
    {
        _createPostView = new CreatePostView(postRepository, userRepository);
        _listPostsView = new ListPostsView(postRepository);
        _singlePostView = new SinglePostView(postRepository, commentRepository);
        _addCommentView = new AddCommentView(commentRepository, postRepository, userRepository);
    }

    public async Task DisplayPostManagementMenuAsync()
    {
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\n=== Manage Posts ===");
            Console.WriteLine("1. Create Post");
            Console.WriteLine("2. List Posts");
            Console.WriteLine("3. View Single Post");
            Console.WriteLine("4. Add Comment to Post");
            Console.WriteLine("5. Exit");

            Console.Write("\nChoose an option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    await _createPostView.CreateNewPostAsync();
                    break;
                case "2":
                    await _listPostsView.DisplayPostsAsync();
                    break;
                case "3" :
                    await _singlePostView.DisplayPostAsync();
                    break;
                case "4":
                    await _addCommentView.AddCommentAsync();
                    break;
                case "5":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
}