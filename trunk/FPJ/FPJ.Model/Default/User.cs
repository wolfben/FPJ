using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPJ.Model.Default
{
    [Table("User")]
    public partial class User
    {
        [Write(false)]
        public string ExtensionProp { get; set; }
    }
}
