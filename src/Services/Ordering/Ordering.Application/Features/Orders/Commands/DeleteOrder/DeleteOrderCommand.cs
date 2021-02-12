using MediatR;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    class DeleteOrderCommand : IRequest
    {
        public int Id { get; set; }
    }
}
