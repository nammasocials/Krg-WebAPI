using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityLayer
{
    public static class AuthPathUtility
    {
        public static string[] GetAnonymousPaths()
        {
            return AppSettingsReader.GetSection<string[]>("AnonymousPaths") ?? Array.Empty<string>();
        }

        // Check if current path is anonymous
        public static bool IsAnonymousPath(string requestPath)
        {
            if (string.IsNullOrWhiteSpace(requestPath))
                return false;

            var anonymousPaths = GetAnonymousPaths();

            // Normalize slashes and ignore case
            requestPath = requestPath.Replace("//", "/");

            return anonymousPaths.Any(p =>
                !string.IsNullOrWhiteSpace(p) &&
                requestPath.Contains(p, StringComparison.OrdinalIgnoreCase));
        }
    }
}
