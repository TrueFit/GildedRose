namespace GildedRose.Persistence.Core
{

   public class DataExecutor : IDataExecutor
   {
      public TReturn ExecuteQuery<TReturn>(IDataQuery<TReturn> query)
      {
         return query.Execute();
      }

      public void ExecuteCommand(IDataCommand command)
      {
         command.Execute();
      }

   }
}
