namespace VendorManagementSystem.Application.Dtos.UtilityDtos
{
    public class PagenationResponseDto
    {
        public IEnumerable<object>? PagenationData { get; set; }
        public int Cursor { get; set; }

        public int PreviousCursor { get; set; } 

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }
    }
}
