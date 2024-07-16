using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos.PurchaseOrder;

namespace VendorManagementSystem.Application.IServices
{
    public interface IEmailService
    {
        void SendLoginEmail(EmailDetailsDto emailDetailsDto);
        public void SendAckEmail(AckEmailDto ackEmailDto);
    }
}
