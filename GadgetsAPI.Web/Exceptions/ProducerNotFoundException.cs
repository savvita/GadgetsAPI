namespace GadgetsAPI.Web.Exceptions
{
    public class ProducerNotFoundException : Exception
    {
        public int ProducerId { get; }

        public ProducerNotFoundException(int id) : base($"Producer with id {id} is not found")
        {
            ProducerId = id;
        }
    }
}
