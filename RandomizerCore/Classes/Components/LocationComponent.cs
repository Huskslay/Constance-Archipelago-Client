using RandomizerCore.Classes.Data.Types.Locations;
using UnityEngine;

namespace RandomizerCore.Classes.Components;

public class LocationComponent : MonoBehaviour
{
    public ALocation Location { get; private set; }

    public void Set(ALocation location)
    {
        Location = location;
    }
}