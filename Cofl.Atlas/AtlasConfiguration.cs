using System;
using System.IO;
using System.Reflection;

namespace Cofl.Atlas
{
    internal class AtlasConfiguration
    {
        internal static AtlasConfiguration DefaultConfiguration = new AtlasConfiguration();

        internal string ApplicationName { get; set; } = Assembly.GetEntryAssembly().GetName().Name;
        internal string FileProviderPath { get; set; } = Directory.GetCurrentDirectory();
    }
}