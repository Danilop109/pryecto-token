using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Api.Helpers
{
    public class GlobalVerbRoleHandler : AuthorizationHandler<GlobalVerbRoleRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccesor;

        public GlobalVerbRoleHandler(IHttpContextAccessor httpContextAccesor)
        {
            this._httpContextAccesor= httpContextAccesor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GlobalVerbRoleRequirement requirement)
        {
            //mira y busca donde los roles requieren verbos
            var roles= context.User.FindAll(c => string.Equals(c.Type, ClaimTypes.Role)).Select(c => c.Value);
            var verb= _httpContextAccesor.HttpContext?.Request.Method;
            if (string.IsNullOrEmpty(verb)) {throw new Exception ($"request cann't be null");}
            foreach (var role in roles) {
                if (requirement.IsAllowed(role,verb))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
            context.Fail();
            return Task.CompletedTask;
        }
    }
}