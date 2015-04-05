using System;

namespace SqlDataMapper
{
	[AttributeUsage(AttributeTargets.Property)]
	public class DbColumnAttribute : Attribute
	{
		public bool Hidden { get; set; }
		public bool PrimaryKey { get; set; }
		public bool ForeignKey { get; set; }
		public string ColumnName { get; set; }
	}
}
