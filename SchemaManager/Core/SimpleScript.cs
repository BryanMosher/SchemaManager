using Utilities.Data;

namespace SchemaManager.Core
{
	public class SimpleScript : ScriptBase, ISimpleScript
	{
		private readonly string _script;
	    private int _timeout;

		public SimpleScript(string script, int timeout)
		{
			_script = script;
		    _timeout = timeout;
		}

		public void Execute(IDbContext context)
		{
			RunAllBatchesFromText(context, _script);
		}

	    protected override int Timeout
	    {
	        get { return _timeout; }
	        set { _timeout = value; }
	    }
	}
}