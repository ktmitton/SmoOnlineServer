using System.Numerics;

namespace SuperMarioOdysseyOnline.Server.Core.Models;

public record Location(Vector3 Position, Quaternion Rotation)
{
    public Location() : this(Vector3.Zero, Quaternion.Identity)
    {
    }
}
