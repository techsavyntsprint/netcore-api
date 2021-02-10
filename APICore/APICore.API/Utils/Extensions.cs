using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICore.API.Utils
{
    public static class Extensions
    {
        public static string ToForgotPasswordEmail(this string resource, string password)
        {
            return resource.Replace("#PASSWORD#", password);
        }
    }
}
