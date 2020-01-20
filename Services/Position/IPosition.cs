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
    }
}