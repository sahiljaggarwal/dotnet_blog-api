using System.Security.Claims;
using BlogPortal.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BlogPortal.Binders
{
    public class UserBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var user = bindingContext.HttpContext.User;

            if (user?.Identity == null || user.Identity.IsAuthenticated != true)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }
            var payload = new UserPayload
            {
                Id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Email = user.FindFirst(ClaimTypes.Email)?.Value,
                Nbf = long.TryParse(user.FindFirst("nbf")?.Value, out var nbfVal) ? nbfVal : 0,
                Exp = long.TryParse(user.FindFirst("exp")?.Value, out var expVal) ? expVal : 0,
                Iat = long.TryParse(user.FindFirst("iat")?.Value, out var iatVal) ? iatVal : 0
            };

            bindingContext.Result = ModelBindingResult.Success(payload);
            return Task.CompletedTask;
        }
    }
}
