namespace PokemonGame.Core.Settings
{
    public class PaginationModel<T>
    {
        public List<T> data { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public int total_pages { get; set; }
        public int total_rows { get; set; }
    }
}
