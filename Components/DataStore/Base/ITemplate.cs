namespace GildedRose.Store.Contracts
{
    public interface ITemplate
    {
        string RawSql { get; }

        object Parameters { get; }
    }
}
