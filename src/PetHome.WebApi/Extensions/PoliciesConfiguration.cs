using PetHome.Domain;

namespace PetHome.WebApi.Extensions;

public static class PoliciesConfiguration
{
    public static IServiceCollection AddPoliciesServices(this IServiceCollection services)
    {
        services.AddAuthorization(opt =>
        {
            opt.AddPolicy(
                PolicyMaster.OWNER_READ, policy =>
                   policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.OWNER_READ
                    )
                   )
            );
            opt.AddPolicy(
                PolicyMaster.OWNER_CREATE, policy =>
                   policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.OWNER_CREATE
                    )
                   )
            );
            opt.AddPolicy(
                PolicyMaster.OWNER_UPDATE, policy =>
                   policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.OWNER_UPDATE
                    )
                   )
            );
         
            opt.AddPolicy(
              PolicyMaster.OWNER_DELETE, policy =>
                 policy.RequireAssertion(
                  context => context.User.HasClaim(
                  c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.OWNER_DELETE
                  )
                 )
          );
         
            opt.AddPolicy(
             PolicyMaster.PET_CREATE, policy =>
                policy.RequireAssertion(
                 context => context.User.HasClaim(
                 c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.PET_CREATE
                 )
                )
         );
            opt.AddPolicy(
                       PolicyMaster.PET_UPDATE, policy =>
                          policy.RequireAssertion(
                           context => context.User.HasClaim(
                           c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.PET_UPDATE
                           )
                          )
                   );
            opt.AddPolicy(
                       PolicyMaster.PET_READ, policy =>
                          policy.RequireAssertion(
                           context => context.User.HasClaim(
                           c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.PET_READ
                           )
                          )
                   );
            opt.AddPolicy(
                       PolicyMaster.PET_DELETE, policy =>
                          policy.RequireAssertion(
                           context => context.User.HasClaim(
                           c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.PET_DELETE
                           )
                          )
                   );
        }
        );

        return services;
    }
}