namespace WebAPIKurs.Services
{
    public interface IDateTimeService
    {
        //Möchte die Uhrzeit zurückgeben, bei dem das Objekt erstellt wurde 
        string GetCurrentConstructionTime();
    }
}
