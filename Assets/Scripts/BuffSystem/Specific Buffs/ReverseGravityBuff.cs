public class ReverseGravityBuff : Buff<GravityController>
{
    public ReverseGravityBuff(BuffConfig config, GravityController service) : base(config, service) { }

    public override bool UseBuff()
    {
        return _service.ReverseGravity();
    }
}
