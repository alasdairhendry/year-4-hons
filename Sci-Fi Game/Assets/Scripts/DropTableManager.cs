using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTableManager : MonoBehaviour
{
    public static DropTableManager instance;

    [SerializeField] private DropTable coinsDropTable;
    [SerializeField] private DropTable ingredientsDropTable;
    [SerializeField] private DropTable gunDropTable;
    [SerializeField] private DropTable meleeDropTable;
    [SerializeField] private DropTable maskDropTable;
    [SerializeField] private DropTable partyHatDropTable;

    public DropTable CoinsDropTable { get => coinsDropTable; set => coinsDropTable = value; }
    public DropTable IngredientsDropTable { get => ingredientsDropTable; set => ingredientsDropTable = value; }
    public DropTable MeleeDropTable { get => meleeDropTable; set => meleeDropTable = value; }
    public DropTable GunDropTable { get => gunDropTable; set => gunDropTable = value; }
    public DropTable MaskDropTable { get => maskDropTable; set => maskDropTable = value; }
    public DropTable PartyHatDropTable { get => partyHatDropTable; set => partyHatDropTable = value; }

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }
    }
}
