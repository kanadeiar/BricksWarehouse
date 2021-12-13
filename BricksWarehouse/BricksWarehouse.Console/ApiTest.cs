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

        MobileTaskService service = new MobileTaskService(client);
        ParseQrService qrService = new ParseQrService();

        Console.WriteLine("Все задания:");
        foreach (var item in (await service.GetAllOutTasks()))
        {
            Console.WriteLine($"[{item.Id}] {item.Name} {item.Number} рег.номер: {item.TruckNumber} {item.Loaded}/{item.Count}");
        }

        Console.WriteLine("Незавершенные задания:");
        foreach (var item in await service.GetAllOutTasks(true))
        {
            Console.WriteLine($"[{item.Id}] {item.Name} {item.Number} рег.номер: {item.TruckNumber} {item.Loaded}/{item.Count}");
        }

        var it = await service.GetOneOutTask(1);
        Console.WriteLine($"\nОдин элемент: [{it.Id}] {it.Name} Формат: {it.Number} рег.номер: {it.TruckNumber} {it.Loaded}/{it.Count}\n");

        Console.WriteLine("Тестирование автоматизации");
        Console.WriteLine("Нажмите любую кнопку ...");
        Console.ReadKey();
                
        await service.SetTaskWithNumber(99);
        //while (true)
        //{
        //    Console.WriteLine("Введите сканированный QR код задания :>");
        //    var code = Console.ReadLine();
        //    var (errorQr, datas) = qrService.Get(TypeQrCode.OutTask, code);
        //    if (string.IsNullOrEmpty(errorQr))
        //    {
        //        await service.SetTaskWithNumber(int.Parse(datas[1]));
        //        break;
        //    }
        //    Console.WriteLine(errorQr);
        //}
        if (service.OutTask.Number == 0)
        {
            Console.WriteLine("*** Выполнение задания \"Прием товара на склад\" ***");
            while (true)
            {
                Console.WriteLine("Введите сканированный QR код на упаковке товара :>");
                var code = Console.ReadLine();
                var(errorQr, datas) = qrService.Get(TypeQrCode.ProductType, code);
                if (string.IsNullOrEmpty(errorQr))
                {
                    if (await service.GetProductTypeByNumber(int.Parse(datas[1])) is { } pt)
                    {
                        service.StartLoadTask(pt);
                        break;
                    }
                    Console.WriteLine($"Такой код товара в базе данных отсутствует: {datas[1]}");
                }
                Console.WriteLine(errorQr);
            }
            Console.WriteLine($"\nВыполнение задания по приему товара на склад: [{service.ProductType.FormatNumber}] {service.ProductType.Name}");
            Console.WriteLine("Нажмите кнопку после загрузки товара на транспортер");
            Console.ReadKey();

            Console.WriteLine("Возможные места хранения для этого перевозимого товара на транспортере:");
            var places = await service.GetRecommendedLoadPlaces(service.ProductType.Id);
            foreach (var item in places)
            {
                Console.WriteLine($"[{item.Number}] {item.Name} уже хранящийся вид товаров: [{item.ProductType?.FormatNumber}] {item.ProductType?.Name} занято: [{item.Count}/{item.Size}]");
            }
            
            while (true)
            {
                Console.WriteLine("\nВведите сканированный QR код на месте хранения товаров :>");
                var code = Console.ReadLine();
                var(errorQr, datas) = qrService.Get(TypeQrCode.Place, code);
                if (string.IsNullOrEmpty(errorQr))
                {
                    if (await service.GetPlaceByNumber(int.Parse(datas[1])) is { } p)
                    {
                        service.BeginLoadTask(p);
                        break;
                    }
                    Console.WriteLine($"Такой код места хранения товаров отсутствует в базе данных: {datas[1]}");
                }
                Console.WriteLine(errorQr);
            }
            Console.WriteLine($"\nВыполнение задания по перевозке товара [{service.ProductType.FormatNumber}] {service.ProductType.Name} на место хранения товара [{service.Place.Number}] {service.Place.Name} [{service.Place.Count}/{service.Place.Size}]");
            Console.WriteLine("Нажмите кнопку после выгрузки товара с транспортера на место хранения товара");
            Console.ReadKey();
            if (await service.EndLoadTask(1) is { } newp)
                Console.WriteLine($"Успешно изменено количестово товара на месте хранения товаров [{newp.Number}] {newp.Name} - {newp.ProductType?.Name} [{newp.Count}/{newp.Size}]");
            else
                Console.WriteLine("Не удалось добавить число товаров к месту хранения товаров");
        }
        else
        {            
            Console.WriteLine("*** Выполнение задания \"Отгрузка товара со склада на грузовик\" ***");
            Console.WriteLine($"Задание: [{service.OutTask.Number}] {service.OutTask.Name} рег.номер грузовика: {service.OutTask.TruckNumber} вид товара: {service.OutTask.ProductType?.Name} загружено: {service.OutTask.Loaded}/{service.OutTask.Count}");
            service.StartShippingTask(service.OutTask);

            Console.WriteLine("Возможные доступные места хранения с таким товаром:");
            service.ProductType = service.OutTask.ProductType;
            foreach (var item in (await service.GetRecommendedShipmentPlaces(service.ProductType!.Id)))
            {
                Console.WriteLine($"{item.Name} [{item.Number}], хранящийся вид товаров: {item.ProductType?.Name} занято: [{item.Count}/{item.Size}]");
            }
            while (true)
            {
                Console.WriteLine($"Введите сканированный QR код на месте хранения товаров с товаром [{service.OutTask.ProductType?.FormatNumber}] {service.OutTask.ProductType?.Name} :>");
                var code = Console.ReadLine();
                var (errorQr, datas) = qrService.Get(TypeQrCode.Place, code);
                if (string.IsNullOrEmpty(errorQr))
                {
                    if (await service.GetPlaceByNumber(int.Parse(datas[1])) is { } p)
                    {
                        if (p.ProductTypeId != service.ProductType.Id)
                        {
                            Console.WriteLine("Это место хранения товаров уже содержит товары другого вида, выберите другое место хранения товаров.");
                            continue;
                        }
                        if (p.Count == 0)
                        {
                            Console.WriteLine("Это место хранения товаровe уже пустое, оно не содержит каких-либо товаров.");
                            continue;
                        }
                        service.BeginShippingTask(p);
                        break;
                    }
                    Console.WriteLine($"Такой код места хранения товаров отсутствует в базе данных: {datas[1]}");
                }
                if (datas[0] == "SNPUnit")
                {
                    if (int.TryParse(datas[1], out int result))
                    {
                        if (await service.GetProductTypeByNumber(result) is { } pt)
                        {
                            Console.WriteLine(pt.Id == service.ProductType.Id
                                ? "Правильно! Вы выбрали товар верно, можно загружать. Вы отсканировали упаковку с товаром, а нужно сканировать место хранения товаров"
                                : "Неправильно! Это не тот товар, его НЕ НУЖНО загружать. Вы отсканировали упаковку с товаром, а нужно сканировать место хранения товаров");
                        }
                    }
                }
                else
                    Console.WriteLine(errorQr);
            }

            Console.WriteLine($"Выполнение задания по перевозке товара: [{service.OutTask.ProductType?.FormatNumber}] {service.OutTask.ProductType?.Name}");
            Console.WriteLine("Нажмите кнопку после загрузки товара на транспортер");
            Console.ReadKey();

            Console.WriteLine($"Выполнение задания по перевозке товара [{service.OutTask.ProductType?.FormatNumber}] {service.OutTask.ProductType?.Name} c места хранения товара [{service.Place.Number}] {service.Place.Name} [{service.Place.Count}/{service.Place.Size}] на грузовик {service.OutTask.TruckNumber} {service.OutTask.Loaded}/{service.OutTask.Count}");
            Console.WriteLine("Нажмите кнопку после выгрузки товара с транспортера на грузовик");

            if (await service.EndShippingTask(1) is { } newp)
            {
                var newtask = await service.GetOneOutTask(service.OutTask.Id);
                Console.WriteLine($"Успешно изменено количестово товара на месте хранения товаров [{newp.Number}] {newp.Name} [{newp.Count}/{newp.Size}], загружено в грузовик {newtask.TruckNumber} [{newtask.Loaded}/{newtask.Count}]");
            }
            else
            {
                Console.WriteLine("Не удалось добавить нужное число товаров к месту хранения товаров");
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

