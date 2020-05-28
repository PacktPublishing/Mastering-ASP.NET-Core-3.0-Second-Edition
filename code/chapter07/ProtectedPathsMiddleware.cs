﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chapter07
{
    public class ProtectedPathsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEnumerable<ProtectedPathOptions> _options;
        private readonly IAuthorizationService _authSvc;

        public ProtectedPathsMiddleware(
            RequestDelegate next,
            IAuthorizationService authSvc,
            IEnumerable<ProtectedPathOptions> options)
        {
            this._next = next;
            this._options = options ?? Enumerable.Empty<ProtectedPathOptions>();
            this._authSvc = authSvc;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            foreach (var option in this._options)
            {
                if (context.Request.Path.StartsWithSegments(option.Path))
                {
                    var result = await this._authSvc.AuthorizeAsync(
                        context.User,
                        context.Request.Path,
                        option.PolicyName);

                    if (!result.Succeeded)
                    {
                        await context.ChallengeAsync();
                        return;
                    }
                }
            }
            await this._next.Invoke(context);
        }
    }
}