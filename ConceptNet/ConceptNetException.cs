using ConceptNet.Models;
using System;

namespace ConceptNet;

public class ConceptNetException(ConceptNetError error) : Exception(error.Details)
{
    public int Status { get; } = error.Status;
}
