using System.Collections.Generic;
using System.Linq;

public class ItemData_PortalGem : ItemConsumable
{
    public ItemData_PortalGem (int ID) : base ( ID, ConsumeType.Use )
    {
        base.Name = "Portal Gem";
        base.Description = "When broken, teleports the user to any Portal Beam";
        base.category = ItemCategory.Consumable;

        base.IsSellable = true;
        
        

        base.IsStackable = true; 
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 100;
        base.FetchSprite ();
    }

    protected override void ConsumeItem ()
    {
        if (!GameManager.instance.CanFireEvent ( false ))
        {
            return;
        }

        TeleportCanvas.instance.SetDestinations ( (td) =>
        {
            if (!GameManager.instance.CanFireEvent ( true ))
            {
                return;
            }

            td.Teleport ( EntityManager.instance.PlayerCharacter.transform );
            EntityManager.instance.CameraController.SnapToTargetPosition ();
            MessageBox.AddMessage ( "You smash the gem into the ground to unleash it's powers." );
            EntityManager.instance.PlayerInventory.RemoveItem ( base.ID, 1 );
            SoundEffectManager.Play ( AudioClipAsset.UseGem, AudioMixerGroup.SFX );
        }, EntityManager.instance.teleportationBeams );

        TeleportCanvas.instance.Open ();
    }
}
