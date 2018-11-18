namespace GildedRose.Persistence.Core
{
   public interface IDataQuery<out TReturn>
   {
      TReturn Execute();
   }
}
