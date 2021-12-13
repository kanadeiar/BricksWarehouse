using System;
using System.Collections.Generic;
using System.Text;

namespace BricksWarehouse.Mobile.Services
{
    /// <summary> Сервис распознавания QR кодов в мобильном устройстве </summary>
    public class ParseQrService
    {
        public ParseQrService()
        {
            
        }

        /// <summary> Распарсивание QR кода </summary>
        /// <param name="needCode">Нужный тип кода</param>
        /// <param name="code">Код</param>
        /// <returns>Текст ошибки и результат распарсивания</returns>
        public (string errorQr, string[] datas) GetDataFromQrCode(TypeQrCode needCode, string code)
        {
            var datas = code.Split("|");
            if (needCode == TypeQrCode.OutTask)
            {
                if (datas[0] == "SNPTaskO")
                {
                    if (int.TryParse(datas[1], out int _))
                    {
                        return (null, datas);
                    }
                    return ("Не удалось распознать номер задания", datas);
                }
                if (datas[0] == "SNPUnit")
                    return ("Вы отскарировали упаковку с товаром, а нужно выбрать или отсканировать код задания", datas);
                if (datas[0] == "SNPPlace")
                    return ("Вы отсканировали место хранения товаров, а нужно выбрать или отсканировать код задания", datas);
            }
            else if (needCode == TypeQrCode.ProductType)
            {
                if (datas[0] == "SNPUnit")
                {
                    if (int.TryParse(datas[1], out int _))
                    {
                        return (null, datas);
                    }
                    return ("Не удалось распознать код на упаковке товара", datas);
                }
                if (datas[0] == "SNPTaskO")
                    return ("Вы отсканировали задание, но нужно сканировать или выбрать упаковку с товаром, который загружается на склад", datas);
                if (datas[0] == "SNPPlace")
                    return ("Вы отсканировали место хранения товаров, а нужно выбрать или отсканировать упаковку с товаром, который загружается на склад", datas);
            }
            else if (needCode == TypeQrCode.Place)
            {
                if (datas[0] == "SNPPlace")
                {
                    if (int.TryParse(datas[1], out int _))
                    {
                        return (null, datas);
                    }
                    return ("Не удалось распознать код места хранения товаров", datas);
                }
                if (datas[0] == "SNPTaskO")
                    return ("Вы отсканировали задание, но нужно сканировать или выбрать место хранения товаров", datas);
                if (datas[0] == "SNPUnit")
                    return ("Вы отсканировали упаковку с товаром, а нужно сканировать место хранения товаров", datas);
            }

            return ("QR-код не распознан", datas);
        }
    }

    public enum TypeQrCode
    {
        OutTask,
        ProductType,
        Place
    }
}
