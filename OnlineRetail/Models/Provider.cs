using System;
namespace OnlineRetail.Models
{
    public abstract class Provider
    {
        OnlineRetailContext context;
        IConfiguration configuration;
        public Provider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        protected OnlineRetailContext Context
        {
            get
            {
                if (context is null)
                {
                    context = new OnlineRetailContext(configuration);
                }
                return context;
            }
        }

    }
}

