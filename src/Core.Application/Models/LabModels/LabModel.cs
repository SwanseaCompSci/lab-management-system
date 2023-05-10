using AutoMapper;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabModels
{
    // TODO: Add docs comments

    /// <summary>
    /// A <see cref="Lab"/> application model.
    /// </summary>
    public class LabModel
    {
        public Guid Id { get; set; }
        public Guid ModuleId { get; set; }
        public string Name { get; set; } = null!;
        public WorkDayOfWeek Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int MinNumberOfStaff { get; set; }
        public int MaxNumberOfStaff { get; set; }
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
            CreateMap<Lab, LabModel>();
        }
    }
}
