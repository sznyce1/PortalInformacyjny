using Microsoft.AspNetCore.Authorization;
using ProjektZaliczeniowy.entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjektZaliczeniowy.Authorization
{
    public class ArticleResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Article>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Article article)
        {
            if(requirement.ResourceOperation == ResourceOperation.Read || requirement.ResourceOperation == ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if(article.AuthorId == int.Parse(userId))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
