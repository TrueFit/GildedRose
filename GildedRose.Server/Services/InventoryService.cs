using GildedRose.Contracts;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GildedRose.Server
{
    public class InventoryService : Inventory.InventoryBase
    {
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(ILogger<InventoryService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}
