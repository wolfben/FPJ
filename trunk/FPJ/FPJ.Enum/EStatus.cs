using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPJ.Enum
{
    public enum EStatus
    {
        系统异常 = 0,

        成功 = 1,


        参数错误 = 40001,


        未授权 = 50001,

        签名错误 = 50002,

    }
}
