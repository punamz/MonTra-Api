namespace MonTraApi.Domains.Entities;

public class CategoryAggregate : CategoryEntity
{
    public List<TransactionEntity> Transactions { get; set; } = new List<TransactionEntity>();
}
