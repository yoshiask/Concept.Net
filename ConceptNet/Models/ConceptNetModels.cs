#pragma warning disable CS8618

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ConceptNet.Models;

public abstract class ConceptNetEntity : LinkedDataEntity
{
    public ConceptNetError? Error { get; set; }
    public abstract ConceptNetEntityType EntityType { get; }
}

public class ConceptNetQuery : ConceptNetEntity
{
    public override ConceptNetEntityType EntityType => ConceptNetEntityType.Unknown;
    public List<ConceptNetEdge> Edges { get; set; }
    public PartialCollectionView? View { get; set; }

    [MemberNotNullWhen(true, nameof(View))]
    public bool TryGetNextPage([NotNullWhen(true)] out string? nextPage)
    {
        nextPage = View?.NextPage;
        return View != null && View.NextPage != null;
    }
}

public class ConceptNetError
{
    public string Details { get; set; }
    public int Status { get; set; }
    public override string ToString() => $"Error {Status}: {Details}";
}

public class ConceptNetEdge : ConceptNetEntity
{
    public override ConceptNetEntityType EntityType => ConceptNetEntityType.Assertion;
    public string Dataset { get; set; }
    public string License { get; set; }
    public object Start { get; set; }
    public object End { get; set; }
    public string? SurfaceText { get; set; }
    public double Weight { get; set; }
    public List<ConceptNetSource> Sources { get; set; }

    [JsonPropertyName("rel")]
    public ConceptNetRelation Relation { get; set; }

    public override string ToString() => SurfaceText ?? Id;
}

public class ConceptNetRelation : ConceptNetEntity
{
    public override ConceptNetEntityType EntityType => ConceptNetEntityType.Relation;
    public string Label { get; set; }
}

public class ConceptNetNode : ConceptNetRelation
{
    public override ConceptNetEntityType EntityType => ConceptNetEntityType.Concept;
    public string Language { get; set; }
    public string Term { get; set; }
    public string SenseLabel { get; set; }
}

public class ConceptNetSource : ConceptNetEntity
{
    public override ConceptNetEntityType EntityType => ConceptNetEntityType.Source;
    public string Contributor { get; set; }
    public string Process { get; set; }
}
