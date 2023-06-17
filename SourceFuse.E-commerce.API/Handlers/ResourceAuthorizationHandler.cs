using Microsoft.AspNetCore.Identity;
using SourceFuse.E_commerce.API.Infrastructure.Managers.Interfaces;
using SourceFuse.E_commerce.Entities.Identity;
using SourceFuse.E_commerce.Common.Enums;
using Microsoft.AspNetCore.Authorization;

namespace SourceFuse.E_commerce.API.Handlers
{
    public class ResourceAuthorizationHandler : AuthorizationHandler<
        ResourceAuthorizationHandler.ResourceAuthorizationRequirement, object>
    {
        private readonly IUsersService _usersService;
        private readonly IConfigurationService _configurationService;

        public ResourceAuthorizationHandler(IConfigurationService configurationService,
            UserManager<ApplicationUser> userManager, IUsersService usersService)
        {
            _configurationService = configurationService;
            _usersService = usersService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ResourceAuthorizationRequirement requirement,
            object resource)
        {


            var user = await _usersService.GetByPrincipal(context.User);

            if (requirement.RoleBased)
            {
                string roleName = requirement.RoleName;

                if (requirement.GetType() == typeof(AllowedToManageProductRequirement) ||
                    requirement.GetType() == typeof(AllowedToCreateCommentRequirement))
                {
                    if (roleName == _configurationService.GetAdminRoleName())
                    {
                    }
                }
            }
            else
            {
                // For creation we have no Resource
                var policy = requirement.AuthorizationPolicy;
                if (policy == AuthorizationPolicyEnum.ONLY_ADMIN)
                {
                    if (context.User.Identity != null &&
                        context.User.Identity.Name != null &&
                        context.User.IsInRole(_configurationService.GetAdminRoleName()))
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                    }
                }
                else if (policy == AuthorizationPolicyEnum.ONLY_OWNER)
                {
                }
                else if (policy == AuthorizationPolicyEnum.AUTHENTICATED_USER)
                {
                    if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
                    {
                        context.Succeed(requirement);
                    }
                }
            }
        }


        public abstract class ResourceAuthorizationRequirement : IAuthorizationRequirement
        {
            public string RoleName { get; set; }
            public bool RoleBased { get; set; }
            public AuthorizationPolicyEnum AuthorizationPolicy { get; set; }

            protected ResourceAuthorizationRequirement(string roleName)
            {
                this.RoleName = roleName;
                this.RoleBased = true;
            }

            protected ResourceAuthorizationRequirement(AuthorizationPolicyEnum authorizationPolicy)
            {
                this.AuthorizationPolicy = authorizationPolicy;
            }
        }



        public class AllowedToManageProductRequirement : ResourceAuthorizationRequirement
        {
            public AllowedToManageProductRequirement(string roleName) : base(roleName)
            {
            }
        }


        public class AllowedToCreateCommentRequirement : ResourceAuthorizationRequirement
        {
            public AllowedToCreateCommentRequirement(string roleName) : base(roleName)
            {
            }
        }

        public class AllowedToUpdateCommentRequirement : ResourceAuthorizationRequirement
        {
            public AllowedToUpdateCommentRequirement(AuthorizationPolicyEnum authorizationPolicy) : base(
                authorizationPolicy)
            {
            }
        }

        public class AllowedToDeleteCommentRequirement : ResourceAuthorizationRequirement
        {
            public AllowedToDeleteCommentRequirement(AuthorizationPolicyEnum authorizationPolicy) : base(
                authorizationPolicy)
            {
            }
        }
    }
}
