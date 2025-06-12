﻿using AssetManagement.Core.Exceptions;

namespace AssetManagement.Application.Exceptions
{
    public class AccessDeniedException : Exception
    {
        private readonly ErrorCode _errorCode;

        public AccessDeniedException(ErrorCode errorCode) : base(errorCode.GetMessage())
        {
            _errorCode = errorCode;
        }

        public ErrorCode GetErrorCode()
        {
            return _errorCode;
        }


    }
}
