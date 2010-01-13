using System.Web.Mvc;

namespace FluentDataAnnotations.Mvc
{
    public interface IPropertyBinder
    {
        object BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, string propertyName);
    }
}