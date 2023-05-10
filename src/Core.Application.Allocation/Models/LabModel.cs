using AutoMapper;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Models
{
    // TODO: Add docs comments
    public sealed class LabModel
    {
        public Guid Id { get; set; }

        public Guid ModuleId { get; set; }
        public Level Level { get; set; }

        public WorkDayOfWeek Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public int MinNumberOfStaff { get; set; }
        public int MaxNumberOfStaff { get; set; }

        public IList<UserModel> AllocatedUsers { get; set; } = new List<UserModel>();
    }

    /// <summary>
    /// A mapping profile for <see cref="LabModel"/>.
    /// </summary>
    public sealed class LabModelMappingProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="LabModelMappingProfile"/> class.
        /// </summary>
        public LabModelMappingProfile()
        {
            CreateMap<Lab, LabModel>()
                .ForMember(x => x.Level, m => m.MapFrom(s => s.Module.Level));
        }
    }
}
