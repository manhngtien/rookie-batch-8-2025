using Microsoft.AspNetCore.Http;

namespace AssetManagement.Core.Exceptions
{
    public class ErrorCode
    {
        /// <summary>
        /// Custom error code, message, and status.
        /// </summary>
        /// 
        // Database integration errors (500 - 599)
        public static readonly ErrorCode SAVE_ERROR = new(500, "Error saving to database", StatusCodes.Status500InternalServerError);

        // User related errors (600 - 699)
        public static readonly ErrorCode ACCOUNT_NOT_FOUND = new(600, "Account not found", StatusCodes.Status404NotFound);
        public static readonly ErrorCode USER_NOT_FOUND = new(601, "User not found", StatusCodes.Status404NotFound);

        // Access and token related errors (700 - 799)
        public static readonly ErrorCode UNAUTHORIZED_ACCESS = new(700, "Unauthorized access", StatusCodes.Status401Unauthorized);
        public static readonly ErrorCode ACCESS_DENIED = new(701, "Access denied to view or modify this resource", StatusCodes.Status403Forbidden);
        public static readonly ErrorCode INVALID_CLAIM = new(702, "Invalid claim", StatusCodes.Status404NotFound);
        public static readonly ErrorCode INVALID_CREDENTIALS = new(704, "Invalid credentials", StatusCodes.Status401Unauthorized);
        public static readonly ErrorCode INVALID_REFRESH_TOKEN = new(705, "Invalid refresh token", StatusCodes.Status401Unauthorized);
        public static readonly ErrorCode REFRESH_TOKEN_EXPIRED = new(706, "Refresh token expired", StatusCodes.Status401Unauthorized);
        public static readonly ErrorCode REFRESH_TOKEN_NOT_FOUND = new(707, "Refresh token not found", StatusCodes.Status404NotFound);

        // Validation related errors (800 - 899)
        public static readonly ErrorCode VALIDATION_ERROR = new(800, "Validation error", StatusCodes.Status422UnprocessableEntity);

        // Assets related errors (900 - 999) 
        public static readonly ErrorCode ASSET_NOT_FOUND = new(900, "Asset not found", StatusCodes.Status404NotFound);

        // Assignment related errors (1000 - 1099)
        public static readonly ErrorCode ASSIGNMENT_NOT_FOUND = new(1000, "Assignment not found", StatusCodes.Status404NotFound);
        public static readonly ErrorCode ASSIGNMENT_ALREADY_ACCEPTED = new(1001, "Assignment has already been accepted", StatusCodes.Status400BadRequest);
        public static readonly ErrorCode ASSET_ALREADY_ASSIGNED = new(1002, "Asset is already assigned in an in-progress assignment", StatusCodes.Status400BadRequest);
        public static readonly ErrorCode INVALID_DATE = new(1003, "Invalid date", StatusCodes.Status400BadRequest);

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
