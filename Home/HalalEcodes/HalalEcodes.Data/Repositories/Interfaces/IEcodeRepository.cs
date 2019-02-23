namespace HalalEcodes.Data.Repositories.Interfaces
{
    /// <summary>
    /// Поиск по коду
    /// </summary>
    public interface IEcodeRepository : IRepository<Ecode>
    {
        Ecode GetByCode(string code);
    }
}
