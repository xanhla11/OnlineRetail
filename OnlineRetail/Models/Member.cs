using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineRetail.Models
{
	[Table("members")]
	public class Member
	{
		public string fullName { get; set; }

		public string username { get; set; }

		[Column("memberId")]

		public string id { get; set; }

		public string password { get; set; }

	}
}

