namespace ERP.Products.Persistance.MongoDb.Enums;

public enum OutboxStatus
{
    Pending = 0,
    Sent = 1,
    Failed = 2
}
