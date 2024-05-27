using Microsoft.AspNetCore.Mvc;
using ServiceContracts.DTO.Enums;
using ServiceContracts.DTO.PersonDTO;
using ServiceContracts.Interfaces;
namespace DIExample.Controllers
{
    public class PersonsController : Controller
    {
        private readonly IPersonsService _personsService;

        public PersonsController(IPersonsService personsService)
        {
            _personsService = personsService;
        }
        [Route("persons/index")]
        [Route("/")]
        public IActionResult Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            ViewBag.SearchFromActionUrl = "persons/index";

            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                {nameof(PersonResponse.PersonName), "Person Name"},
                {nameof(PersonResponse.Email), "Email"},
                {nameof(PersonResponse.DateOfBirth), "Birth"},
                {nameof(PersonResponse.Gender), "Gender"},
                {nameof(PersonResponse.CountryID), "Country"},
                {nameof(PersonResponse.Address), "Address"},
                {nameof(PersonResponse.Age), "Age"},
            };

            List<PersonResponse> persons = _personsService.GetFilteredPersons(searchBy, searchString);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            //Sort
            List<PersonResponse> sortedPersons = _personsService.GetSortedPersons(persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder;


            return View(sortedPersons);
        }

        [Route("persons/create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
    }
}
