using Seedwork.Domain;

namespace Seedwork.Sample.Domain.Orders;

public record OrderId(Guid Value) : Id<OrderId>(Value);
