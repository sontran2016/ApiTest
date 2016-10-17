using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace API.Filters
{
    /// <summary>
    /// Allow request with no basic code
    /// </summary>
    public class AllowNoBasicCodeAttribute: ActionFilterAttribute
    {
    }
}