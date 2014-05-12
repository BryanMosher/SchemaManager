using System.Collections.Generic;
using System.IO;
using System.Linq;
using SchemaManager.Core;

namespace SchemaManager.AlwaysRun
{
	public class FileSystemAlwaysRunScriptsProvider : IProvideAlwaysRunScripts
	{
		private readonly string _pathToScripts;
        private readonly SchemaManagerGlobalOptions _globalOptions;

		public FileSystemAlwaysRunScriptsProvider(string pathToScripts, SchemaManagerGlobalOptions globalOptions)
		{
			_pathToScripts = pathToScripts;
		    _globalOptions = globalOptions;
		}

		public IEnumerable<ISimpleScript> GetScripts()
		{
            return Directory.EnumerateFiles(_pathToScripts).Select(script => new SimpleScript(File.ReadAllText(script), (int)_globalOptions.Timeout.TotalSeconds));
		}
	}
}