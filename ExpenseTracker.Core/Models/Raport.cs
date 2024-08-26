namespace ExpenseTracker.Core.Models;

public class DateRange
{
  public DateTime Start { get; init; }
  public DateTime End { get; init; }

  public DateRange(DateTime start, DateTime end)
  {
    Start = start;
    End = end;
  }
}

public class Raport
{
  public Guid Id { get; private set; }
  public decimal TotalIncomme { get; private set; }
  public decimal TotalOutcome { get; private set; }
  //public TimeSpan TimeSpan { get; private set; }
  public DateRange DateRange { get; set; }
  public int RaportType { get; private set; }

  private Raport(Guid id, decimal income, decimal outcome, DateRange dateInterval, int raportType)
  {
    Id = id;
    TotalIncomme = income;
    TotalOutcome = outcome;
    DateRange = dateInterval;
    RaportType = raportType;
  }

  public static Raport CreateNew(
      decimal income, decimal outcome, DateRange dateRange, int raportType
  )
  {
    return new(
        Guid.NewGuid(),
        income,
        outcome,
        dateRange,
        raportType
    );
  }

  public static Raport Create(
      Guid id, decimal income, decimal outcome, DateRange dateRange, int raportType
  )
  {
    return new(
        id,
        income,
        outcome,
        dateRange,
        raportType
    );
  }
}
