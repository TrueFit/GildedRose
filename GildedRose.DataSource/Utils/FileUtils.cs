namespace GildedRose.DataSource.Utils
{
    /// <summary>
    /// Helper functions regarding files and file names
    /// </summary>
    public class FileUtils
    {
        /// <summary>
        /// This function is used so that the data base name has only be stored once and can be adjusted by only changing one string
        /// instead of various places throughout the code.
        /// </summary>
        public static string GetDataBaseFileName()
        {
            return "DataBase.db";
        }
    }
}
