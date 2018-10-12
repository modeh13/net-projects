using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace CursoBackend.Util
{
    public class HandleExceptionAttribute : ExceptionFilterAttribute
    {
        public Type Type { get; set; }
        public HttpStatusCode Status { get; set; }

        public override void OnException(HttpActionExecutedContext context)
        {
            var ex = context.Exception;
            if (ex.GetType() is Type)
            {
                var response = context.Request.CreateResponse<string>(Status, ex.Message);
                throw new HttpResponseException(response);
            }
        }
    }
}