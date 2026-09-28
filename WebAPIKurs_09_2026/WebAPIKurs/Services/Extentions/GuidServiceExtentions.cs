namespace WebAPIKurs.Services.Extentions
{
    public static class GuidServiceExtentions
    {
        public static void AddGuidServce(this IServiceCollection collection)
        {
            collection.AddScoped<IScopedGuidService, GuidService>();
        }
    }
}
