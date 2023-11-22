namespace FFS.Application.DTOs.Others
{
    public class EntityFilter<T>
    {
        public List<T> List { get; set; }
        public int Total { get; set; }
        public int PageIndex { get; set; }
    }
}
