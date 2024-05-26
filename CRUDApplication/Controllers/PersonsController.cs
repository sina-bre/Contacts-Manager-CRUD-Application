using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            List<PersonResponse> persons = _personsService.GetAllPersons().ToList();

            return View(persons);
        }
    }
}
