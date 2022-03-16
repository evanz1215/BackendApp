using Infrastructure.Common.Validation;

namespace Infrastructure.Persistence.Settings
{
    public class UserSeedSettings
    {
        public bool SeedDefaultUser { get; init; }

        [RequiredIf(nameof(SeedDefaultUser), true)]
        public string DefaultUsername { get; init; }

        [RequiredIf(nameof(SeedDefaultUser), true)]
        public string DefaultPassword { get; init; }

        public string DefaultEmail { get; init; }
    }
}