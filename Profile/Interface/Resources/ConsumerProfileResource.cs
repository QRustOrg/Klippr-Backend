namespace Interface.Resources;

public class ConsumerProfileResource
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public LocationResource? Location { get; set; }
    public SavingsStatisticsResource? SavingsStatistics { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}

public class LocationResource
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
}

public class SavingsStatisticsResource
{
    public decimal TotalSavings { get; set; }
    public int TransactionCount { get; set; }
    public decimal AverageTransactionValue { get; set; }
}
