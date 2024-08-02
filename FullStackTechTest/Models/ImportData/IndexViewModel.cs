using DAL;
using Models;

namespace FullStackTechTest.Models.ImportData
{
    public class IndexViewModel
    {
        public IFormFile UploadFile { get; set; }

        public int InsertPeopleCount { get; set; }

        public static async Task<IndexViewModel> InsertAsync(IImportDataRepository importDataRepository, List<PersonWithAddress> persons)
        {
            var model = new IndexViewModel
            {
                InsertPeopleCount = await importDataRepository.Insert(persons)
            };
            return model;

        }
    }
}
