using AssetManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Core.Interfaces.Services.Auth
{
    public interface ITokenService
    {
        string GenerateRefreshToken();
        string GenerateToken(Account user, IList<string> roles);
    }
}
