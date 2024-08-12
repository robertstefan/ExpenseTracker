namespace ExpenseTracker.Core.Models;

public class Raport
{
    public Guid Id { get; private set; }
    public decimal Total { get; private set; }
    public TimeSpan TimeSpan { get; private set; }
    public int RaportType { get; private set; }

    private Raport(Guid id, decimal total, TimeSpan timeSpan, int raportType)
    {
        Id = id;
        Total = total;
        TimeSpan = timeSpan;
        RaportType = raportType;
    }

    public static Raport CreateNew(
        decimal total, TimeSpan timeSpan, int raportType
    )
    {
        return new(
            Guid.NewGuid(),
            total,
            timeSpan,
            raportType
        );
    }

    public static Raport Create(
        Guid id, decimal total, TimeSpan timeSpan, int raportType
    )
    {
        return new(
            id,
            total,
            timeSpan,
            raportType
        );
    }
}
