using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace OnlineRetail.Models
{
	public class MemberRepository : Repository
	{
        public MemberRepository(OnlineRetailContext context) : base(context)
        {
        }

        public Member Login(LoginModel obj)
        {
            return context.Members.FromSqlRaw<Member>("SELECT * FROM members WHERE username = @Usr AND password = @Pwd", new SqliteParameter[]
            {
                new SqliteParameter("@Usr", obj.Usr),
                new SqliteParameter("@Pwd" , obj.Pwd)
            }).AsEnumerable().Select(p => new Member
            {
                id = p.id,
                fullName = p.fullName
            }).SingleOrDefault();
        }

        public async Task<int> Add(RegisterModel obj)
        {
            if (string.IsNullOrEmpty(obj.memberId))
            {
                obj.memberId = Guid.NewGuid().ToString(); // Generating a random string for memberId
            }

            context.Members.Add(new Member
            {
                id = obj.memberId,
                fullName = obj.FullName,
                username = obj.Usr,
                password = obj.Pwd
            });
            return await context.SaveChangesAsync();
        }
    }
}

