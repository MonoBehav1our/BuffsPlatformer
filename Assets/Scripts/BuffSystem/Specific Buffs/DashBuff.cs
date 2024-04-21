public class DashBuff : Buff<PlayerController>
{
    public DashBuff(BuffConfig config, PlayerController service) : base(config, service) { }

    public override bool UseBuff()
    {
        return _service.Dash();
    }
}
