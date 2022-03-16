using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Persistence.Settings
{
    public class ApplicationDbSettings
    {
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