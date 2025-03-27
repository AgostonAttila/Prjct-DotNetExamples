using CleanFromScratch.Domain.Constants;
using CleanFromScratch.Domain.Entities;

namespace CleanFromScratch.Domain.Repositories
{
    public interface IRestaurantsRepository
    {
        Task<IEnumerable<Restaurant>> GetAllAsync();
        Task<(IEnumerable<Restaurant>,int)> GetAllMatchingAsync(string? searchPhrase,int pageSize,int pageNumber,string? sortBy,SortDirection sortDirection );
     
        Task<Restaurant?> GetByIdAsync(int id);
        Task<int> Create(Restaurant entity);
        Task Delete(Restaurant restaurant);
        Task SaveChanges();
    }
}
