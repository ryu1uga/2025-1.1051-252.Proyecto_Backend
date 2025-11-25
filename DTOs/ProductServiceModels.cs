namespace Loop.DTOs.Common
{
    public record ProductApiResponse(bool success, ProductData? data);

    public record ProductData(Guid Id, Guid BusinessId, Guid CategoryId, string Description, int Status, float Price);
}
