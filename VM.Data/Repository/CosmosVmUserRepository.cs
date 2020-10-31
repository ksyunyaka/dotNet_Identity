using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Core.Entities;
using VM.Core.Interfaces;

namespace VM.Data.Repository
{
    public class CosmosVmUserRepository : IVmUserRepository
    {
        private readonly CosmosClient cosmos;
        private readonly Container container;
        private readonly ILogger<CosmosVmUserRepository> logger;

        private readonly IList<string> availableRoles = new List<String>() { "VM_CREW", "VM_ADMIN", "CUSTOMER" };

        public CosmosVmUserRepository(CosmosClient con, ILogger<CosmosVmUserRepository> log)
        {
            cosmos = con;
            logger = log;
            container = cosmos.GetContainer("VMSite", "users");
        }

        
        public VmUserRole GetRole(string roleName)
        {
            if (availableRoles.Contains(roleName))
            {
                return new VmUserRole()
                {
                    Name = roleName
                };
            }
            else
            {
                logger.LogWarning("Got unknown role: " + roleName);
                return null;
            }
        }

        public async Task<VmUser> GetUserByUserName(string userName)
        {
            QueryDefinition query = new QueryDefinition("select * from users u where u.NormalizedUserName=@userName").WithParameter("@userName", userName);
            try
            {
                FeedIterator<VmUser> feedIterator = container.GetItemQueryIterator<VmUser>(query);
                if (feedIterator.HasMoreResults)
                {
                    FeedResponse<VmUser> currentResultSet = await feedIterator.ReadNextAsync();
                    int userCount = currentResultSet.Count();
                    if (userCount == 1)
                    {
                        foreach (VmUser user in currentResultSet)
                        {
                            return user;

                        }
                    }
                    else
                    {
                        logger.LogWarning("Found more than one person by username: " + userName);
                    }

                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to get user by username: " + userName);
          
            }
            return null;
        }

        public async Task<VmUser> CreateUserAsync(VmUser user)
        {
            try
            {
                ItemResponse<VmUser> itemResponse = await container.CreateItemAsync<VmUser>(user, new PartitionKey(user.UserName));
                return itemResponse.Resource;
            }
            catch (Exception e)
            {
                logger.LogError("Failed to create user: " + user, e);
                return null;
            }
            
        }
    }
}
