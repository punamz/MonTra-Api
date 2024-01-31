namespace MonTraApi.Domains.Entities;

public class TransactionAggregate : TransactionEntity
{
    public CategoryEntity Category { get; set; } = null!;
}
