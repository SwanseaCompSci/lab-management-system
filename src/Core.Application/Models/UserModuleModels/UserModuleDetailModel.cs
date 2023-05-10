using AutoMapper;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModuleModels
{
    /// <summary>
    /// A detailed <see cref="UserModule"/> application model.
    /// </summary>
    public class UserModuleDetailModel : UserModuleModel
    {
        /// <summary>
        /// A first name of the <see cref="User"/>.
        /// </summary>
        public string UserFirstName { get; set; } = null!;
        /// <summary>
        /// A surname of the <see cref="User"/>.
        /// </summary>
        public string UserSurname { get; set; } = null!;

        /// <summary>
        /// A name of the <see cref="Module"/>.
        /// </summary>
        public string ModuleName { get; set; } = null!;
    }

    /// <summary>
    /// A mapping profile for <see cref="UserModuleDetailModel"/>.
    /// </summary>
    public sealed class UserModuleDetailModelMappingProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="UserModuleDetailModelMappingProfile"/> class.
        /// </summary>
        public UserModuleDetailModelMappingProfile()
        {
            CreateMap<UserModule, UserModuleDetailModel>()
                .IncludeBase<UserModule, UserModuleModel>()
                .ForMember(x => x.UserFirstName, m => m.MapFrom(s => s.User.FirstName))
                .ForMember(x => x.UserSurname, m => m.MapFrom(s => s.User.Surname))
                .ForMember(x => x.ModuleName, m => m.MapFrom(s => s.Module.Name));
        }
    }
}
