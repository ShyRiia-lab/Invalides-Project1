using System.Text.Encodings.Web;
using System.Text.Json;
using PetTracker.Models;

namespace PetTracker.Services;

public class DataFile
{
    public List<Pet> Pets { get; set; } = new();
    public int NextPetId { get; set; } = 1;
}

public class JsonDataStore
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping // кирилиця читабельна
    };

    private readonly string _path;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly DataFile _data = new();

    public JsonDataStore(IWebHostEnvironment env)
    {
        var dir = Path.Combine(env.ContentRootPath, "Data");
        Directory.CreateDirectory(dir);
        _path = Path.Combine(dir, "data.json");

        if (File.Exists(_path))
        {
            _data = JsonSerializer.Deserialize<DataFile>(File.ReadAllText(_path), Options) ?? new();
        }
        else
        {
            _data.Pets.AddRange(new[]
            {
                new Pet { Id = 1, Name = "Барсик", PhotoUrl = "/images/barsik.jpg", Age = 3,
                          Diet = "Сухий корм 2 рази на день", Allergies = "Курка",
                          GeneralInfo = "Спокійний кіт, любить спати на підвіконні." },
                new Pet { Id = 2, Name = "Рекс", PhotoUrl = "/images/rex.jpg", Age = 5,
                          Diet = "Натуральна їжа: м'ясо, крупи", Diseases = "Артрит",
                          GeneralInfo = "Німецька вівчарка, потребує довгих прогулянок." },
                new Pet { Id = 3, Name = "Луна", PhotoUrl = "/images/luna.jpg", Age = 1,
                          Diet = "Корм для цуценят, 3 рази на день", Allergies = "Зернові",
                          GeneralInfo = "Активна, дуже грайлива." }
            });
            _data.NextPetId = 4;
            File.WriteAllText(_path, JsonSerializer.Serialize(_data, Options));
        }
    }

    public async Task<T> ReadAsync<T>(Func<DataFile, T> read)
    {
        await _lock.WaitAsync();
        try { return read(_data); }
        finally { _lock.Release(); }
    }

    public async Task<T> WriteAsync<T>(Func<DataFile, T> change)
    {
        await _lock.WaitAsync();
        try
        {
            var result = change(_data);
            await File.WriteAllTextAsync(_path, JsonSerializer.Serialize(_data, Options));
            return result;
        }
        finally { _lock.Release(); }
    }
}