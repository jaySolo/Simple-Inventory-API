using System.Reflection;

using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
// using NHibernate;
// using NHibernate.AspNetCore.Identity;
// using NHibernate.Cfg;

// using jsolo.simpleinventory.impl.identity;
// using jsolo.simpleinventory.impl.persistance;
// using jsolo.simpleinventory.impl.persistance.configurations;
// using jsolo.simpleinventory.impl.providers;
// using jsolo.simpleinventory.impl.services;
using jsolo.simpleinventory.sys.common.interfaces;



namespace jsolo.simpleinventory.impl;


public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // services.AddTransient<IDateTimeService, DateTimeService>();
        // services.AddTransient<IIdentityService, IdentityService>();

        #region configure and inject NHibernate
        // string dbConnectString = string.Empty;
        // Configuration dbNHibernateConfig;

        // dbConnectString = configuration.GetConnectionString("DbConnectionString");
        // dbNHibernateConfig = NHibernateConfig.GenerateMySqlConfiguration(dbConnectString);
        // // dbNHibernateConfig = NHibernateConfig.GenerateMsSqlConfiguration(dbConnectString);

        // ISessionFactory sessionFactory = dbNHibernateConfig.BuildSessionFactory();

        // services.AddSingleton(sessionFactory);
        // services.AddScoped(factory => sessionFactory.OpenSession());

        // services.AddScoped<IDatabaseContext, DatabaseContext>();
        // services.AddTransient((provider) => new Func<IDatabaseContext>(() => new DatabaseContext(
        //     sessionFactory.OpenSession(),
        //     provider.GetRequiredService<ICurrentUserService>(),
        //     provider.GetRequiredService<IDateTimeService>()
        // )));
        // services.AddScoped<IIdentityDatabaseContext, IdentityDatabaseContext>();
        #endregion

        #region configure and inject Identity
        // services.AddIdentity<User, UserRole>(opts =>
        // {

        //     opts.Lockout.AllowedForNewUsers = true;
        //     opts.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0, 30, 00);
        //     opts.Lockout.MaxFailedAccessAttempts = 5;

        //     opts.Password.RequireDigit = true;
        //     opts.Password.RequiredLength = 6;
        //     opts.Password.RequiredUniqueChars = 1;
        //     opts.Password.RequireLowercase = true;
        //     opts.Password.RequireNonAlphanumeric = false;
        //     opts.Password.RequireUppercase = true;

        //     opts.SignIn.RequireConfirmedAccount = false;
        //     opts.SignIn.RequireConfirmedEmail = false;
        //     opts.SignIn.RequireConfirmedPhoneNumber = false;

        //     opts.User.RequireUniqueEmail = true;
        //     opts.User.AllowedUserNameCharacters =
        //         "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";

        // }).AddTokenProvider<DataProtectorTokenProvider<User>>("Default")
        //   .AddTokenProvider<FirstTimeLoginTokenProvider>("FirstTimeLoginTokenProvider")
        //   .AddUserStore<UserStore<User, UserRole, int>>()
        //   .AddRoleStore<RoleStore<UserRole, int>>();
        #endregion

        return services;
    }
}
