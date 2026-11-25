using Microsoft.AspNetCore.Mvc;
using PetTracker.Models;
using PetTracker.Services;

namespace PetTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PetsController : ControllerBase
{
    private readonly JsonDataStore _store;

    public PetsController(JsonDataStore store) => _store = store;

    // GET /api/pets
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pet>>> GetPets()
    {
        var pets = await _store.ReadAsync(d => d.Pets.OrderBy(p => p.Name).ToList());
        return Ok(pets);
    }
}