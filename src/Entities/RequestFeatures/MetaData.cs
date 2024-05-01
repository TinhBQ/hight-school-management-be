namespace Entities.RequestFeatures
{
    public class MetaData
    {
        public int currentPage { get; set; }
        public int totalPages { get; set; }
        public int pageSize { get; set; }
        public int totalCount { get; set; }

        public bool hasPrevious => currentPage > 1;
        public bool hasNext => currentPage < totalPages;
    }
}
