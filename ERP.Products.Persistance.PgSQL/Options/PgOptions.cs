namespace ERP.Products.Persistance.PgSQL.Options;

public class PgOptions
{
    public const string SectionName = "PgDbSettings";

    public string WriteConnectionString { get; set; } = string.Empty;
    public string ReadConnectionString { get; set; } = string.Empty;
}




