using Application.Common.Dependencies.DataAccess.Repositories.Common;

namespace Infrastructure.ApplicationDependencies.DataAccess.Repositories.Common
{
    public class ListResponseModel<TEntity> : IListResponseModel<TEntity>
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }

        public int PageCount { get; private set; }
        public int RowCount { get; private set; }

        public string ActiveFilter { get; private set; }
        public string ActiveOrderBy { get; private set; }

        public int FirstRowOnPage => RowCount <= 0 ? 0 : ((PageIndex - 1) * PageSize) + 1;
        public int LastRowOnPage => Math.Min(PageIndex * PageSize, RowCount);

        public IEnumerable<TEntity> Results { get; set; } = new List<TEntity>();

        public ListResponseModel(ListQueryModel<TEntity> queryModel, int rowCount, IEnumerable<TEntity> results)
        {
            Results = results;

            PageIndex = queryModel.PageIndex;
            PageSize = queryModel.PageSize;
            ActiveOrderBy = queryModel.OrderBy;
            ActiveFilter = queryModel.Filter;
            RowCount = rowCount;
            PageCount = (int)Math.Ceiling((double)rowCount / PageSize);
        }
    }
}