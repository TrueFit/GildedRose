namespace gild_repo
{
    public class FileSystemConnectionContext
    {
        public string DataRoot { get; set; }

        public FileSystemConnectionContext() : this(null)
        {
        }

        public FileSystemConnectionContext(string dataRoot)
        {
            DataRoot = dataRoot;
        }
    }
}
