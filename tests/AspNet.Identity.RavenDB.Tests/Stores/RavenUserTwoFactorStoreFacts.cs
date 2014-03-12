﻿using AspNet.Identity.RavenDB.Entities;
using AspNet.Identity.RavenDB.Stores;
using Microsoft.AspNet.Identity;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AspNet.Identity.RavenDB.Tests.Stores
{
    public class RavenUserTwoFactorStoreFacts : TestBase
    {
        [Fact]
        public async Task GetTwoFactorEnabledAsync_Should_Get_User_IsTwoFactorEnabled_Value()
        {
            using (IDocumentStore store = CreateEmbeddableStore())
            {
                const string userName = "Tugberk";
                const string userId = "RavenUsers/1";

                using (IAsyncDocumentSession ses = store.OpenAsyncSession())
                {
                    RavenUser user = new RavenUser { Id = userId, UserName = userName, IsTwoFactorEnabled = true };
                    await ses.StoreAsync(user);
                    await ses.SaveChangesAsync();
                }

                using (IAsyncDocumentSession ses = store.OpenAsyncSession())
                {
                    // Act
                    RavenUser user = await ses.LoadAsync<RavenUser>(userId);
                    IUserTwoFactorStore<RavenUser> userTwoFactorStore = new RavenUserStore<RavenUser>(ses);
                    bool isTwoFactorEnabled = await userTwoFactorStore.GetTwoFactorEnabledAsync(user);

                    // Assert
                    Assert.True(isTwoFactorEnabled);
                }
            }
        }
    }
}