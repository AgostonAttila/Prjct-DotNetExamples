using OFinance.Application.DTOs;

namespace OFinance.Application.Services
{ 
    public interface IItemService
    {
        Task<ItemDto> GetByIdAsync(Guid id);
        Task<IEnumerable<ItemDto>> GetAllAsync();
        Task<ItemDto> CreateAsync(CreateItemDto dto);
        Task<ItemDto> UpdateAsync(Guid id, UpdateItemDto dto);
        Task DeleteAsync(Guid id);
    }
}
