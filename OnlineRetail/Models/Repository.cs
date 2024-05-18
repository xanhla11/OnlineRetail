using System;
namespace OnlineRetail.Models
{
    public abstract class Repository
    {
        protected OnlineRetailContext context;
        public Repository(OnlineRetailContext context)
        {
            this.context = context;
        }
    }
}

