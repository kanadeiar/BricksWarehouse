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

    public static async Task TestMobileApi()
    {
        Console.WriteLine("******* Задания *********");

        TaskClient client = new TaskClient(new HttpClient { BaseAddress = new Uri(__WebAPI) }, new DtoMapperService(), new DtoMapperService(), new DtoMapperService(), new DtoMapperService());

        //Console.WriteLine("Все элементы:");
        //var count = 0;
        //foreach (var item in (await client.GetAll()))
        //{
        //    Console.WriteLine($"{item.Name} {item.Number} рег.номер: {item.TruckNumber} {item.Loaded}/{item.Count}");
        //    count++;
        //}
        //Console.WriteLine($"Количество: {count}");

        //var it = await client.GetById(1);
        //Console.WriteLine($"\nОдин элемент: {it.Name} Формат: {it.Number} рег.номер: {it.TruckNumber} {it.Loaded}/{it.Count}\n");

        Console.WriteLine("Тестирование автоматизации");
                
        int numberTask = 99;
        var task = (await client.GetAll()).FirstOrDefault(t => t.Number == numberTask);
        //while (true)
        //{
        //    Console.WriteLine("Введите сканированный QR код задания :>");
        //    var code = Console.ReadLine();
        //    var datas = code.Split("|");
        //    if (datas[0] == "SNPTaskO")
        //    {
        //        if (int.TryParse(datas[1], out int result))
        //        {
        //            numberTask = result;
        //            break;
        //        }
        //        Console.WriteLine("Не удалось распознать номер задания");
        //        continue;
        //    }
        //    else if (datas[0] == "SNPUnit")
        //        Console.WriteLine("Вы отскарировали упаковку с товаром, а нужно выбрать или отсканировать код задания");
        //    else if (datas[0] == "SNPPlace")
        //        Console.WriteLine("Вы отсканировали место хранения товаров, а нужно выбрать или отсканировать код задания");
        //    else
        //        Console.WriteLine("Код не распознан");
        //}
        if (numberTask == 1)
        {
            ProductType productType;
            Console.WriteLine("*** Выполнение задания \"Загрузка товара на склад\" ***");
            while (true)
            {
                Console.WriteLine("Введите сканированный QR код на упаковке товара :>");
                var code = Console.ReadLine();
                var datas = code.Split("|");
                if (datas[0] == "SNPUnit")
                {
                    if (int.TryParse(datas[1], out int result))
                    {
                        if (await client.GetProductTypeByFormat(result) is { } pt)
                        {
                            productType = pt;
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"Такой код товара в базе данных отсутствует: {result}");
                        }
                    }
                    Console.WriteLine("Не удалось распознать код на упаковке товара");
                }
                else if (datas[0] == "SNPTaskO")
                    Console.WriteLine("Вы отсканировали задание, но нужно сканировать или выбрать упаковку с товаром, который загружается на склад");
                else if (datas[0] == "SNPPlace")
                    Console.WriteLine("Вы отсканировали место хранения товаров, а нужно выбрать или отсканировать упаковку с товаром, который загружается на склад");
            }
            Console.WriteLine($"Выполнение задания по перевозке товара: [{productType.FormatNumber}] {productType.Name}");
            Console.WriteLine("Нажмите кнопку после загрузки товара на транспортер");
            Console.ReadKey();

            Console.WriteLine("Возможные места хранения для товара:");

            var places = await client.GetRecommendedLoadPlaces(productType.Id);
            foreach (var item in places)
            {
                Console.WriteLine($"{item.Name} {item.Number} [{item.Count}/{item.Size}] вид товаров: {item.ProductType?.Name}");
            }

            Place place;
            while (true)
            {
                Console.WriteLine("Введите сканированный QR код на месте хранени ятоваров :>");
                var code = Console.ReadLine();
                var datas = code.Split("|");
                if (datas[0] == "SNPPlace")
                {
                    if (int.TryParse(datas[1], out int result))
                    {
                        if (await client.GetPlaceByNumber(result) is { } p)
                        {
                            place = p;
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"Такой код места хранения товаров отсутствует: {result}");
                        }
                    }
                    Console.WriteLine("Не удалось распознать код места хранения товаров");
                }
                else if (datas[0] == "SNPTaskO")
                    Console.WriteLine("Вы отсканировали задание, но нужно сканировать или выбрать место хранения товаров");
                else if (datas[0] == "SNPUnit")
                    Console.WriteLine("Вы отсканировали упаковку с товаром, а нужно сканировать место хранения товаров");
            }
            Console.WriteLine($"Выполнение задания по перевозке товара [{productType.FormatNumber}] {productType.Name} на место хранения товара [{place.Number}] {place.Name} [{place.Count}/{place.Size}]");
            Console.WriteLine("Нажмите кнопку после выгрузки товара с транспортера на место хранения товара");
            Console.ReadKey();
            if (await client.LoadProductToPlace(productType.Id, place.Id, 1) is { } newp)
            {
                Console.WriteLine($"Успешно изменено количестово товара на месте хранения товаров [{place.Number}] {place.Name} [{place.Count}/{place.Size}]");
            }
            else
            {
                Console.WriteLine("Не удалось добавить число товаров к месту хранения товаров");
            }
        }
        else
        {            
            Console.WriteLine("*** Выполнение задания \"Выгрузка товара со склада\" ***");
            Console.WriteLine($"Задание: [{task.Number}] {task.Name} рег.номер: {task.TruckNumber} {task.Loaded}/{task.Count}");
            
            Console.WriteLine("Возможные места с таким товаром:");
            var productTypeId = task.ProductTypeId ?? 0;
            foreach (var item in (await client.GetRecommendedShipmentPlaces(productTypeId)))
            {
                Console.WriteLine($"{item.Name} {item.Number} {item.ProductType?.Name} {item.Count}/{item.Size}");
            }
            Place place;
            while (true)
            {
                Console.WriteLine("Введите сканированный QR код на месте хранения товаров :>");
                var code = Console.ReadLine();
                var datas = code.Split("|");
                if (datas[0] == "SNPPlace")
                {
                    if (int.TryParse(datas[1], out int result))
                    {
                        if (await client.GetPlaceByNumber(result) is { } p)
                        {
                            if (p.ProductTypeId != productTypeId)
                            {
                                Console.WriteLine("Это место хранения товаров содержит товары другого вида, нужно другое место");
                                continue;
                            }
                            else if (p.Count == 0)
                            {
                                Console.WriteLine("Это место хранения товаров пустое, не содержит товары");
                                continue;
                            }
                            place = p;
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"Такой код места хранения товаров отсутствует: {result}");
                        }
                    }
                    Console.WriteLine("Не удалось распознать код места хранения товаров");
                }
                else if (datas[0] == "SNPTaskO")
                    Console.WriteLine("Вы отсканировали задание, но нужно сканировать или выбрать место хранения товаров");
                else if (datas[0] == "SNPUnit")
                {
                    if (int.TryParse(datas[1], out int result))
                    {
                        if (await client.GetProductTypeByFormat(result) is { } pt)
                        {
                            if (pt.Id == productTypeId)
                            {
                                Console.WriteLine("Вы правильно выбрали товар, нужно загружать. Вы отсканировали упаковку с товаром, а нужно сканировать место хранения товаров");
                            }
                            else
                            {
                                Console.WriteLine("Это не тот товар, не этот нужно загружать. Вы отсканировали упаковку с товаром, а нужно сканировать место хранения товаров");
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"Выполнение задания по перевозке товара: [{task.ProductType?.FormatNumber}] {task.ProductType?.Name}");
            Console.WriteLine("Нажмите кнопку после загрузки товара на транспортер");
            Console.ReadKey();

            Console.WriteLine($"Выполнение задания по перевозке товара [{task.ProductType?.FormatNumber}] {task.ProductType?.Name} c места хранения товара [{place.Number}] {place.Name} [{place.Count}/{place.Size}] на грузовик {task.TruckNumber} {task.Loaded}/{task.Count}");
            Console.WriteLine("Нажмите кнопку после загрузки товара на грузовик с транспортера");

            if (await client.ShipmentProductToPlace(place.Id, task.Id, 1) is { } newp)
            {
                var newtask = await client.GetById(task.Id);
                Console.WriteLine($"Успешно изменено количестово товара на месте хранения товаров [{newp.Number}] {newp.Name} [{newp.Count}/{newp.Size}], загружено в грузовик {newtask.TruckNumber} [{newtask.Loaded}/{newtask.Count}]");
            }
            else
            {
                Console.WriteLine("Не удалось добавить число товаров к месту хранения товаров");
            }
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

