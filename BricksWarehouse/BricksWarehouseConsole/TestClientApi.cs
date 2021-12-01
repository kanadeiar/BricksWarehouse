using BricksWarehouse.Domain.Mappers;
using BricksWarehouse.Domain.Models;
using BricksWarehouse.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksWarehouseConsole
{
    public class TestClientApi
    {
        private static string __WebAPI = "https://localhost:7178";
        //private static string __WebAPI = "http://airwarehouse.azurewebsites.net";

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

        public static async Task TestOfPlaces()
        {
            Console.WriteLine("******* Места хранений товаров *********");

            PlaceClient client = new PlaceClient(new HttpClient { BaseAddress = new Uri(__WebAPI) }, new DtoMapperService(), new DtoMapperService());

            await PrintThis(client);

            var item = await client.GetById(1);
            Console.WriteLine($"\nОдин элемент: {item.Name} {item.Number} {item.Count} / {item.Size} [ <{item.ProductTypeId}> {item.ProductType?.Name} ]\n");

            Console.WriteLine("Проверка изменения элементов");
            Console.ReadKey();

            var added = await client.Add(new Place { Name = "Место", Number = 99, Count = 1, Size = 50, LastDateTime = DateTime.Today, PlaceStatus = PlaceStatus.Wait });
            Console.WriteLine($"Добавлен элемент с индексом: {added.Id} его данные: {added.Name} {added.Number} {added.Count} {added.Size} [ <{item.ProductTypeId}> {item.ProductType?.Name} ] {added.LastDateTime.ToString("dd MMMM yyyy")} - {Place.GetNamePlaceStatus(added.PlaceStatus)} ");

            var updateme = await client.GetById(added.Id);
            updateme.Name = "Обновленное имя";
            var updated = await client.Update(updateme);
            Console.WriteLine($"Обновлен элемент с индексом: {updated.Id} ");

            await PrintThis(client);

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
            if (deleted is Place prdel)
                Console.WriteLine($"Элемент удален с индексом {prdel.Id} его данные: {prdel.Name} {prdel.Number} {prdel.Count} {prdel.Size} [ <{item.ProductTypeId}> {item.ProductType?.Name} ] ");
            else
                Console.WriteLine($"Не удалось удалить элемент с индексом {added.Id}");

            static async Task PrintThis(PlaceClient client)
            {
                Console.WriteLine("Все элементы:");
                var count = 0;
                foreach (var item in (await client.GetAll()).OrderBy(i => i.Order))
                {
                    Console.WriteLine($"{item.Name} {item.Number} {item.Count} {item.Size} [ <{item.ProductTypeId}> {item.ProductType?.Name} ] {item.LastDateTime.ToString("dd MMMM yyyy")} - {Place.GetNamePlaceStatus(item.PlaceStatus)} ");
                    count++;
                }
                Console.WriteLine($"Количество: {count}");
            }
        }
    }
}
