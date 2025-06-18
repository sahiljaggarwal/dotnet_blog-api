using Microsoft.AspNetCore.Mvc;

[AttributeUsage(AttributeTargets.Parameter)]
public class GetUserAttribute : ModelBinderAttribute
{
    public GetUserAttribute(): base(typeof(BlogPortal.Binders.UserBinder)){}
}