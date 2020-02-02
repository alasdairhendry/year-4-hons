using UnityEngine;

public enum FactionType { Emerys, Graesen, Kyrish, Sylas, Wallon, Xavix}

[CreateAssetMenu]
public class Faction : ScriptableObject
{
    public string factionName;
    [NaughtyAttributes.ResizableTextArea] public string factionDescription;
    [Space]
    public FactionType factionType;
    [Space]
    public string specialisationName;
    [NaughtyAttributes.ResizableTextArea] public string specialisationDescription;
    [Space]
    public string nativeRegion;
    public Faction opposingFaction;
}
