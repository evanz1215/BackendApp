using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Persistence.Settings
{
    public class ApplicationDbSettings
    {

        /// <summary>
        /// db Provider mssql or mysql
        /// https://learn.microsoft.com/zh-tw/ef/core/providers/?tabs=dotnet-core-cli
        /// </summary>
        [Required]
        public string Provider { get; set; }


        /// <summary>
        /// Specifies if migration should be attempted automatically during configuration.
        /// </summary>
        [Required]
        public bool? AutoMigrate { get; init; }

        /// <summary>
        /// Specifies if seeding should be attempted automatically during configuration.
        /// </summary>
        [Required]
        public bool? AutoSeed { get; init; }

        
    }
}