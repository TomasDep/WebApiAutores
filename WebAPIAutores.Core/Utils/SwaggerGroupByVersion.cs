using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace WebAPIAutores.Core.Utils
{
    public class SwaggerGroupByVersion : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var namespaceController = controller.ControllerType.Namespace;
            var versionApi = namespaceController.Split('.').Last().ToLower();
            controller.ApiExplorer.GroupName = versionApi;
        }
    }
}