using Kernel.Domain.Model.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kernel.Domain.Validation
{
    public static class AnnotationsValidator
    {
        public static void ValidateAnnotations(this ValidatorResult result, object entity, string namePrefix = "")
        {
            if (entity == null)
                return;

            var ctx = new ValidationContext(entity);

            var type = entity.GetType();
            var props = type.GetProperties();

            foreach (var prop in props)
            {
                var value = prop.GetValue(entity, null);

                if (prop.PropertyType.BaseType?.Name == typeof(ValueObject<>).Name)
                {
                    // Se a propriedade for um Objeto de Valor e tiver o Required, Valida as suas propriedades
                    if (Attribute.IsDefined(prop, typeof(RequiredAttribute)))
                    {
                        namePrefix += prop.Name + ".";
                        result.ValidateAnnotations(value, namePrefix);
                    }
                }

                ctx.MemberName = prop.Name;
                var validationList = new List<ValidationResult>();
                Validator.TryValidateProperty(value, ctx, validationList);

                foreach (var validation in validationList)
                {
                    result.AddError(
                        validation.ErrorMessage,
                        prop.ReflectedType?.BaseType?.Name == typeof(ValueObject<>).Name
                            ? namePrefix + prop.Name
                            : prop.Name);
                }
            }
        }
    }
}
