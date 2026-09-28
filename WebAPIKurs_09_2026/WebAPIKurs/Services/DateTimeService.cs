namespace WebAPIKurs.Services
{
    public class DateTimeService : IDateTimeService
    {
        private readonly DateTime _connstructionTime;

        //Beim Konstruktor wird die Uhrzeit gesetzt
        public DateTimeService()
        {
            _connstructionTime = DateTime.Now;
        }

        public string GetCurrentConstructionTime()
        {
            return _connstructionTime.ToString();
        }
    }
}
