using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Application.DTOs.Users
{
    public class UpdateUserRequest
    {
        public required DateTime DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public required DateTime JoinedDate { get; set; }
        public required string Type { get; set; }
    }
}
