using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaTimeDemo.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        //為了精簡程式碼把save功能抽出來
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IStoreRepository Store { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IOrderHeaderRepository OrderHeader { get; } 
        IOrderDetailRepository OrderDetail { get; }
        void Save();
    }
}
