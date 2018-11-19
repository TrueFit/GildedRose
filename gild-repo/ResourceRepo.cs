using System;
using System.IO;
using System.Reflection;

namespace gild_repo
{
    public interface IResourceRepo
    {
        string GetResource(string resouceName, Assembly assembly = null);
    }

    public class ResourceRepo : IResourceRepo
    {
        public string GetResource(string resouceName, Assembly assembly = null)
        {
            if (string.IsNullOrWhiteSpace(resouceName)) { throw new ArgumentNullException(nameof(resouceName)); }

            using (var stream = (assembly ?? GetType().Assembly).GetManifestResourceStream(resouceName))
            {
                if(stream == null) { throw new ApplicationException($"Failed to located embedded resource \"{resouceName}\"."); }
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
