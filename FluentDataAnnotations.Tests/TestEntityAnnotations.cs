namespace FluentDataAnnotations.Tests
{
    public class TestEntityAnnotations : DataAnnotations<TestEntity>
    {
        public TestEntityAnnotations()
        {
            AnnotateProperty(x => x.Name).SetRequired();
            AnnotateProperty(x => x.Description).AddValidation.StringLength.Minimum(15);
        }
    }
}