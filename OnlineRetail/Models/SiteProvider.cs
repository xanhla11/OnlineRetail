using System;
namespace OnlineRetail.Models
{
	public class SiteProvider: Provider
	{
        public SiteProvider(IConfiguration configuration) : base(configuration)
        {
        }
        CartRepository cart;
        public CartRepository Carts
        {
            get
            {
                if (cart is null)
                {
                    cart = new CartRepository(Context);
                }
                return cart;
            }
        }
        MemberRepository member;
        public MemberRepository Member
        {
            get
            {
                if (member is null)
                {
                    member = new MemberRepository(Context);
                }
                return member;
            }
        }
        CategoryRepository category;
        public CategoryRepository Category
        {
            get
            {
                if (category is null)
                {
                    category = new CategoryRepository(Context);
                }
                return category;
            }
        }
        ProductRepository products;
        public ProductRepository Products
        {
            get
            {
                if (products is null)
                {
                    products = new ProductRepository(Context);
                }
                return products;
            }
        }
    }
}

