using ExpenseTracker.API.Common.Interfaces;

namespace ExpenseTracker.API.Common.Pagination;

public class Paged<T> where T : IEntityDTO
{
    public IEnumerable<T> Items { get; set; }
    public int TotalItems { get; set; }
    public int PageNumber { get; private set; } = 1;
    public int PageSize { get; private set; } = 10;
    public int TotalPages { get; private set; }
    public int StartPage { get; private set; }
    public int EndPage { get; private set; }
    public IEnumerable<int> PageNumbers { get; private set; }

    public Paged(int totalCount, int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber <= 0)
        {
            pageNumber = 1;
        }
        if (pageSize > 100)
        {
            pageSize = 100;
        }

        const int MAX_NAV_PAGES = 3; //must be a odd number

        var totalPages = (int)Math.Ceiling(totalCount / (decimal)pageSize);

        int startPage;  //what page should page numbers start from
        int endPage;    //what page should page numbers end with

        if (totalPages <= MAX_NAV_PAGES) // Show all available pages
        {
            startPage = 1;
            endPage = totalPages;
        }
        else
        {
            var maxPagesBeforeActualPage = (int)Math.Floor(MAX_NAV_PAGES / (decimal)2);       //how many pages we show before the current page
            var maxPagesAfterActualPage = (int)Math.Ceiling(MAX_NAV_PAGES / (decimal)2) - 1;  //how many pages we show after the current page

            if (pageNumber <= maxPagesBeforeActualPage)
            /// <summary>
            /// Handles pagination logic when the current page is at the start. 
            /// This means it shows pages starting from 1 to the maximum number of navigation pages (`MAX_NAV_PAGES`).
            /// </summary>
            /// <example>
            /// For instance, if `pageNumber = 1` and `maxPagesBeforeActualPage = 1`, the following logic applies:
            /// <code>
            /// if (1 <= 1) {
            ///     startPage = 1;
            ///     endPage = MAX_NAV_PAGES;
            /// }
            /// </code>
            /// The sequence would be: 1, 2, 3.
            /// </example>
            {
                startPage = 1;
                endPage = MAX_NAV_PAGES;
            }

            else if (pageNumber + maxPagesAfterActualPage >= totalPages)
            /// <summary>
            /// Handles pagination logic when the current page is near the end of the sequence, 
            /// either in the middle of the end sequence or at the end. 
            /// This logic shows the last `MAX_NAV_PAGES`.
            /// </summary>
            /// <example>
            /// Example 1: 
            /// <code>
            /// TotalPages = 6;
            /// pageNumber = 5;
            /// maxPagesAfterActualPage = 1;
            /// if (5 + 1 >= 6) {
            ///     startPage = 6 - 3 + 1; Result: startPage = 4
            ///     endPage = 6;
            /// }
            /// The sequence would be 4, 5, 6.
            /// </code>
            /// 
            /// Example 2: 
            /// <code>
            /// TotalPages = 6;
            /// pageNumber = 6;
            /// maxPagesAfterActualPage = 1;
            /// if (6 + 1 >= 6) {
            ///     startPage = 6 - 3 + 1; Result: startPage = 4
            ///     endPage = 6;
            /// }
            /// The sequence would be 4, 5, 6.
            /// </code>
            /// </example>
            {
                startPage = totalPages - MAX_NAV_PAGES + 1;
                endPage = totalPages;
            }

            else
            /// <summary>
            /// Handles pagination logic when the current page is in the middle of the sequence. 
            /// The start page is calculated as the current page minus the number of pages to show before it, 
            /// and the end page is the current page plus the number of pages to show after it.
            /// </summary>
            /// <example>
            /// Example:
            /// <code>
            /// TotalPages = 6;
            /// pageNumber = 4;
            /// maxPagesBefore = 1;
            /// maxPagesAfter = 1;
            ///
            /// Since pageNumber >= maxPagesBeforeActualPage and pageNumber + maxPagesAfter < TotalPages, 
            /// the logic follows the latest else condition:
            /// startPage = 4 - 1; Result: startPage = 3
            /// endPage = 4 + 1;   Result: endPage = 5
            /// 
            /// The sequence would be 3, 4, 5.
            /// </code>
            /// </example>

            {
                startPage = pageNumber - maxPagesBeforeActualPage;
                endPage = pageNumber + maxPagesAfterActualPage;
            }
        }
        var pageNumbers = Enumerable.Range(startPage, (endPage + 1) - startPage);

        StartPage = startPage;
        EndPage = endPage;
        PageNumber = pageNumber;
        PageNumbers = pageNumbers;
        PageSize = pageSize;
        TotalItems = totalCount;
        TotalPages = totalPages;
    }
}
