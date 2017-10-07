using HoloToolkit.Sharing.Spawning;
using HoloToolkit.Sharing.SyncModel;


[SyncDataClass]
public class PlayfieldModel : SyncSpawnedObject
{
    public PlayfieldModel()
    {

    }

    [SyncData] public SyncBool IsBeingPlaced;
    [SyncData] public SyncBool GameStarted;
}
