using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HotPizza.Entities;
using HotPizza.Repositories;
using HotPizza.Services;
using HotPizza.UI;
using HotPizza.Validators;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IPizzaRepository, JsonPizzaRepository>();
        services.AddSingleton<NameFieldValidator>();
        services.AddSingleton<DescriptionFieldValidator>();
        services.AddSingleton<PriceFieldValidator>();
        services.AddSingleton<SizeFieldValidator>();
        services.AddSingleton<IValidator<Pizza>, PizzaValidator>();
        services.AddSingleton<IPizzaService, PizzaService>();
        services.AddSingleton<IConsoleAdapter, ConsoleAdapter>();
        services.AddSingleton<ConsoleUI>();
    });

var host = builder.Build();

var consoleUI = host.Services.GetRequiredService<ConsoleUI>();
await consoleUI.RunAsync();
