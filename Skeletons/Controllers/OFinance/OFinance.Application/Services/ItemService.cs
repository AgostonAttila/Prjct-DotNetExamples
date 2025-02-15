
using System.Security.Principal;
using OFinance.Application.DTOs;
using OFinance.Domain.Entities;
using OFinance.Domain.Repositories;

namespace OFinance.Application.Services
{


    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ItemDto> GetByIdAsync(Guid id)
        {
            var item = await _unitOfWork.Items.GetByIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException($"Item with id {id} not found");

            return MapToDto(item);
        }

        public async Task<IEnumerable<ItemDto>> GetAllAsync()
        {
            var items = await _unitOfWork.Items.GetAllAsync();
            return items.Select(MapToDto);
        }

        public async Task<ItemDto> CreateAsync(CreateItemDto dto)
        {
            var item = new Item
            {
                Title = dto.Title,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow,                
            };

            var created = await _unitOfWork.Items.AddAsync(item);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(created);
        }

        public async Task<ItemDto> UpdateAsync(Guid id, UpdateItemDto dto)
        {
            var item = await _unitOfWork.Items.GetByIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException($"Item with id {id} not found");

            item.Amount = dto.Amount;
            item.Category = dto.Category;
            item.Type = dto.Type;
            item.Description = dto.Description;
            item.UpdatedAt = DateTime.Now;          

            await _unitOfWork.Items.UpdateAsync(item);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(item);
        }

        public async Task DeleteAsync(Guid id)
        {
            var item = await _unitOfWork.Items.GetByIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException($"Item with id {id} not found");

            await _unitOfWork.Items.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        private static ItemDto MapToDto(Item item)
        {
            return new ItemDto(
                  item.Id,
                  item.Account,
                  item.Title,
                  item.Amount,
                  item.Category,
                  item.Type,
                  item.Description,
                  item.CreatedAt,
                  item.UpdatedAt
            );
        }
    }
}
