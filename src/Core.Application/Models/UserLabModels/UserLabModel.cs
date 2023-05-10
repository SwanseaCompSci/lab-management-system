using AutoMapper;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserLabModels
{
    /// <summary>
    /// A <see cref="UserLab"/> application model.
    /// </summary>
    public sealed class UserLabModel
    {
        /// <summary>
        /// A unique identifier of the <see cref="User"/>.
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// A unique identifier of the <see cref="Lab"/>.
        /// </summary>
        public Guid LabId { get; set; }
    }

    /// <summary>
    /// A mapping profile for <see cref="UserLabModel"/>.
    /// </summary>
    public sealed class UserLabModelMappingProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="UserLabModelMappingProfile"/> class.
        /// </summary>
        public UserLabModelMappingProfile()
        {
            CreateMap<UserLab, UserLabModel>();
        }
    }
}
