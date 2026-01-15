using System;

namespace RandomizerCore.Classes.Handlers.State;

[Serializable]
public class RandomStateElement<T>(T source, bool hasObtainedSource)
{
    public readonly T source = source;
    public bool hasObtainedSource = hasObtainedSource;
}
