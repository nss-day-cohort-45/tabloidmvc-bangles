using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class PostTagRepository : BaseRepository, IPostTagRepository
    {
        public PostTagRepository(IConfiguration config) : base(config) { }
        
        public void AddPostTag(PostTag postTag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO PostTag(PostId, TagId)
                        OUTPUT INSERTED.ID
                        VALUES (@postId, @tagId)";

                    cmd.Parameters.AddWithValue("@postId", postTag.PostId);
                    cmd.Parameters.AddWithValue("@tagId", postTag.TagId);

                    int id = (int)cmd.ExecuteScalar();

                    postTag.Id = id;
                }
            }
        }

        public void DeletePostTag(int postTagId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM PostTag
                        WHERE Id = @postTagId";

                    cmd.Parameters.AddWithValue("@postTagId", postTagId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdatePostTag(PostTag postTag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE PostTag
                        set
                            PostId = @postId,
                            TagId = @tagId
                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@postId", postTag.PostId);
                    cmd.Parameters.AddWithValue("@tagId", postTag.TagId);
                    cmd.Parameters.AddWithValue("@id", postTag.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void PostTagExist(PostTag postTag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT DISTINCT COUNT(Id) AS Count, PostId, TagId
                        FROM PostTag
                        WHERE PostId = @postId AND TagId = @tagId
                        GROUP BY PostId, TagId";
                    
                    cmd.Parameters.AddWithValue("@postId", postTag.PostId);
                    cmd.Parameters.AddWithValue("@tagId", postTag.TagId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
