using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTableManager : MonoBehaviour
{
    public static DropTableManager instance;

    [SerializeField] private DropTable coinsDropTable;
    [SerializeField] private DropTable globalDropTable;
    [SerializeField] private DropTable rareDropTable;
    [SerializeField] private DropTable superRareDropTable;

    public DropTable CoinsDropTable { get => coinsDropTable; set => coinsDropTable = value; }
    public DropTable GlobalDropTable { get => globalDropTable; set => globalDropTable = value; }
    public DropTable RareDropTable { get => rareDropTable; set => rareDropTable = value; }
    public DropTable SuperRareDropTable { get => superRareDropTable; set => superRareDropTable = value; }

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }
    }
}
