using CurrencyService.Caching;
using CurrencyService.Common;
using CurrencyService.Cron;
using CurrencyService.Provider;
using CurrencyService.Services;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

#region //Redis Configuration
builder.Services.AddStackExchangeRedisCache(options =>
            {
                string connection = builder.Configuration.GetConnectionString("Redis")!;
                options.Configuration = connection;
            });
#endregion

#region //Quartz Configuration
builder.Services.AddQuartz(q =>
{
    int hour = builder.Configuration.GetValue<int>("Schedule:Hour");

    q.UseMicrosoftDependencyInjectionJobFactory();

    q.ScheduleJob<UpdateCurrencyJob>(trigger => trigger
        .WithIdentity(nameof(UpdateCurrencyJob))
        .StartNow()
        .WithSimpleSchedule(
            options => options.RepeatForever()
            .WithIntervalInSeconds(10)
        ));
    // .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(hour, 00)));
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
#endregion

builder.Services.AddSingleton<ICurrencyExchangeProvider, CurrencyExchangeProvider>();

builder.Services.AddScoped<IMessageProducer, MessageProducer>();

builder.Services.AddScoped<ICacheManager, CacheManager>();

var app = builder.Build();

app.Run();
