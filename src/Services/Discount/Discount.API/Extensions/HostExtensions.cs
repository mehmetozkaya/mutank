using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();                

                try
                {
                    logger.LogInformation("Migrating postresql database.");

                    using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();

                    using var command = new NpgsqlCommand
                    {
                        Connection = connection
                    };

                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductId VARCHAR(24) NOT NULL,
                                                                Name TEXT,
                                                                Value INT)";
                    command.ExecuteNonQuery();


                    command.CommandText = "INSERT INTO Coupon(ProductId, Name, Value) VALUES('60210c2a1556459e153f0554', 'IPhone Discount', 150);";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon(ProductId, Name, Value) VALUES('60210c2a1556459e153f0555', 'Samsung Discount', 100);";
                    command.ExecuteNonQuery();

                    logger.LogInformation("Migrated postresql database.");

                    // TODO : Apply Polly 

                    //var retry = Policy.Handle<SqlException>()
                    //        .WaitAndRetry(new TimeSpan[]
                    //        {
                    //        TimeSpan.FromSeconds(3),
                    //        TimeSpan.FromSeconds(5),
                    //        TimeSpan.FromSeconds(8),
                    //        });

                    ////if the sql server container is not created on run docker compose this
                    ////migration can't fail for network related exception. The retry options for DbContext only 
                    ////apply to transient exceptions
                    //// Note that this is NOT applied when running some orchestrators (let the orchestrator to recreate the failing service)
                    //retry.Execute(() => InvokeSeeder(seeder, context, services));                                        
                }
                catch (NpgsqlException ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the postresql database");

                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, retryForAvailability);
                    }
                }
            }

            return host;
        }
    }
}
