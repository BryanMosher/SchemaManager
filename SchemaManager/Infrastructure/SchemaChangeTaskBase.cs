﻿using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Ninject;
using SchemaManager.Core;

namespace SchemaManager.Infrastructure
{
	public abstract class SchemaChangeTaskBase : Task
	{
		private DatabaseVersion _targetRevision = DatabaseVersion.Max;

		[Required]
		public string ConnectionString { get; set; }

		[Required]
		public string PathToChangeScripts { get; set; }

		public virtual string TargetRevision
		{
			get
			{
				return _targetRevision.ToString();
			}
			set
			{
				_targetRevision = DatabaseVersion.FromString(value);
			}
		}

		public override bool Execute()
		{
			using (var kernel = BuildKernel())
			{
				try
				{
					RunSchemaChanges(kernel);
				}
				catch (Exception ex)
				{
					Log.LogErrorFromException(ex);
					return false;
				}
			}

			return true;
		}

		protected virtual StandardKernel BuildKernel()
		{
			return new StandardKernel(new SchemaManagerModule(this, PathToChangeScripts, ConnectionString, _targetRevision));
		}

		protected abstract void RunSchemaChanges(StandardKernel kernel);
	}
}