using AutoMapper;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Models.DashboardModels
{
    // TODO: Add docs comments
    public class WorkShiftModel
    {
        public Guid LabScheduleId { get; set; }
        public DateTime LabScheduleStart { get; set; }
        public DateTime LabScheduleEnd { get; set; }

        public Guid LabId { get; set; }
        public string LabName { get; set; } = null!;

        public Guid ModuleId { get; set; }
        public string ModuleName { get; set; } = null!;
    }

    /// <summary>
    /// A mapping profile for <see cref="WorkShiftModel"/>.
    /// </summary>
    public sealed class WorkShiftModelMappingProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="WorkShiftModelMappingProfile"/> class.
        /// </summary>
        public WorkShiftModelMappingProfile()
        {
            CreateMap<UserLabSchedule, WorkShiftModel>()
                .ForMember(x => x.LabScheduleId, m => m.MapFrom(s => s.LabSchedule.Id))
                .ForMember(x => x.LabScheduleStart, m => m.MapFrom(s => s.LabSchedule.Start))
                .ForMember(x => x.LabScheduleEnd, m => m.MapFrom(s => s.LabSchedule.End))
                .ForMember(x => x.LabId, m => m.MapFrom(s => s.LabSchedule.Lab.Id))
                .ForMember(x => x.LabName, m => m.MapFrom(s => s.LabSchedule.Lab.Name))
                .ForMember(x => x.ModuleId, m => m.MapFrom(s => s.LabSchedule.Lab.Module.Id))
                .ForMember(x => x.ModuleName, m => m.MapFrom(s => s.LabSchedule.Lab.Module.Name));
        }
    }
}
