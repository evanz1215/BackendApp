using Application.Common.Dependencies.DataAccess.Repositories.Common;
using Application.Common.Mapping;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Common;
using Infrastructure.Common.Extensions;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Infrastructure.ApplicationDependencies.DataAccess.Repositories.Common
{
    public abstract class RepositoryBase<TEntity, TId> : IRepositoryBase<TEntity, TId> where TEntity : class, IEntity<TId>
    {
        protected ApplicationDbContext _dbContext;
        protected DbSet<TEntity> _set;
        private readonly IMapper _mapper;

        protected abstract IQueryable<TEntity> BaseQuery { get; }

        public RepositoryBase(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _set = dbContext.Set<TEntity>();
            _mapper = mapper;
        }

        public async Task<TEntity> GetByIdAsync(TId id)
        {
            return await BaseQuery.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public EntityEntry<TEntity> Create(TEntity entity)
        {
            return _set.Add(entity);
        }

        public async Task<EntityEntry<TEntity>> CreateAsync(TEntity entity)
        {
            return await _set.AddAsync(entity);
        }

        public void CreateRange(IEnumerable<TEntity> entities)
        {
            _set.AddRange(entities);
        }

        public async Task CreateRangeAsync(IEnumerable<TEntity> entities)
        {
            await _set.AddRangeAsync(entities);
        }

        public EntityEntry<TEntity> Update(TEntity entity)
        {
            return _set.Update(entity);
        }

        public EntityEntry<TEntity> Delete(TEntity entity)
        {
            return _set.Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _set.RemoveRange(entities);
        }

        public async Task<IListResponseModel<TEntity>> GetProjectedListAsync(ListQueryModel<TEntity> model, Expression<Func<TEntity, bool>> additionalFilter = null, bool readOnly = false)
        {
            var query = readOnly ? BaseQuery.AsNoTracking() : BaseQuery;

            if (additionalFilter != null)
            {
                query = query.Where(additionalFilter);
            }

            IQueryable<TEntity> filteredDtoQuery = default;

            try
            {
                filteredDtoQuery = query
                    //.ProjectTo<TDto>(_mapper.ConfigurationProvider)
                    .ApplyFilter(model.Filter);
            }
            catch (FormatException fe)
            {
                model.ThrowFilterIncorrectException(fe.InnerException);
            }

            var totalRowCount = await filteredDtoQuery.CountAsync();

            IEnumerable<TEntity> resultPage = default;

            try
            {
                resultPage = await filteredDtoQuery
                    .ApplyOrder(model.OrderBy)
                    .ApplyPaging(model.PageSize, model.PageIndex)
                    .ToListAsync();
            }
            catch (FormatException fe)
            {
                model.ThrowOrderByIncorrectException(fe.InnerException);
            }

            return new ListResponseModel<TEntity>(model, totalRowCount, resultPage);
        }

        public virtual async Task<IListResponseModel<TDto>> GetProjectedListDtoAsync<TDto>(ListQueryModel<TDto> model, Expression<Func<TEntity, bool>> additionalFilter = null, bool readOnly = false) where TDto : IMapFrom<TEntity>
        {
            var query = readOnly ? BaseQuery.AsNoTracking() : BaseQuery;

            if (additionalFilter != null)
            {
                query = query.Where(additionalFilter);
            }

            IQueryable<TDto> filteredDtoQuery = default;
            try
            {
                filteredDtoQuery = query
                    .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                    .ApplyFilter(model.Filter);
            }
            catch (FormatException fe)
            {
                model.ThrowFilterIncorrectException(fe.InnerException);
            }

            var totalRowCount = await filteredDtoQuery.CountAsync();

            IEnumerable<TDto> resultPage = default;
            try
            {
                resultPage = await filteredDtoQuery
                    .ApplyOrder(model.OrderBy)
                    .ApplyPaging(model.PageSize, model.PageIndex)
                    .ToListAsync();
            }
            catch (FormatException fe)
            {
                model.ThrowOrderByIncorrectException(fe.InnerException);
            }

            return new ListResponseModel<TDto>(model, totalRowCount, resultPage);
        }
    }
}