using BricksWarehouseConsole;
using System.Runtime.InteropServices;

[DllImport("kernel32.dll")] static extern bool SetConsoleCP(uint pagenum);
[DllImport("kernel32.dll")] static extern bool SetConsoleOutputCP(uint pagenum);
SetConsoleCP(65001);        //установка кодовой страницы utf-8 (Unicode) для вводного потока
SetConsoleOutputCP(65001);  //установка кодовой страницы utf-8 (Unicode) для выводного потока

Console.WriteLine("------- Тестирование ---------");

//await TestClientApi.TestOfTypeProducts();
await TestClientApi.TestOfPlaces();

Console.WriteLine("\nДля завершения нажмите любую кнопку ...");
Console.ReadLine();
