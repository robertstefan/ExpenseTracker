namespace ExpenseTracker.Core.Common.Pagination
{
    public static class PagedMethods
    {
        public static class OffsetMethods
        {
            public static int GetOffsetByPageSize(int pageNumber, int pageSize)
            {
                if (pageNumber <= 0)
                {
                    pageNumber = 1;
                }
                return (pageNumber - 1) * pageSize;
            }
        }
    }
}