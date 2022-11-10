using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.API.Client;
using Vintagestory.API;
using System.Reflection;
using System;
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

using witchcraft.config;
using BuffStuff;
using HarmonyLib;

[assembly: ModInfo( "witchcraft",
	Description = "Adds my own flavor of witchcraft to the game.",
	Website     = "",
	Authors     = new []{ "gabb" } )]

namespace witchcraft
{
    public class witchcraft : ModSystem
    {
        private readonly Harmony _harmony = new("witchcraft");
        public override void Start(ICoreAPI api)
        {
            base.Start(api);

            PatchGame();

            try
            {
                var Config = api.LoadModConfig<WitchcraftConfig>("witchcraftconfig.json");
                if (Config != null)
                {
                    api.Logger.Notification("Mod Config successfully loaded.");
                    WitchcraftConfig.Current = Config;
                }
                else
                {
                    api.Logger.Notification("No Mod Config specified. Falling back to default settings");
                    WitchcraftConfig.Current = WitchcraftConfig.GetDefault();
                }
            }
            catch
            {
                WitchcraftConfig.Current = WitchcraftConfig.GetDefault();
                api.Logger.Error("Failed to load custom mod configuration. Falling back to default settings!");
            }
            finally
            {
                
                api.StoreModConfig(WitchcraftConfig.Current, "witchcraftconfig.json");
            }
        }
        public override void StartServerSide(ICoreServerAPI api)
        {
            BuffManager.Initialize(api, this);
            BuffManager.RegisterBuffType("VitalityBuff", typeof(VitalityBuff));
            BuffManager.RegisterBuffType("NullBuff", typeof(NullBuff));
        }

        public override void StartClientSide(ICoreClientAPI api)
        {

        }

        public override void Dispose()
        {
            var harmony = new Harmony("witchcraft");
            harmony.UnpatchAll("witchcraft");
        }

        private void PatchGame()
        {
            Mod.Logger.Event("Applying Harmony patches");
            var harmony = new Harmony("witchcraft");
            var original = typeof(CollectibleObject).GetMethod("tryEAtStop");
            var patches = Harmony.GetPatchInfo(original);
            if (patches != null && patches.Owners.Contains("witchcraft"))
            {
                return;
            }
            harmony.PatchAll();
        }

        private void UnPatchGame()
        {
            Mod.Logger.Event("Unapplying Harmony patches");

            _harmony.UnpatchAll();
        }
    }
}

#pragma warning disable IDE0051 // Remove unused private members

[HarmonyPatch]
public class Patch_CollectibleObject_tryEatStop 
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CollectibleObject), "tryEatStop")]
        private static void Prefix(float secondsUsed, ItemSlot slot, EntityAgent byEntity) 
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
        }
    }