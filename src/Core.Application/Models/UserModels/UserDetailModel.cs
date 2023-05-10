using AutoMapper;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.TimeAvailabilityModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels
{
    // TODO: Add docs comments

    /// <summary>
    /// A detailed <see cref="User"/> application model.
    /// </summary>
    public class UserDetailModel : UserModel
    {
        public IEnumerable<UserDetailModulePreferenceModel> ModulePreferences { get; set; } = null!;
        public IEnumerable<UserDetailModuleRoleModel> ModuleRoles { get; set; } = null!;
        public IEnumerable<TimeAvailabilityModel> TimeAvailabilities { get; set; } = null!;
    }

    /// <summary>
    /// A mapping profile for <see cref="UserDetailModel"/>.
    /// </summary>
    public sealed class UserDetailModelMappingProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="UserDetailModelMappingProfile"/> class.
        /// </summary>
        public UserDetailModelMappingProfile()
        {
            CreateMap<User, UserDetailModel>()
                .IncludeBase<User, UserModel>()
                .ForMember(x => x.ModuleRoles, m => m.MapFrom(s => s.UserModules));
        }
    }
}
