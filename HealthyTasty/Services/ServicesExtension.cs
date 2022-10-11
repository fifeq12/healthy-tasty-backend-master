namespace HealthyTasty.Services
{
    public static class ServicesExtension
    {
        public static void AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IIdentityService, IdentityService>();
        }
    }
}
