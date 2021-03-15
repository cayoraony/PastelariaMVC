using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PastelariaMvc.Middlewares
{
    public class LoginValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public LoginValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
                
            }
            catch
            {
                if (context.Session.GetString("Token") == null)
                {
                    context.Response.Redirect("/Login/Login");
                }
            }
            
        }
    }
}
