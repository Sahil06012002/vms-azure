namespace VendorManagementSystem.Application.Dtos.UtilityDtos
{
    public class FileContentDto
    {
        public byte[]? Content { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
    }
}
