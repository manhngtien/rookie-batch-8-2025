using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Infrastructure.ConfigurationOptions
{
    public class AssetInfrastructureOptions
    {
        public ConnectionStringOptions ConnectionStrings { get; set; }
    }
}
