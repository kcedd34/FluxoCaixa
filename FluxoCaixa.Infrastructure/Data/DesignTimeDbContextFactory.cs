using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace FluxoCaixa.Infrastructure.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FluxoCaixaDbContext>
    {
        public FluxoCaixaDbContext CreateDbContext(string[] args)
        {
            // Obtém o diretório do projeto de infraestrutura
            var basePath = Directory.GetCurrentDirectory();

            // Se estiver executando no projeto de API, ajusta para encontrar o appsettings.json
            var projectName = Path.GetFileName(basePath);
            if (projectName == "FluxoCaixa.API")
            {
                // Já está no projeto API
            }
            else
            {
                // Tenta navegar para o projeto API
                var apiProjectPath = Path.Combine(Directory.GetParent(basePath).FullName, "FluxoCaixa.API");
                if (Directory.Exists(apiProjectPath))
                {
                    basePath = apiProjectPath;
                }
            }

            Console.WriteLine($"Caminho base para configuração: {basePath}");

            // Carrega o arquivo de configuração
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // Obtém a string de conexão
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                // Fallback para uma string de conexão fixa, caso não consiga ler da configuração
                connectionString = "Server=(localdb)\\mssqllocaldb;Database=FluxoCaixaDb;Trusted_Connection=True;MultipleActiveResultSets=true";
                Console.WriteLine("Usando string de conexão padrão pois não foi encontrada na configuração.");
            }
            else
            {
                Console.WriteLine("String de conexão obtida da configuração.");
            }

            // Cria as opções do DbContext
            var optionsBuilder = new DbContextOptionsBuilder<FluxoCaixaDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new FluxoCaixaDbContext(optionsBuilder.Options);
        }
    }
}