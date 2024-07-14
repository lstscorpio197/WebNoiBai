using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNoiBai.WHttpMessage
{
    public class HttpMessagePagination
    {
        public readonly int Maximum_NumberRowsOnPage = 500;

        public int StartIndex => (PageNumber - 1) * NumberRowsOnPage + 1;

        public int EndIndex => StartIndex - 1 + TotalRowsOnPage;

        public int PageNumber { get; set; }

        public int TotalRowsOnPage { get; set; }

        public int TotalRows { get; set; }

        public int NumberRowsOnPage { get; set; }

        public int TotalPage
        {
            get
            {
                if (TotalRows <= NumberRowsOnPage)
                {
                    return 1;
                }
                if (TotalRows % NumberRowsOnPage == 0)
                {
                    return TotalRows / NumberRowsOnPage;
                }
                return TotalRows / NumberRowsOnPage + 1;
            }
        }

        public HttpMessagePagination(int numberRowOnPage)
        {
            if (numberRowOnPage > Maximum_NumberRowsOnPage)
            {
                numberRowOnPage = Maximum_NumberRowsOnPage;
            }
            NumberRowsOnPage = numberRowOnPage;
        }

        public HttpMessagePagination()
        {
        }
    }
}