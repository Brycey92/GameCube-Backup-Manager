﻿using System.Runtime.InteropServices;

namespace GCBM.tools.Interop
{
    public enum IMAPI_FORMAT2_TAO_WRITE_ACTION
    {
        [TypeLibVar(0x40)] IMAPI_FORMAT2_TAO_WRITE_ACTION_UNKNOWN = 0,
        IMAPI_FORMAT2_TAO_WRITE_ACTION_PREPARING = 1,
        IMAPI_FORMAT2_TAO_WRITE_ACTION_WRITING = 2,
        IMAPI_FORMAT2_TAO_WRITE_ACTION_FINISHING = 3,
        IMAPI_FORMAT2_TAO_WRITE_ACTION_VERIFYING = 4
    }
}