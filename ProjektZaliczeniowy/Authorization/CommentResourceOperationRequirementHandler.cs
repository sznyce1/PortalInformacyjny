using Microsoft.AspNetCore.Authorization;
using ProjektZaliczeniowy.entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjektZaliczeniowy.Authorization
{
    public class CommentResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Comment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Comment comment)
        {
            if (requirement.ResourceOperation == ResourceOperation.Read || requirement.ResourceOperation == ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (comment.AuthorId == int.Parse(userId))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
