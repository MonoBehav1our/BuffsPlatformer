public abstract class Buff<T> : IAction where T : IService
{
    protected T _service;
    protected BuffConfig _config;

    public Buff(BuffConfig config, T service)
    {
        _config = config;
        _service = service;
    }

    public abstract bool UseBuff();

    public BuffConfig GetConfig() 
    {
        return _config;
    }
}
