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
        public static readonly ErrorCode INVALID_OLD_PASSWORD = new(602, "Invalid old password", StatusCodes.Status400BadRequest);
        public static readonly ErrorCode IDENTITY_CREATION_FAILED = new(603, "Create user failed", StatusCodes.Status500InternalServerError);
        public static readonly ErrorCode SELF_ACCESS_DENIED = new(604, "Cannot access or modify your own user details in this administrative context", StatusCodes.Status403Forbidden);

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
        public static readonly ErrorCode ASSET_INVALID_STATE = new(901, "Asset is in invalid state", StatusCodes.Status400BadRequest);
        public static readonly ErrorCode ASSET_CANNOT_BE_EDITED = new(903, "Cannot edit an asset that is currently assigned", StatusCodes.Status400BadRequest);
        public static readonly ErrorCode INVALID_ASSET_STATE = new(904, "Invalid asset state", StatusCodes.Status400BadRequest);
        public static readonly ErrorCode ASSET_CANNOT_BE_DELETED = new(905, "Cannot delete an asset that is currently assigned", StatusCodes.Status400BadRequest);
        public static readonly ErrorCode ASSET_SORTING_ERROR = new(907, "Failed to sort assets with the edited asset on top", StatusCodes.Status500InternalServerError);
        public static readonly ErrorCode ASSET_HAS_HISTORICAL_ASSIGNMENTS = new(908, "Cannot delete the asset because it belongs to one or more historical assignments. If the asset is not able to be used anymore, please update its state in Edit Asset page", StatusCodes.Status400BadRequest);

        // Assignment related errors (1000 - 1099)
        public static readonly ErrorCode ASSIGNMENT_NOT_FOUND = new(1000, "Assignment not found", StatusCodes.Status404NotFound);
        public static readonly ErrorCode ASSIGNMENT_ALREADY_ACCEPTED = new(1001, "Assignment has already been accepted", StatusCodes.Status400BadRequest);
        public static readonly ErrorCode ASSET_ALREADY_ASSIGNED = new(1002, "Asset is already assigned in an in-progress assignment", StatusCodes.Status400BadRequest);
        public static readonly ErrorCode INVALID_DATE = new(1003, "Invalid date", StatusCodes.Status400BadRequest);
        public static readonly ErrorCode INVALID_LOCATION = new(1004, "Invalid location", StatusCodes.Status400BadRequest);
        public static readonly ErrorCode ASSET_NOT_AVAILABLE = new(1005, "Asset is not available", StatusCodes.Status400BadRequest);
        public static readonly ErrorCode USER_HAS_ACTIVE_ASSIGNMENTS = new(1006, "User has active assignments", StatusCodes.Status400BadRequest);
        public static readonly ErrorCode INVALID_ASSIGNMENT_ID = new(1007, "Invalid assignment id", StatusCodes.Status400BadRequest);
        public static readonly ErrorCode ASSIGNMENT_STATE_IS_NOT_WAITING_FOR_ACCEPTANCE = new(1008, "Assignment is not waiting for acceptance", StatusCodes.Status400BadRequest);

        // Category related errors (1100 - 1199)
        public static readonly ErrorCode CATEGORY_NOT_FOUND = new(1100, "Category not found", StatusCodes.Status404NotFound);
        public static readonly ErrorCode CATEGORY_MODIFICATION_NOT_ALLOWED = new(1101, "Category modification is not allowed", StatusCodes.Status400BadRequest);
        public static readonly ErrorCode CATEGORY_NAME_ALREADY_EXISTS = new(1101, "Category is already existed. Please enter a different category", StatusCodes.Status409Conflict);
        public static readonly ErrorCode CATEGORY_PREFIX_ALREADY_EXISTS = new(1102, "Prefix is already existed. Please enter a different prefix", StatusCodes.Status409Conflict);


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
