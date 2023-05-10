using AutoMapper;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Models
{
    // TODO: Add docs comments
    public sealed class TimeAvailabilityModel
    {
        public Guid Id { get; set; }

        public WorkDayOfWeek Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public bool IsAllocated { get; set; }
    }

    /// <summary>
    /// A mapping profile for <see cref="TimeAvailabilityModel"/>.
    /// </summary>
    public sealed class TimeAvailabilityModelMappingProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="TimeAvailabilityModelMappingProfile"/> class.
        /// </summary>
        public TimeAvailabilityModelMappingProfile()
        {
            CreateMap<TimeAvailability, TimeAvailabilityModel>();
        }
    }
}
