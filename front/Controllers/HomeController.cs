using Microsoft.AspNetCore.Mvc;
using front.Models;
using System.Collections.Generic;
using System.Linq;

namespace front.Controllers
{
    public class HomeController : Controller
    {
        // Наша "база даних" у пам'яті
        private static List<Pet> _pets = new List<Pet>
        {
            new Pet { Id = 1, Name = "Барсік", Age = "3 роки", Diet = "Сухий корм Profine", Allergies = "Немає", Diseases = "Гастрит", GeneralInfo = "Боїться пилососа.", Img = "https://placehold.co/400x400?text=Cat" },
            new Pet { Id = 2, Name = "Рекс", Age = "5 років", Diet = "Home Food", Allergies = "Курятина", Diseases = "Дисплазія", GeneralInfo = "Знає команди.", Img = "https://placehold.co/400x400?text=Dog" }
        };

        // 1. Пошук (зчитує текст із поля searchQuery)
        public IActionResult Index(string searchQuery)
        {
            var pets = _pets;

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                // Видаляємо пробіли з початку і кінця запиту
                var cleanQuery = searchQuery.Trim();

                // System.StringComparison.OrdinalIgnoreCase вже ігнорує великі/малі літери
                pets = _pets.Where(p => p.Name.Contains(cleanQuery, System.StringComparison.OrdinalIgnoreCase)).ToList();
            }

            ViewBag.SearchQuery = searchQuery;
            return View(pets);
        }

        // 2. Додавання нової тварини
        [HttpPost]
        public IActionResult AddPet(Pet newPet)
        {
            newPet.Id = _pets.Count > 0 ? _pets.Max(p => p.Id) + 1 : 1;
            if (string.IsNullOrEmpty(newPet.Img))
                newPet.Img = "https://placehold.co/400x400?text=New+Pet"; // Картинка за замовчуванням

            _pets.Add(newPet);
            return RedirectToAction("Index"); // Оновлюємо сторінку
        }

        // 3. Видалення тварини
        [HttpPost]
        public IActionResult DeletePet(int id)
        {
            var pet = _pets.FirstOrDefault(p => p.Id == id);
            if (pet != null) _pets.Remove(pet);

            return RedirectToAction("Index");
        }

        // Трекер залишаємо без змін
        public IActionResult Tracker() => View();

        [HttpPost]
        public IActionResult SaveTrackerData([FromBody] object trackerData) => Ok(new { success = true, message = "Збережено" });
    }
}