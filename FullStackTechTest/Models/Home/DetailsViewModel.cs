using DAL;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;

namespace FullStackTechTest.Models.Home;

public class DetailsViewModel
{
    public Person Person { get; set; }
    public Address Address { get; set; }
    public List<Speciality> Specialities { get; set; }
    public List<PersonSpeciality> PersonSpecialities { get; set; }
    public MultiSelectList CurrentSpecialities { get; set; }
    public string PersonSpecialitiesSimplified { get; set; }
    public List<int> SelectedSpecialities { get; set; }
    public bool IsEditing { get; set; }

    public static async Task<DetailsViewModel> CreateAsync(int personId, bool isEditing, IPersonRepository personRepository, IAddressRepository addressRepository, ISpecialityRepository specialityRepository, IPersonSpecialitiesRepository personSpecialitiesRepository)
    {
        var model = new DetailsViewModel
        {
            Person = await personRepository.GetByIdAsync(personId),
            Address = await addressRepository.GetForPersonIdAsync(personId),
            Specialities = await specialityRepository.ListAllAsync(),
            PersonSpecialities = await personSpecialitiesRepository.GetByIdAsync(personId),
            IsEditing = isEditing
        };

        model.CurrentSpecialities = new MultiSelectList(model.Specialities, "Id", "Name", model.PersonSpecialities.Select(x => x.SpecialityId));

        if (model.PersonSpecialities.Count() > 0)
        {
            model.PersonSpecialitiesSimplified = string.Join(", ", model.PersonSpecialities.Select(x => x.SpecialityName).ToArray());
        }
        else
        {
            model.PersonSpecialitiesSimplified = "No specialitiy selected.";
        }
        

        return model;
    }
}