namespace GildedRose.Persistence.Core
{
   public interface IDataExecutor
   {
      TReturn ExecuteQuery<TReturn>(IDataQuery<TReturn> query);

      void ExecuteCommand(IDataCommand command);
   }
}
