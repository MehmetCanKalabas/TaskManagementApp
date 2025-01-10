using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementApp.Infrastructure.JWT
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, JwtHelper jwtHelper)
        {
            var token = context.Request.Headers["Authorization"].ToString()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                var principal = jwtHelper.GetPrincipalFromToken(token);

                if (principal != null)
                {
                    context.User = principal;
                }
            }

            await _next(context);
        }
    }

}
