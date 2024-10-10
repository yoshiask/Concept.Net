using System;

namespace ConceptNet.Models;

public enum ConceptNetEntityType : byte
{
    Unknown,

    /// <summary>
    /// Also known as edges (as of 5.5, these are the same thing).
    /// </summary>
    Assertion,

    /// <summary>
    /// Also known as terms (words and phrases in a particular language).
    /// </summary>
    Concept,

    /// <summary>
    /// Broad sources of knowledge.
    /// </summary>
    Dataset,

    /// <summary>
    /// Language-independent relations, such as <c>/r/IsA</c>.
    /// </summary>
    Relation,

    /// <summary>
    /// Knowledge sources, which can be human contributors, Web sites, or automated processes.
    /// </summary>
    Source,

    /// <summary>
    /// Conjunctions of sources that were used together to create an assertion.
    /// </summary>
    And,

    [Obsolete("In 5.4 and earlier, these represented 'edges' that were not yet combined into assertions.")]
    Edge,

    [Obsolete("In 5.4 and earlier, these represented Creative Commons license terms.")]
    License,

    [Obsolete("Each assertion can come from multiple conjunctions of sources. In 5.3 and earlier, these conjunctions were combined into one big disjunction, with a URI in the /or/ namespace.")]
    Or,
}

public enum SynsetType : byte
{
    Unknown,

    Noun,
    Verb,
    Adjective,
    AdjectiveSatellite,
    Adverb
}
