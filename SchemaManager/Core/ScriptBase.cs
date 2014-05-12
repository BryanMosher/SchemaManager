using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Utilities.Data;

namespace SchemaManager.Core
{
	public abstract class ScriptBase
	{
		private static readonly Regex BatchSplitter = new Regex(@"^GO\s*$", RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

		private IEnumerable<string> GetBatchesFrom(string sql)
		{
			return from batch in BatchSplitter.Split(sql)
			       let trimmedBatch = batch.Trim()
			       where !string.IsNullOrEmpty(trimmedBatch)
			       select trimmedBatch;
		}

        protected abstract int Timeout { get; set; }

		protected void RunAllBatchesFromText(IDbContext context, string script)
		{
			foreach (var sqlBatch in GetBatchesFrom(script))
			{
				using (var command = context.CreateCommand())
				{
					command.CommandTimeout = (int) TimeSpan.FromMinutes(Timeout).TotalSeconds;

					command.CommandText = sqlBatch;

					command.ExecuteNonQuery();
				}
			}
		}
	}
}