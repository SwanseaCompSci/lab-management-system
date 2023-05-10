using AutoMapper;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Models
{
    // TODO: Add docs comments
    public sealed class UserModel
    {
        public Guid Id { get; set; }
        public Level AchievedLevel { get; set; }
        public int MaxWeeklyWorkHours { get; set; }

        public ICollection<TimeAvailabilityModel> TimeAvailabilities { get; set; } = new List<TimeAvailabilityModel>();
        public IEnumerable<ModulePreferenceModel> ModulePreferences { get; set; } = Array.Empty<ModulePreferenceModel>();
    }

    /// <summary>
    /// A mapping profile for <see cref="UserModel"/>.
    /// </summary>
    public sealed class UserModelMappingProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="UserModelMappingProfile"/> class.
        /// </summary>
        public UserModelMappingProfile()
        {
            CreateMap<User, UserModel>();
        }
    }
}
