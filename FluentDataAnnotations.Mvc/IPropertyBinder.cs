using System.Web.Mvc;

namespace FluentDataAnnotations.Mvc
{
    public interface IPropertyBinder<TProperty> : IPropertyBinder where TProperty : class 
    {
        new TProperty BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext);
    }

    public interface IPropertyBinder
    {
        object BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext);
    }
}