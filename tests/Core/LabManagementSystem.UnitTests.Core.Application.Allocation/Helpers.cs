using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Allocation
{
    // TODO: Move to a shared project
    internal static class Helpers
    {
        public static void SetPrivatePropertyValue<T>(this BaseEntity entity, string propName, T newValue)
        {
            var propertyInfo = entity.GetType().GetProperty(propName);
            if (propertyInfo is not null)
            {
                propertyInfo.SetValue(entity, newValue);
            }
        }
    }
}
