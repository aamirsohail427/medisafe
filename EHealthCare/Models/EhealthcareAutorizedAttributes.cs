using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EHealthCare.Models
{
    public class EhealthcareAutorizedAttributes : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["UserId".ToString()] == null)
            {
                ViewResult result = new ViewResult
                {
                    ViewName = "~/Views/Main/Index.cshtml",
                };
                filterContext.Result = result;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}