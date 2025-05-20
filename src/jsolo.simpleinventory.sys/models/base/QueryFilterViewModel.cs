namespace jsolo.simpleinventory.sys.models;



public class QueryFilterViewModel
{
    public int PageSize { get; set; } = -1;

    public int PageIndex { get; set; } = -1;

    public string? SortBy { get; set; }

    public string? OrderBy { get; set; }
}



public class QueryFilterResultsViewModel<T> where T : class
{
    public int Total { get; set; }

    public IList<T> Items { get; set; } = [];
}
