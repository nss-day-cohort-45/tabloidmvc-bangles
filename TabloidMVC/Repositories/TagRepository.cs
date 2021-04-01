using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class TagRepository : BaseRepository, ITagRepository
    {
        public TagRepository(IConfiguration config) : base(config) { }

        public void AddTag(Tag tag)
        {
            throw new NotImplementedException();
        }

        public void DeleteTag(int tagId)
        {
            throw new NotImplementedException();
        }

        public List<Tag> GetAllTags()
        {
            throw new NotImplementedException();
        }

        public void UpdateTag(Tag tag)
        {
            throw new NotImplementedException();
        }
    }
}
