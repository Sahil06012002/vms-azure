using Azure;
using System.Reflection.Metadata.Ecma335;
using VendorManagementSystem.Application.Dtos.ModelDtos.Item;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos.Item;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Models.Enums;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.Services
{
    public class ItemService : IItemService
    {
        
        private readonly IItemRepository _itemRepository;
        private readonly IUtilityService _utilityService;
        private readonly ITokenService _tokenService;
        private readonly IUnitRepository _unitRepository;
        public ItemService(IItemRepository itemRepository,IUtilityService utilityService,ITokenService tokenService,IUnitRepository unitRepository)
        {
            _itemRepository = itemRepository;
            _utilityService = utilityService;
            _tokenService = tokenService;
            _unitRepository = unitRepository;
        }
        public ApplicationResponseDto<ItemFormDto> GetItemFormDetails()
        {
            try
            { 
                string[] taxPreference = Enum.GetNames(typeof (TaxPreference));
                List<int> gstRates = Enum.GetValues(typeof(GST)).Cast<int>().ToList();
                IEnumerable<Unit> units = _unitRepository.GetAllUnits();
                ItemFormDto response = new()
                {
                    TaxPreference = taxPreference,
                    GstRates = gstRates,
                    Units = units
                };
                return new ApplicationResponseDto<ItemFormDto>
                {
                    Data = response,
                };
            }
            catch (Exception ex)
            {
                return new ApplicationResponseDto<ItemFormDto>
                {
                    Error = new()
                    {
                        Message = [ex.Message]
                    }
                };
            }
        }
        public ApplicationResponseDto<IEnumerable<ItemResponseDto>> GetItemService()
        {
            ApplicationResponseDto<IEnumerable<ItemResponseDto>> serviceResponse = new();
            try
            {
                var response = _itemRepository.GetItems();
                serviceResponse.Data = response;
                return serviceResponse;
            }catch (Exception ex)
            {
                serviceResponse.Error = new() { Message = [ex.Message] };
                return serviceResponse;
            }

        }
        public ApplicationResponseDto<int> AddItem(string jwtToken, ItemDto itemDto)
        {
            try
            {
                string currentUser = _tokenService.ExtractUserDetials(jwtToken, "id");
                ItemType itemType = _utilityService.ParseEnum<ItemType>(itemDto.ItemType);
                TaxPreference taxPreference = _utilityService.ParseEnum<TaxPreference>(itemDto.TaxPreference);

                Item item = new()
                {
                    Name = itemDto.Name,
                    UnitId = itemDto.UnitId,
                    ItemType = itemType,
                    Code = itemDto.Code,
                    TaxPreference = taxPreference,
                    SellingPrice = itemDto.SellingPrice,
                    SalesAccount = itemDto.SalesAccount,
                    SalesDescription = itemDto.SalesDescription,
                    CostPrice = itemDto.CostPrice,
                    PurchaseAccount = itemDto.PurchaseAccount,
                    PurchaseDescription = itemDto.PurchaseDescription,
                    GstRate = itemDto.GstRate,
                    IGstRate = itemDto.GstRate,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = int.Parse(currentUser),
                    UpdatedBy = int.Parse(currentUser)
                };

                var response = _itemRepository.AddItem(item);
                return new ApplicationResponseDto<int>
                {
                    Data = response,
                };
            }catch(Exception ex)
            {
                return new ApplicationResponseDto<int>
                {
                    Error = new()
                    {
                        Message = [ex.Message]
                    }
                };
            }
        }
    }
}
