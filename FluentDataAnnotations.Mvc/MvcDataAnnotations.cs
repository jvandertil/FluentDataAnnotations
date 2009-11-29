using FluentDataAnnotations.AttributeBuilders;

namespace FluentDataAnnotations.Mvc
{
    /*
     * ModelBinder framework (Abstract ModelBinder with validation and preparation methods0
     * Overridden Default ModelBinder (Validation & Binding)
     * MVC2 ValidationProvider
     */

    public static class MvcDataAnnotationExtensions
    {
        public static BaseDataAnnotationInterface DenyClientModification(this BaseDataAnnotationInterface builder)
        {
            builder.AddRawAttribute(new DenyClientModificationAttribute());

            return builder;
        }
    }
}
