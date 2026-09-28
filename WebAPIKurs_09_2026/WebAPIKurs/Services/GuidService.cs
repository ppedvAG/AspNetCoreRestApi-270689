namespace WebAPIKurs.Services
{
    public class GuidService : IScopedGuidService, ITransientGuidService, ISingletonGuidService
    {
        private Guid _guid;
        public GuidService()
        {
            _guid = Guid.NewGuid();
        }
        public string GetGuid()
        {
            return _guid.ToString();
        }
    }
}
