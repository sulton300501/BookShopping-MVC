using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BookShopping.Repositories
{

    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpcontextAccessor;


        public CartRepository(ApplicationDbContext context, UserManager<IdentityUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpcontextAccessor = contextAccessor;
        }



        public  async Task<int> AddItem(int bookId , int qty)
        {

            string userid = GetUserId();
            using var transaction =  _context.Database.BeginTransaction();
            try
            {

             
                if (string.IsNullOrEmpty(userid))
                    throw new UnauthorizedAccessException(" User is  not logges-in ");
                var cart = await GetCart(userid);

                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userid

                    };
                    _context.ShoppingCarts.Add(cart);
            

                }
                _context.SaveChanges();



                // cart detail section
                var cartItem = _context.CartDetails.FirstOrDefault(a => a.ShoppingCart_id == cart.Id && a.BookId == bookId);
                if(cartItem is not null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    var book = _context.Books.Find(bookId);
                    cartItem = new CartDetail
                    {
                        BookId = bookId,
                        ShoppingCart_id = cart.Id,
                        Quantity = qty
                       


                    };
                    _context.CartDetails.Add(cartItem);
                }
                _context.SaveChanges();
                transaction.Commit();
              



            }catch (Exception ex)
            {
               
            }

            var cartItemCount = await GetCartItemCount(userid);
            return cartItemCount;


        }




        public async Task<ShoppingCart> GetUserCart()
        {

            var userId = GetUserId();
            if (userId == null)
                throw new InvalidOperationException("Invalid userId");

            var shoppingCart = await _context.ShoppingCarts
                .Include(a=>a.Cartsdetails)
                .ThenInclude(a=>a.Book)
                .ThenInclude(a=>a.Genre)
                .Where(a=>a.UserId==userId).FirstOrDefaultAsync();


            return shoppingCart;
          
        }




        public async Task<int> RemoveItem(int bookId)
        {

            string userid = GetUserId();

           /* using var transaction = _context.Database.BeginTransaction();*/
            try
            {

              
                if (string.IsNullOrEmpty(userid))
                    throw new Exception("user is not logged-in");
                var cart = await GetCart(userid);

                if (cart is null)
                {
                    throw new Exception("Invalid Cart");

                }
                 
                // cart detail section
                var cartItem = _context.CartDetails
                    .FirstOrDefault(a => a.ShoppingCart_id == cart.Id && a.BookId == bookId);
               

                if (cartItem is  null)
                {
                    throw new Exception("Not Items in cart");
                }
                else if(cartItem.Quantity==1)
                {
                    _context.CartDetails.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity = cartItem.Quantity - 1;

                }
                _context.SaveChanges();
            /*    transaction.Commit();*/ 



            }
            catch (Exception ex)
            {
               
            }

            var cartItemCount = await GetCartItemCount(userid);
            return cartItemCount;

        }







        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _context.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }

     

        public async Task<int> GetCartItemCount(string userId="")
        {
            if(string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from cart in _context.ShoppingCarts
                              join cartDetail in _context.CartDetails
                              on cart.Id equals cartDetail.ShoppingCart_id
                              where cart.UserId == userId 
                              select new { cartDetail.Id }
                       ).ToListAsync();

            return data.Count;


        }

      

        private string GetUserId()
        {
            var principal = _httpcontextAccessor.HttpContext.User;
            var userid = _userManager.GetUserId(principal);
            return userid;
        }














    }
}
