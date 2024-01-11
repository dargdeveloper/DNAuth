// namespace Sisa.Application.Api.Plumbing;
//
// using Architecture.Common.Domain.Seguridad;
// using Configuration;
// using Domain.Seguridad;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Filters;
// using Microsoft.Extensions.Options;
//
// public class PermisosRequeridosAttribute : AuthorizeAttribute, IAuthorizationFilter
// {
//     private readonly object[] _permissions;
//
//     public PermisosRequeridosAttribute(params object[] permissions)
//     {
//         _permissions = permissions;
//     }
//
//     public void OnAuthorization(AuthorizationFilterContext context)
//     {
//         var authSettingsOptions = context
//             .HttpContext.RequestServices.GetService<IOptions<AuthSettingsOptions>>()
//             ?.Value;
//         if (authSettingsOptions is { IgnoreAuth: true })
//         {
//             return;
//         }
//         var permissions = _permissions.Select(x => x.ToString());
//
//         var userAccess = GetPermissionsForUser(context);
//
//         var authorized = permissions.Any(userAccess.Contains);
//         if (!authorized)
//         {
//             context.Result = new ForbidResult();
//         }
//     }
//
//     /// <summary>
//     /// Lógica para obtener los permisos con logica de cache
//     /// </summary>
//     /// <param name="context"></param>
//     /// <param name="userId"></param>
//     /// <returns></returns>
//     private List<string> GetPermissionsForUser(AuthorizationFilterContext context)
//     {
//         var loggedUser = context.HttpContext.RequestServices.GetService<ILoggedUser>();
//         var roles = loggedUser
//             .GetClaims()
//             .Where(x => x.Type == CustomClaims.Permisos)
//             .Select(x => x.Value);
//
//         return roles.ToList();
//     }
// }