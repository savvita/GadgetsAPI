namespace GadgetsAPI.Web.Exceptions
{
    public class CategoryNotFoundException : Exception
    {
        public int CategoryId { get; }

        public CategoryNotFoundException(int id) : base($"Category with id {id} is not found")
        {
            CategoryId = id;
        }
    }
}
