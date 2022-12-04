using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftCode.BBS.Common.Helper
{
    public static class UtilConvert
    {
        public static bool ObjToBool(this object thisValue)
        {
            bool reval = false;

            if(thisValue != null && thisValue != DBNull.Value && bool.TryParse(thisValue.ToString(),out reval))
                return reval;

            return reval;
        }
    }
}
