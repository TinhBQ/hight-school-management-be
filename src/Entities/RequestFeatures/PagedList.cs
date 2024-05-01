namespace Entities.RequestFeatures
{
    public class PagedList<T> : List<T>
    {
        public MetaData metaData { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            metaData = new MetaData
            {
                totalCount = count,
                pageSize = pageSize,
                currentPage = pageNumber,
                totalPages = (int)Math.Ceiling(count / (double)pageSize)
            };
            AddRange(items);
        }

        public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
