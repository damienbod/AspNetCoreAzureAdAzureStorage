﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreAzureStorage
{
    public class StorageBlobDataContributorRoleHandler : AuthorizationHandler<StorageBlobDataContributorRoleRequirement>
    {
        private readonly AzureManagementFluentService _azureManagementFluentService;

        public StorageBlobDataContributorRoleHandler(AzureManagementFluentService azureManagementFluentService)
        {
            _azureManagementFluentService = azureManagementFluentService;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            StorageBlobDataContributorRoleRequirement requirement
        )
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (requirement == null)
                throw new ArgumentNullException(nameof(requirement));

            var scope = "subscriptions/1f943d6c-66d4-4c2f-a158-e6b99fcec7a2/resourceGroups/damienbod-rg/providers/Microsoft.Storage/storageAccounts/azureadfiles";

            var spIdUserClaim = context.User.Claims.FirstOrDefault(t => t.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");

            if (spIdUserClaim != null)
            {
                var success = _azureManagementFluentService.HasRoleStorageBlobDataContributorForScope(spIdUserClaim.Value, scope);
                if (success)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}