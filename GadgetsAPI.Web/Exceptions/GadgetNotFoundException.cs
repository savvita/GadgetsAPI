namespace GadgetsAPI.Web.Exceptions
{
    public class GadgetNotFoundException : Exception
    {
        public int GadgetId { get; }

        public GadgetNotFoundException(int id) : base($"Gadget with id {id} is not found")
        {
            GadgetId = id;
        }
    }
}
