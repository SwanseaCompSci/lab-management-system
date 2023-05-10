using AutoMapper;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabScheduleModels
{
    // TODO: Add docs comments
    public class LabScheduleModel
    {
        public Guid Id { get; set; }
        public Guid LabId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    public sealed class LabScheduleModelMappingProfile : Profile
    {
        public LabScheduleModelMappingProfile()
        {
            CreateMap<LabSchedule, LabScheduleModel>();
        }
    }
}
