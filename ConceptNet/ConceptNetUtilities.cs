using ConceptNet.Models;
using System;

namespace ConceptNet;

public static class ConceptNetUtilities
{
    public static ConceptNetEntityType IdentifyEntityType(string id)
    {
        if (id.Length < 4 || id[0] != '/')
            throw new FormatException($"'{id}' is not a valid ConceptNet URI");

        var sepIndex = id[1..].IndexOf('/') + 1;
        var objectKind = id[1..sepIndex];

        return objectKind switch
        {
            "a" => ConceptNetEntityType.Assertion,
            "c" => ConceptNetEntityType.Concept,
            "d" => ConceptNetEntityType.Dataset,
            "r" => ConceptNetEntityType.Relation,
            "s" => ConceptNetEntityType.Source,
            "and" => ConceptNetEntityType.And,

#pragma warning disable CS0618 // Type or member is obsolete, but we want to recognize it here anyway
            "e" => ConceptNetEntityType.Edge,
            "l" => ConceptNetEntityType.License,
            "or" => ConceptNetEntityType.Or,
#pragma warning restore CS0618

            _ => throw new ArgumentException($"'{objectKind}' is a not a valid ConceptNet object type")
        };
    }

    public static bool TryIdentifyEntityType(string id, out ConceptNetEntityType type)
    {
        try
        {
            type = IdentifyEntityType(id);
            return true;
        }
        catch
        {
            type = ConceptNetEntityType.Unknown;
            return false;
        }
    }
}
