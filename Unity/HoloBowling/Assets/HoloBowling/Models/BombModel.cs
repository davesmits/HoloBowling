using HoloToolkit.Sharing.Spawning;
using HoloToolkit.Sharing.SyncModel;

[SyncDataClass]
public class BombModel : SyncSpawnedObject
{
    [SyncData] public SyncVector3 Force = new SyncVector3("Position");
}
