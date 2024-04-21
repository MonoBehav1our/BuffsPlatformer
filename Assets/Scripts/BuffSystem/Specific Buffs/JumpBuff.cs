public class JumpBuff : Buff<PlayerController>
{
    public JumpBuff(BuffConfig config, PlayerController service) : base(config, service) { }

    public override bool UseBuff()
    {
        return _service.Jump();
    }
}
