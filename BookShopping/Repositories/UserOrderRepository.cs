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

        public async Task ChangeOrderStatus(UpdateStatusModel data)
        {

            var order = await _context.Orders.FindAsync(data.OrderId);
            if(order == null)
            {
                throw new InvalidOperationException($"order with id:{data.OrderId} does not found");
            }
            order.OrderStatusId = data.OrderStatusId;
            await _context.SaveChangesAsync();


        }

        public async Task<Order?> GetOrderById(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<OrderStatus>> GetOrderStatuses()
        {
           return await  _context.OrderStatus.ToListAsync();
        }

        public async Task TogglePaymentStatus(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if(order == null) 
            {
                throw new InvalidOperationException($"order with id: {orderId} does not found");
            
            }

            order.IsPaid = !order.IsPaid;
            await _context.SaveChangesAsync();  

        }

        public async Task<IEnumerable<Order>> UserOrders(bool getAll = false)
        {
            var orders = _context.Orders
                                        .Include(x => x.OrderStatus)
                                        .Include(x => x.OrderDetail)
                                        .ThenInclude(x => x.Book)
                                         .ThenInclude(x => x.Genre)
                                         .AsQueryable();

            if (!getAll)
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged-in");
                orders = orders.Where(a=>a.UserId== userId);
                return await orders.ToListAsync();
            }


            return await orders.ToListAsync();

        }

        private string GetUserId()
        {
            var principal = _httpcontextAccessor.HttpContext.User;
            var userid = _userManager.GetUserId(principal);
            return userid;
        }









    }

 
}
