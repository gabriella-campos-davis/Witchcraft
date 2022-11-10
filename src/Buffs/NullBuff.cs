using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.API.Client;
using ProtoBuf;
using BuffStuff;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class NullBuff : Buff {
    [ProtoIgnore]
    private static int DURATION_TICKS = 0 ; //8 seconds
    public void init() {
      SetExpiryInTicks(DURATION_TICKS);
    }
    public override void OnDeath() {
      SetExpiryImmediately();
    }

    public override void OnExpire()
    {
      SetExpiryImmediately();
    }
    public override void OnTick() {
    }
  }