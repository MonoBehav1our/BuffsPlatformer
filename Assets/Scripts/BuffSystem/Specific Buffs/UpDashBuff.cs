public class UpDashBuff : Buff<PlayerController>
{
    public UpDashBuff(BuffConfig config, PlayerController service) : base(config, service) { }

    public override bool UseBuff()
    {
        return _service.UpDash();
    }
}
