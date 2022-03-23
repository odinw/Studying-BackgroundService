using Microsoft.Extensions.Hosting;
using Studing_BackgroundService.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Studing_BackgroundService.Services
{
    public class Cycle : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Factory.StartNew(async () =>
            {
                try
                {
                    Console.WriteLine("hey, background is running");
                    Status clientStatus = Status.Init;
                    while (true)
                    {
                        switch (clientStatus)
                        {
                            case Status.Init:
                                clientStatus = await CheckStatus(clientStatus, Status.DoSomething, 1000);
                                break;

                            case Status.DoSomething:
                                clientStatus = await CheckStatus(clientStatus, Status.GetResult, 3000);
                                break;

                            case Status.GetResult:
                                clientStatus = await CheckStatus(clientStatus, Status.ShowResult, 2000);
                                break;

                            case Status.ShowResult:
                                clientStatus = await CheckStatus(clientStatus, Status.End, 5000);
                                break;

                            case Status.End:
                                clientStatus = await CheckStatus(clientStatus, Status.Init, 1000);
                                break;
                        }
                        await Task.Delay(1);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"error: {e}");
                    throw e;
                }
            });
        }

        async Task<Status> CheckStatus(Status statusNow, Status statusNext, int delayMs)
        {
            Console.WriteLine($"{DateTimeOffset.UtcNow} \t DelayMs : {delayMs}");
            await Task.Delay(delayMs);
            Console.WriteLine($"{DateTimeOffset.UtcNow} \t {statusNow} => {statusNext}");

            return statusNext;
        }
    }
}
