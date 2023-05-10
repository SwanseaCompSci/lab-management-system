using AutoMapper;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels
{
    // TODO: Add docs comments

    public sealed class ModuleDetailUserRoleModel
    {
        public UserModel User { get; set; } = null!;
        public string Role { get; set; } = null!;
    }

    /// <summary>
    /// A mapping profile for <see cref="ModuleDetailUserRoleModel"/>.
    /// </summary>
    public sealed class ModuleDetailUserRoleModelMappingProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ModuleDetailUserRoleModelMappingProfile"/> class.
        /// </summary>
        public ModuleDetailUserRoleModelMappingProfile()
        {
            CreateMap<UserModule, ModuleDetailUserRoleModel>()
                .ForMember(x => x.Role, m => m.MapFrom(x => x.Role.ToString()));
        }
    }
}
