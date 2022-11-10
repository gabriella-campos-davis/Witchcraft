using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Common;

namespace witchcraft.config
{
    class WitchcraftConfig 
    {

        public WitchcraftConfig()
        {}

        public static WitchcraftConfig Current { get; set; }

        public static WitchcraftConfig GetDefault()
        {
            WitchcraftConfig defaultConfig = new();


            return defaultConfig;
        }
    }
}