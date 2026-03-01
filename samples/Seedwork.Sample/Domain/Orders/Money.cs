using Seedwork.Domain;
using Seedwork.Guard;

namespace Seedwork.Sample.Domain.Orders;

public class Money : ValueObject
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }

    public Money(decimal amount, string currency)
    {
        DomainGuard.AgainstNegative(amount, nameof(amount));
        DomainGuard.AgainstNullOrWhiteSpace(currency, nameof(currency));

        Amount = amount;
        Currency = currency;
    }

    private Money() { Currency = string.Empty; }

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Cannot add money with different currencies.");

        return new Money(Amount + other.Amount, Currency);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}
