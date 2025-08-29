using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PokemonGame.Core.Models.Response
{
    public class Pagination<T>
    {

        public List<T> Items {  get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
        public Pagination(List<T> items, int pageIndex, int totalPages)
        {
            Items = items;
            PageIndex = pageIndex;
            TotalPages = totalPages;
        }
    }
}
