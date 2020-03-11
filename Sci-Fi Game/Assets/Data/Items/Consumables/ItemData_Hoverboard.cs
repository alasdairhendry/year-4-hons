public class ItemData_Hoverboard : ItemConsumable
{
    public ItemData_Hoverboard (int ID) : base ( ID, ConsumeType.Use )
    {
        base.Name = "Hoverboard";
        base.Description = "Permanently unlocks a hoverboard";
        base.category = ItemCategory.Consumable;

        base.IsSellable = false;
        base.IsStackable = false; 
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 2500;
        base.FetchSprite ();
    }

    protected override void ConsumeItem ()
    {
        if (!GameManager.instance.CanFireEvent ( true ))
        {
            return;
        }

        UnityEngine.GameObject.FindObjectOfType<HoverboardCanvas> ().SetActive ();
        MessageBox.AddMessage ( "You activate the hoverboard. It can be summoned by using the button above the hotbar.", MessageBox.Type.Warning );
        SoundEffectManager.Play ( AudioClipAsset.UseGem, AudioMixerGroup.SFX );

        EntityManager.instance.PlayerInventory.RemoveItem ( base.ID, 1 );
    }
}
