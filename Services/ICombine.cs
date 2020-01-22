using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMSApp.Services
{
    public interface ICombine
    {
        string GetTeamLeaderById(long id);
    }
}