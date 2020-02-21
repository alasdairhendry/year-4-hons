public enum ConsumeType { Eat, Drink, Inject, Use }

public abstract class ItemConsumable : ItemBaseData
{
    public ConsumeType consumeType { get; protected set; } = ConsumeType.Eat;

    public ItemConsumable (int ID, ConsumeType consumeType) : base ( ID )
    {
        this.consumeType = consumeType;

        switch (this.consumeType)
        {
            case ConsumeType.Eat:
                AddInteractionData ( new InventoryInteractionData ( InventoryInteractionData.InteractType.Eat, (inventoryIndex) =>
                {
                    SoundEffect.Play ( EntityManager.instance.eatSoundEffects.GetRandom () );
                    ConsumeItem ();
                } ) );
                defaultInteractionData = InventoryInteractionData.InteractType.Eat;
                break;
            case ConsumeType.Drink:
                AddInteractionData ( new InventoryInteractionData ( InventoryInteractionData.InteractType.Drink, (inventoryIndex) =>
                {
                    SoundEffect.Play ( EntityManager.instance.drinkSoundEffects.GetRandom () );
                    ConsumeItem ();
                } ) );
                defaultInteractionData = InventoryInteractionData.InteractType.Drink;
                break;
            case ConsumeType.Inject:
                AddInteractionData ( new InventoryInteractionData ( InventoryInteractionData.InteractType.Use, (inventoryIndex) =>
                {
                    SoundEffect.Play ( EntityManager.instance.injectSoundEffects.GetRandom () );
                    ConsumeItem ();
                } ) );
                defaultInteractionData = InventoryInteractionData.InteractType.Use;
                break;
            case ConsumeType.Use:
                AddInteractionData ( new InventoryInteractionData ( InventoryInteractionData.InteractType.Use, (inventoryIndex) =>
                {                    
                    ConsumeItem ();
                } ) );
                defaultInteractionData = InventoryInteractionData.InteractType.Use;
                break;
        }
    }

    protected abstract void ConsumeItem ();
}