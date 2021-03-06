﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ItemGear : ItemBaseData
{
    public GearSlot gearSlot;

    public ItemGear (int ID) : base (ID)
    {
       
    }

    protected virtual void EquipItem ()
    {
        if (DragHandler.isDragging) return;
        EntityManager.instance.PlayerCharacter.cGear.EquipGear ( ID );
    }
}