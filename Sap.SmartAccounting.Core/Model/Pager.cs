namespace Sap.SmartAccounting.Core
{
    public class Pager : IPager
    {
        public Pager()
        {
            PagingSize = 10;
            CurrentPage = 0;
        }

        public Pager(int index)
        {
            PagingSize = 10;
            CurrentPage = index;
        }

        public short PagingSize { get; set; }
        public int CurrentPage { get; set; }

        public int MaxPage { get; private set; }
        public int TotalCount { get; private set; }

        public void GetPageSize()
        {
            if (PagingSize <= 0)
            {
                PagingSize = 10;
            }
        }

        public void SetTotalCount(int value)
        {
            TotalCount = value;

            GetPageSize();

            MaxPage = TotalCount/PagingSize;

            if (CurrentPage > MaxPage)
            {
                CurrentPage = MaxPage;
            }
        }
    }

    public interface IPager
    {
        short PagingSize { get; set; }
        int CurrentPage { get; set; }

        int MaxPage { get; }
        int TotalCount { get; }

        void GetPageSize();
        void SetTotalCount(int value);
    }
}