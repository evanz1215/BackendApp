using Application.Common.Mapping;
using Domain.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Application.Common.Dependencies.DataAccess.Repositories.Common
{
    public interface IRepositoryBase<TEntity, TId> where TEntity : class, IEntity<TId>
    {
        Task<TEntity> GetByIdAsync(TId id);

        EntityEntry<TEntity> Create(TEntity entity);

        Task<EntityEntry<TEntity>> CreateAsync(TEntity entity);

        void CreateRange(IEnumerable<TEntity> entities);

        Task CreateRangeAsync(IEnumerable<TEntity> entities);

        EntityEntry<TEntity> Update(TEntity entity);

        EntityEntry<TEntity> Delete(TEntity entity);

        void DeleteRange(IEnumerable<TEntity> entities);

        //Task<PaginatedResult<T>> GetProjectedListAsync(int pageNumber, int pageSize, Expression<Func<T, bool>> additionalFilter, bool readOnly = false);
        Task<IListResponseModel<TEntity>> GetProjectedListAsync(ListQueryModel<TEntity> model, Expression<Func<TEntity, bool>> additionalFilter = null, bool readOnly = false);

        Task<IListResponseModel<TDto>> GetProjectedListDtoAsync<TDto>(ListQueryModel<TDto> model, Expression<Func<TEntity, bool>> additionalFilter = null, bool readOnly = false) where TDto : IMapFrom<TEntity>;
    }
}