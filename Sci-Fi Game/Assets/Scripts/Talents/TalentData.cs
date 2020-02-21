using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TalentType { Unbreakable, SleightOfHand, SecondChance, EagleEye, BigShot, BigGulp, QuickStep, Haste, Restoration, Resourceful, Demand, MaterialDesign, Supply, GreenFingers, Glamour }

[CreateAssetMenu]
public class TalentData : ScriptableObject
{
    public string talentName;
    [TextArea] public string talentDescription;
    public Sprite talentSprite;
    public int maxLevels = 6;
    public TalentType talentType;
}
