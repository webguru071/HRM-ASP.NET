using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMSApp.Models;

namespace EMSApp.Services.Position
{
    public interface IPosition
    {
        List<POSITIONAL_INFO> GetPositionList();
        bool InsertIncrementInfo(INCREMENT_INFO incObj, POSITIONAL_INFO posObj);
        bool UpdateIncrementInfo(INCREMENT_INFO incObj, POSITIONAL_INFO posObj);
    }
}