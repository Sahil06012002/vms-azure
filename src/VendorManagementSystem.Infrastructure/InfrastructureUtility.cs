using System.Reflection;
using VendorManagementSystem.Application.Dtos.ModelDtos;
using VendorManagementSystem.Models.Enums;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Infrastructure
{
    internal static class InfrastructureUtility
    {
        internal static void UpdateModel<T>(T modelToUpdate, List<UpdateColumnDto> columnData)
        {
            if(object.Equals(modelToUpdate, default(T)))
            {
                throw new ArgumentNullException(nameof(modelToUpdate));
            }
            Type type = modelToUpdate!.GetType();
            foreach(var column in columnData)
            {
                PropertyInfo? propertyInfo = type.GetProperty(column.ColumnName);
                var newValue = column.NewValue;
                if(propertyInfo != null)
                {
                    Type propertyType = propertyInfo.PropertyType;
                    if (propertyType == typeof(int))
                    {
                        propertyInfo.SetValue(modelToUpdate, int.Parse(newValue));
                    }
                    else if (propertyType == typeof(double))
                    {
                        propertyInfo.SetValue(modelToUpdate, Double.Parse(newValue));
                    }
                    else if (propertyType == typeof(decimal))
                    {
                        propertyInfo.SetValue(modelToUpdate, Decimal.Parse(newValue));
                    }
                    else if (propertyType == typeof(bool))
                    {
                        propertyInfo.SetValue(modelToUpdate, Boolean.Parse(newValue));
                    }
                    else if(propertyType == typeof(Country))
                    {
                        var value = (Country)Enum.Parse(typeof(Country), newValue);
                        propertyInfo.SetValue(modelToUpdate, value);
                    }
                    else if (propertyType == typeof(Salutation))
                    {
                        var value = (Salutation)Enum.Parse(typeof(Salutation), newValue);
                        propertyInfo.SetValue(modelToUpdate, value);
                    }
                    else if (propertyType == typeof(Currency))
                    {
                        var value = (Currency)Enum.Parse(typeof(Currency), newValue);
                        propertyInfo.SetValue(modelToUpdate, value);
                    }
                    else if (propertyType == typeof(PaymentTerms))
                    {
                        var value = (PaymentTerms)Enum.Parse(typeof(PaymentTerms), newValue);
                        propertyInfo.SetValue(modelToUpdate, value);
                    }
                    else if (propertyType == typeof(AddressTypes))
                    {
                        var value = (AddressTypes)Enum.Parse(typeof(AddressTypes), newValue);
                        propertyInfo.SetValue(modelToUpdate, value);
                    }
                    else
                    {
                        propertyInfo.SetValue(modelToUpdate, newValue);
                    }
                }
            }

        }
    }
}
