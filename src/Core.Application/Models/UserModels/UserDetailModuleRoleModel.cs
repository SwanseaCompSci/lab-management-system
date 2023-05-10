using AutoMapper;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels
{
    /// <summary>
    /// A <see cref="UserModule"/> model for <see cref="UserDetailModel"/>.
    /// </summary>
    public sealed class UserDetailModuleRoleModel
    {
        /// <summary>
        /// The <see cref="Domain.Entities.Module"/> for which the <see cref="User"/> as a role.
        /// </summary>
        public ModuleModel Module { get; set; } = null!;
        /// <summary>
        /// The role assigned to the <see cref="User"/> for this <see cref="Module"/>.
        /// </summary>
        public string Role { get; set; } = null!;
    }

    /// <summary>
    /// A mapping profile for <see cref="UserDetailModuleRoleModel"/>.
    /// </summary>
    public sealed class UserDetailModuleRoleModelMappingProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="UserDetailModuleRoleModelMappingProfile"/> class.
        /// </summary>
        public UserDetailModuleRoleModelMappingProfile()
        {
            CreateMap<UserModule, UserDetailModuleRoleModel>()
                .ForMember(x => x.Role, m => m.MapFrom(s => s.Role.ToString()));
        }
    }
}
