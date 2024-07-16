namespace VendorManagementSystem.Application.Dtos.UtilityDtos
{
    public class ApplicationResponseDto<T>
    {
        public Error? Error { get; set; }
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
