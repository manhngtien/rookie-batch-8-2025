﻿using AssetManagement.Core.Exceptions;
using AssetManagement.Application.Exceptions;
using System.Security.Claims;

namespace AssetManagement.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                throw new AccessDeniedException(ErrorCode.UNAUTHORIZED_ACCESS);
            }

            try
            {
                return claim.Value;
            }
            catch (FormatException)
            {
                throw new AccessDeniedException(ErrorCode.INVALID_CLAIM);
            }
        }
    }
}
