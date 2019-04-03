using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configurarion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectModel.Application;
using ProjectModel.ConsoleApp.Dto;
using ProjectModel.GoogleBooksApiAdapter;
using ProjectModel.Domain.Services;
using Newtonsoft.Json;
using AutoMapper;
using System.Threading.Tasks;

namespace ProjectModel.ConsoleApp
{
    class Program
    {
        private static IConfiguration _configuration { get; set; }

        static void Main(string[] args)
        {
            Console.WriteLine("Starting Service");

            _configuration = Startup.Start();

            ServiceProviderFactory serviceProviderFactory = new ServiceProviderFactory(services =>
            {
                services.AddDbReadOnlyAdapter(_configuration.SafeGet<GoogleBooksReadOnlyAdapterConfigurarion>());
                services.AddApplication(_configuration.SafeGet<ApplicationConfiguration>());
            });

            var serviceProvider = serviceProviderFactory.CreateServiceProvider();
            ConsoleService service = new ConsoleService(serviceProvider.GetService<ILivrosService>());

            // Obter consulta default
            ConsoleConfiguration consoleOptions = _configuration.SafeGet<ConsoleConfiguration>();

            IEnumerable<LivroDto> retorno = service.obterLivrosPorTitulo(consoleOptions.ConsultaDefault).Result;

            foreach (LivroDto dto in retorno)
            {
                Console.WriteLine(string.Format("Livro: {0}\n", JsonConvert.SerializeObject(dto)));
            }

            Console.ReadKey();
        }

        /*
        private static readonly IServiceCollection _services = new ServiceCollection();
        private static IConfiguration _configurationRoot;

        public static void Main(string[] args)
        {
            ConfigurarAplicacao(args);
            IniciarProcessamento().Wait();
        }

        private static void ConfigurarAplicacao(string[] args)
        {
            Mapper.Initialize(config =>
            {
                config.AddProfile<ConsoleMapperProfile>();
            });

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            _configurationRoot = builder.Build();

            _services.AddDbReadOnlyAdapter(_configurationRoot.SafeGet<GoogleBooksReadOnlyAdapterConfigurarion>());
            _services.AddApplication(_configurationRoot.SafeGet<ApplicationConfiguration>());
        }

        private static async Task IniciarProcessamento()
        {
            using (var serviceProvider = _services.BuildServiceProvider())
            {
                ConsoleConfiguration consoleOptions = _configurationRoot.SafeGet<ConsoleConfiguration>();
                var consoleService = serviceProvider.GetService<ConsoleService>();
                IEnumerable<LivroDto> retorno = await consoleService.obterLivrosPorTitulo(consoleOptions.ConsultaDefault);

                foreach (LivroDto dto in retorno)
                {
                    Console.WriteLine(string.Format("Livro: {0}\n", JsonConvert.SerializeObject(dto)));
                }
            }
        }
        */
        
        
    }
}
