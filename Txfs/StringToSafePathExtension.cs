using System.IO;

namespace Txfs
{
    public static class StringToSafePathExtension
    {
        /// <summary>
        /// Makes the path absolute, relative to the executing assembly path if the
        /// path has not been rooted.
        /// </summary>
        /// <param name="path">The path to safify.</param>
        /// <returns>The resulting absolute path.</returns>
        public static string ToSafePath(this string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }

            var assemblyPath = Path.GetDirectoryName(typeof(StringToSafePathExtension).Assembly.Location);

            return Path.Combine(assemblyPath, path);
        }
    }
}
