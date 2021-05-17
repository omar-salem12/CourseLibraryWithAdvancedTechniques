namespace CourseLibrary.API.Services
{
    public interface IPropertyCheckerServices
    {
        bool TypeHasProperties<T>(string fields);
    }
}