public class LasersBuff : Buff<LasersController>
{
    public LasersBuff(BuffConfig config, LasersController service) : base(config, service) { }

    public override bool UseBuff()
    {
        return _service.ChangeState();
    }
}
