using AutoMapper;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabScheduleModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabModels
{
    // TODO: Add docs comments

    public class LabDetailModel : LabModel
    {
        public IEnumerable<UserModel> Users { get; set; } = null!;
        public IEnumerable<LabScheduleModel> LabSchedules { get; set; } = null!;
    }

    public sealed class LabDetailModelMappingProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="LabDetailModelMappingProfile"/> class.
        /// </summary>
        public LabDetailModelMappingProfile()
        {
            CreateMap<Lab, LabDetailModel>()
                .IncludeBase<Lab, LabModel>()
                .ForMember(x => x.Users, m => m.MapFrom(s => s.UserLabs.Select(x => x.User)));
        }
    }

}
