using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IImportDataRepository
    {
        Task<int> Insert(List<PersonWithAddress> persons);
    }
}
