namespace VendorManagementSystem.Application.Dtos.UtilityDtos
{
    public class PaginationDto
    {
        public int Cursor { get; set; }
        public int Size { get; set; }
        public bool Next { get; set; }
    }
}
