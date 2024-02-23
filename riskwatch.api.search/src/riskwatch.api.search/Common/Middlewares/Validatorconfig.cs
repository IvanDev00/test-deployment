using FluentValidation.AspNetCore;
using System.Reflection;

namespace riskwatch.api.search.Common.Middlewares
{
    #region Validator
    public static class Validatorconfig
    {
        public static void ConfigureValidators(this IMvcBuilder builder)
        {
            builder.AddFluentValidation(options =>
            {
                // Validate child properties and root collection elements
                options.ImplicitlyValidateChildProperties = true;
                options.ImplicitlyValidateRootCollectionElements = true;

                // Automatic registration of validators in assembly
                options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            });
        }
    }
    #endregion
}
