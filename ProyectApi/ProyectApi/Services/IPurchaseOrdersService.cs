using ApiRestBilling.Models;

namespace ProyectApi.Services
{
    public interface IPurchaseOrdersService
    {
        Task<decimal> CheckUnitPrice(OrderItem detalle);
        Task<decimal> CalculateSubtotalOrderItem(OrderItem item);
        decimal CalculateTotalOrderItems(List<OrderItem> item);
    }
}
