using Application.Common.Dependencies.DataAccess;
using Infrastructure.ApplicationDependencies.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ApplicationDependencies
{
    public static class Startup
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddScoped<ISurveyCategoryRepository, SurveyCategoryRepository>();
            //services.AddScoped<ISurveyRepository, SurveyRepository>();
            //services.AddScoped<IQuestionCategoryRepository, QuestionCategoryRepository>();
            //services.AddScoped<IQuestionRepository, QuestionRepository>();
            //services.AddScoped<ISurveyOrderRepository, SurveyOrderRepository>();
            //services.AddScoped<ICustomerRepository, CustomerRepository>();
            //services.AddScoped<IQuestionChoiceRepository, QuestionChoiceRepository>();
            //services.AddScoped<ISurveyAnswerRepository, SurveyAnswerRepository>();
            //services.AddScoped<ICustomerSurveyOrderRepository, CustomerSurveyOrderRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //services.AddTransient<IDateTime, DateTimeService>();
            //services.AddTransient<IStockStatisticsService, StockStatisticsService>();
        }
    }
}