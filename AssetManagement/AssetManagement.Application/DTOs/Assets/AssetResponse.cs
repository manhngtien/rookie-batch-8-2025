using AssetManagement.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Application.DTOs.Assets
{
    internal class AssetResponse
    {
        public required string AssetCode { get; set; }
        public required string AssetName { get; set; }
        public required string Specification { get; set; }
        public AssetStatus State { get; set; }
        public ELocation Location { get; set; }
        public DateTime InstalledDate { get; set; }
        public int CategoryId { get; set; }
        public required string CategoryName { get; set; }
    }
}