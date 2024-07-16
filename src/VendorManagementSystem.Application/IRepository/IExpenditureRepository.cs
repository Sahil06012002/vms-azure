using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IRepository
{
    public interface IExpenditureRepository
    {
        public bool AddExpenditures(List<Expenditure> expenditure);

        public IEnumerable<Expenditure> GetAllExpenditure(int id);

        public IEnumerable<Expenditure> GetTopExpenditure(int count);

    }
}
