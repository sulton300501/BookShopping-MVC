using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookShopping.Repositories
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpcontextAccessor;
        private readonly UserManager<IdentityUser> _userManager;


        public UserOrderRepository(ApplicationDbContext context, IHttpContextAccessor httpcontextAccessor, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _httpcontextAccessor = httpcontextAccessor;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Order>> UserOrders()
        {
            var userid = GetUserId();
            if(string.IsNullOrEmpty(userid))
            {
                throw new Exception("User not is logged-in");
            }
            var orders = await _context.Orders
                .Include(x=>x.OrderStatus)
                .Include(x=>x.OrderDetails)
                .ThenInclude(x=>x.Book)
                .ThenInclude(x=>x.Genre)
                .Where(a=>a.UserId==userid)
                .ToListAsync();

            return orders;

        }



        private string GetUserId()
        {
            var principal = _httpcontextAccessor.HttpContext.User;
            var userid = _userManager.GetUserId(principal);
            return userid;
        }









    }

 
}
