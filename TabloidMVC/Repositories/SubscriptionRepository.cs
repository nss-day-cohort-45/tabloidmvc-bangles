using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class SubscriptionRepository : BaseRepository, ISubscriptionRepository
    {
        public SubscriptionRepository(IConfiguration config) : base(config) { }

        public void Add(Subscription subscription)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Subscription (
                            SubscriberUserProfileId, ProviderUserProfileId, BeginDateTime, 
                            EndDateTime )
                        OUTPUT INSERTED.ID
                        Values (
                            @subscriberUserProfileId, @providerUserProfileId, @beginDateTime,
                            @endDateTime )
                        
                    ";
                    cmd.Parameters.AddWithValue("@subscriberUserProfileId", subscription.SubscriberUserProfileId);
                    cmd.Parameters.AddWithValue("@providerUserProfileId", subscription.ProviderUserProfileId);
                    cmd.Parameters.AddWithValue("@beginDateTime", subscription.BeginDateTime);
                    cmd.Parameters.AddWithValue("@endDateTime", DbUtils.ValueOrDBNull(subscription.EndDateTime));
                  

                    subscription.Id = (int)cmd.ExecuteScalar();
                }

            }
        }

    
    
    
    
    }
}
