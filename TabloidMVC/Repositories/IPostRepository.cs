using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IPostRepository
    {
        void Add(Post post);
        List<Post> GetAllPublishedPosts();
        List<Post> GetUserPosts(int userProfileId);
        Post GetPublishedPostById(int id);
        Post GetUserPostById(int id, int userProfileId);
    }
}