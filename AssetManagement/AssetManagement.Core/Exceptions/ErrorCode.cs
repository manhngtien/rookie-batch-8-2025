using Microsoft.AspNetCore.Http;

namespace AssetManagement.Core.DTOs.Exceptions
{
    public class ErrorCode
    {
        /// <summary>
        /// Custom error code, message, and status.
        /// </summary>
        // User related errors (600 - 699)


        // Access and token related errors (700 - 799)
        public static readonly ErrorCode UNAUTHORIZED_ACCESS = new(700, "Unauthorized access", StatusCodes.Status401Unauthorized);
        public static readonly ErrorCode ACCESS_DENIED = new(701, "Access denied to view or modify this resource", StatusCodes.Status403Forbidden);
        public static readonly ErrorCode INVALID_CLAIM = new(702, "Invalid claim", StatusCodes.Status403Forbidden);

        // Validation related errors (800 - 899)
        public static readonly ErrorCode VALIDATION_ERROR = new(800, "Validation error", StatusCodes.Status400BadRequest);

        // Assets related errors (900 - 999) 
        public static readonly ErrorCode ASSET_NOT_FOUND = new(900, "Asset not found", StatusCodes.Status404NotFound);

        /// <summary>
        /// Atributes for error code, message, and status. 
        /// </summary>
        private readonly int _code;
        private readonly string _message;
        private readonly int _status;

        public ErrorCode(int code, string message, int status)
        {
            _code = code;
            _message = message;
            _status = status;
        }

        public int GetCode() => _code;
        public string GetMessage() => _message;
        public int GetStatus() => _status;
    }
}
