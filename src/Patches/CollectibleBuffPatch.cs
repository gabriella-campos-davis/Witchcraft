using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;
using BuffStuff;

namespace witchcraft 
{
    [HarmonyPatch(typeof (CollectibleObject))]
    [HarmonyPatch("tryEatStop")]

    public class Patch_CollectibleObject_tryEatStop 
    {
        static bool Prefix(float secondsUsed, ItemSlot slot, EntityAgent byEntity) 
        {
            byEntity.Api.Logger.Debug("Harmony patch loaded");
            if(slot.Itemstack.Attributes["buffProps"] != null)
            {
                byEntity.Api.Logger.Debug("buffProps not null");
                foreach(KeyValuePair<string, Type> buff in BuffManager.buffTypes)
                {
                    if(buff.Key == slot.Itemstack.Attributes["buffProps"].ToString())
                    {
                        Buff potionBuff = BuffManager.GetBuffById(buff.Key);
                        byEntity.Api.Logger.Debug(buff.Key);
                        potionBuff.Apply(byEntity);
                    }
                }
            }
            return true;
        }
    }
}