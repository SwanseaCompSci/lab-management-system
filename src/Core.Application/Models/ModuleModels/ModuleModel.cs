using AutoMapper;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels
{
    /// <summary>
    /// A <see cref="Module"/> application model.
    /// </summary>
    public class ModuleModel
    {
        /// <summary>
        /// Module unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Module name.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Module code.
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// Module level.
        /// </summary>
        public string Level { get; set; } = null!;
    }

    /// <summary>
    /// A mapping profile for <see cref="ModuleModel"/>.
    /// </summary>
    public sealed class ModuleModelMappingProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ModuleModelMappingProfile"/> class.
        /// </summary>
        public ModuleModelMappingProfile()
        {
            CreateMap<Module, ModuleModel>()
                .ForMember(x => x.Level, m => m.MapFrom(s => s.Level.ToString()));
        }
    }
}
