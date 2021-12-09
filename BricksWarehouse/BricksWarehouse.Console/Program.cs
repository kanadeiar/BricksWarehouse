using BricksWarehouseConsole;
using System.Runtime.InteropServices;

[DllImport("kernel32.dll")] static extern bool SetConsoleCP(uint pagenum);
[DllImport("kernel32.dll")] static extern bool SetConsoleOutputCP(uint pagenum);
SetConsoleCP(1251);        
SetConsoleOutputCP(1251);  

Console.WriteLine("------- Тестирование ---------");

//await ApiTest.TestOfTypeProducts();

ApiTest.TestMobileApi();

Console.WriteLine("\nДля завершения нажмите любую кнопку ...");
Console.ReadLine();
