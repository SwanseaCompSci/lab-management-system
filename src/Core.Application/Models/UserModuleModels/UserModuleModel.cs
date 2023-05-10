using AutoMapper;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModuleModels
{
    /// <summary>
    /// A <see cref="UserModule"/> application model.
    /// </summary>
    public class UserModuleModel
    {
        /// <summary>
        /// A unique identifier of the <see cref="User"/>.
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// A unique identifier of the <see cref="Module"/>.
        /// </summary>
        public Guid ModuleId { get; set; }
        /// <summary>
        /// A <see cref="ModuleRole"/> assigned to the <see cref="User"/> for the <see cref="Module"/>.
        /// </summary>
        public string Role { get; set; } = null!;
    }

    /// <summary>
    /// A mapping profile for <see cref="UserModuleModel"/>.
    /// </summary>
    public sealed class UserModuleModelMappingProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="UserModuleModelMappingProfile"/> class.
        /// </summary>
        public UserModuleModelMappingProfile()
        {
            CreateMap<UserModule, UserModuleModel>()
                .ForMember(x => x.Role, m => m.MapFrom(s => s.Role.ToString()));
        }
    }
}
