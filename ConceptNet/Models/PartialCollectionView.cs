#pragma warning disable CS8618

namespace ConceptNet.Models;

public class PartialCollectionView : LinkedDataEntity
{
    public string? Comment { get; set; }
    public string FirstPage { get; set; }
    public string? NextPage { get; set; }
    public string? PreviousPage { get; set; }
    public string PaginatedProperty { get; set; }
}
