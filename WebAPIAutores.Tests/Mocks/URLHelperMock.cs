using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Diagnostics.CodeAnalysis;

namespace WebAPIAutores.Tests.Mocks
{
    public class URLHelperMock : IUrlHelper
    {
        public ActionContext ActionContext => throw new NotImplementedException();

        public string Action(UrlActionContext actionContext)
        {
            throw new NotImplementedException();
        }

        public string Link(string routeName, object values)
        {
            return "";
        }

        [return: NotNullIfNotNull("contentPath")]
        public string Content(string contentPath)
        {
            throw new NotImplementedException();
        }

        public bool IsLocalUrl([NotNullWhen(true)] string url)
        {
            throw new NotImplementedException();
        }

        public string RouteUrl(UrlRouteContext routeContext)
        {
            throw new NotImplementedException();
        }
    }
}