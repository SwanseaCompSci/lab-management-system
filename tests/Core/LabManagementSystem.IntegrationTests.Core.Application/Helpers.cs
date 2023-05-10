using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application
{
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
