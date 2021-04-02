using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;


namespace TabloidMVC.Repositories
{
    public interface ICommentRepository
    {
        void Add(Comment comment);
        List<Comment> GetCommentsByPost(int postId);
    }
}
