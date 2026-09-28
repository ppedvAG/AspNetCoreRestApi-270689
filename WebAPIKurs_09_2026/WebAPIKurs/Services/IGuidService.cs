namespace WebAPIKurs.Services
{
    public interface IGuidService
    {
        public string GetGuid();
    }

    public interface ISingletonGuidService : IGuidService
    {
        //Leerimplementierung

        //Eventuell könnte man auch hier spezifizische Singleton-Hinterlegen
    }

    public interface IScopedGuidService : IGuidService
    {
        
    }

    public interface ITransientGuidService : IGuidService
    {

    }
}
