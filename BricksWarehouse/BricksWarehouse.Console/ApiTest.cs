using BricksWarehouse.Domain.Mappers;
using BricksWarehouse.Domain.Models;
using BricksWarehouse.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksWarehouseConsole;

public class ApiTest
{
    private static string __WebAPI = "https://localhost:7178";
    //private static string __WebAPI = "https://brickswarehouse.azurewebsites.net";

    public static async void TestMobileApi()
    {
        Console.WriteLine("******* Задания *********");

        TaskClient client = new TaskClient(new HttpClient { BaseAddress = new Uri(__WebAPI) }, new DtoMapperService(), new DtoMapperService());

        Console.WriteLine("Все элементы:");
        var count = 0;
        foreach (var item in (await client.GetAll()))
        {
            Console.WriteLine($"{item.Name} {item.Number} рег.номер: {item.TruckNumber} {item.Loaded}/{item.Count}");
            count++;
        }
        Console.WriteLine($"Количество: {count}");

        var it = await client.GetById(1);
        Console.WriteLine($"\nОдин элемент: {it.Name} Формат: {it.Number} рег.номер: {it.TruckNumber} {it.Loaded}/{it.Count}\n");

        Console.WriteLine("Тестирование автоматизации");
                
        int numberTask = 0;
        while (true)
        {
            Console.WriteLine("Введите сканированный QR код задания :>");
            var code = Console.ReadLine();
            var datas = code.Split("|");
            if (datas[0] == "СНПЗаданиеО")
            {
                if (int.TryParse(datas[1], out int result))
                {
                    numberTask = result;
                    break;
                }
                Console.WriteLine("Не удалось распознать номер задания");
                continue;
            }
            else if (datas[0] == "СНПВид")
                Console.WriteLine("Вы отскарировали упаковку с товаром, а нужно выбрать или отсканировать код задания");
            else if (datas[0] == "СНПМесто")
                Console.WriteLine("Вы отсканировали место хранения товаров, а нужно выбрать или отсканировать код задания");
            else
                Console.WriteLine("Код не распознан");
        }

        if (numberTask == 1)
        {
            Console.WriteLine("*** Выполнение задания \"Загрузка товара на склад\" ***");

            Console.WriteLine("Введите сканированный QR код на упаковке товара :>");
            var code = Console.ReadLine();
            var datas = code.Split("|");
            if (datas[0] == "СНПВид")
            {
                if (int.TryParse(datas[1], out int result))
                {
                    var format = result;

                }
                Console.WriteLine("Не удалось распознать код на упаковке товара");
            }
            else if (datas[0] == "СНПЗаданиеО")
                Console.WriteLine("Вы отсканировали задание, но нужно сканировать или выбрать упаковку с товаром, который загружается на склад");
            else if (datas[0] == "СНПМесто")
                Console.WriteLine("Вы отсканировали место хранения товаров, а нужно выбрать или отсканировать упаковку с товаром, который загружается на склад");
        }
        else
        {

        }





    }


    public static async Task TestOfTypeProducts()
    {
        Console.WriteLine("******* Виды товаров *********");

        ProductTypeClient client = new ProductTypeClient(new HttpClient { BaseAddress = new Uri(__WebAPI) }, new DtoMapperService(), new DtoMapperService());

        Console.WriteLine("Все элементы:");
        var count = 0;
        foreach (var item in (await client.GetAll()).OrderBy(i => i.Order))
        {
            Console.WriteLine($"{item.Name} Формат: {item.FormatNumber} {item.Units} {item.Volume} [ {item.Places?.Count} ], ");
            count++;
        }
        Console.WriteLine($"Количество: {count}");

        var it = await client.GetById(1);
        Console.WriteLine($"\nОдин элемент: {it.Name} Формат: {it.FormatNumber} {it.Units} {it.Volume} [ {it.Places?.Count} ],\n");

        Console.WriteLine("Проверка изменения элементов");
        Console.ReadKey();

        var added = await client.Add(new ProductType { Name = "Новый вид", FormatNumber = 999, Units = 11, Order = 99, Volume = 12.56, Weight = 800.19 });
        Console.WriteLine($"Добавлен элемент с индексом: {added.Id} его данные: {added.Name} Формат: {added.FormatNumber} {added.Units} {added.Volume} [ {added.Places?.Count} ] ");

        var updateme = await client.GetById(added.Id);
        updateme.Name = "Обновленное имя";
        var updated = await client.Update(updateme);
        Console.WriteLine($"Обновлен элемент с индексом: {updated.Id} его данные: {updated.Name} Формат: {updated.FormatNumber} {updated.Units} {updated.Volume} [ {updated.Places?.Count} ] ");

        Console.WriteLine("Все элементы:");
        count = 0;
        foreach (var item in (await client.GetAll()).OrderBy(i => i.Order))
        {
            Console.WriteLine($"{item.Name} Формат: {item.FormatNumber} {item.Units} {item.Volume} [ {item.Places?.Count} ], ");
            count++;
        }
        Console.WriteLine($"Количество: {count}");

        Console.WriteLine("Проверка мусорной корзины");
        Console.ReadKey();

        if (await client.ToTrash(added.Id))
            Console.WriteLine("Успешное помещение в корзину");
        else
            Console.WriteLine("Не удалось поместить в корзину");

        Console.WriteLine("Проверка мусорной корзины");
        Console.ReadKey();

        if (await client.FromTrash(added.Id))
            Console.WriteLine("Успешное возвращение из корзины");
        else
            Console.WriteLine("Не удалось возвратить из корзины");

        Console.WriteLine("Проверка удаления элемента");
        Console.ReadKey();

        var deleted = await client.Delete(added.Id);
        if (deleted is ProductType prdel)
            Console.WriteLine($"Элемент удален с индексом {prdel.Id} его данные: {prdel.Name} Формат: {prdel.FormatNumber} {prdel.Units} {prdel.Volume} [ {prdel.Places?.Count} ] ");
        else
            Console.WriteLine($"Не удалось удалить элемент с индексом {added.Id}");
    }
}

