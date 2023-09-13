using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Core
{
    public static class ClaimConstants
    {
        ///<summary>A claim that specifies the subject of an entity</summary>
        public const string Subject = "sub";
    }



    public static class PropertyConstants
    {
        ///<summary>A property that specifies the full name of an entity</summary>
        public const string FullName = "fullname";

    }



    public static class ScopeConstants
    {
        ///<summary>A scope that specifies the roles of an entity</summary>
        public const string Roles = "roles";
    }
}
