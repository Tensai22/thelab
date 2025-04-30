namespace TheLab.Services.Cache
{
    public interface ICacheService
    {
        object Get(string key);
        void Set(string key, object value);
    }
}
