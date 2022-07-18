using Application.Common.Dependencies.Services;

namespace Infrastructure.ApplicationDependencies.DataAccess.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}