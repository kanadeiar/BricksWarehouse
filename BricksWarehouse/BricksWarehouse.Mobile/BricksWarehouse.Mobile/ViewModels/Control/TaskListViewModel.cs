using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BricksWarehouse.Domain.Models;
using BricksWarehouse.Mobile.Services;
using BricksWarehouse.Mobile.ViewModels.Base;
using BricksWarehouse.Mobile.Views.Control;
using Xamarin.Forms;
using ZXing.Mobile;

namespace BricksWarehouse.Mobile.ViewModels.Control
{
    public class TaskListViewModel : ViewModel
    {
        #region Данные

        private readonly MobileTaskService _MobileTaskService;
        private readonly ParseQrService _ParseQrService;
        private IEnumerable<OutTask> _Tasks { get; set; }

        #endregion

        #region Свойства

        public ObservableCollection<OutTaskView> OutTasks { get; set; } = new ObservableCollection<OutTaskView>();

        private OutTaskView _selectedOutTask;
        /// <summary> Выбранная задача </summary>
        public OutTaskView SelectedOutTask
        {
            get => _selectedOutTask;
            set => Set(ref _selectedOutTask, value);
        }

        private bool _refreshingOutTasks;
        /// <summary> Обновление данных </summary>
        public bool RefreshingOutTasks
        {
            get => _refreshingOutTasks;
            set => Set(ref _refreshingOutTasks, value);
        }

        private string _title = "Управление складом";
        /// <summary> Заголовок </summary>
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        #endregion

        public TaskListViewModel(MobileTaskService mobileTaskService, ParseQrService parseQrService)
        {
            _MobileTaskService = mobileTaskService;
            _ParseQrService = parseQrService;

            Task.Run(async () =>
            {
                await UpdateDataAsync();
                RefreshingOutTasks = false;
            });
        }

        #region Команды

        private ICommand _ScanQrCodeCommand;
        /// <summary> Сканировать QR код </summary>
        public ICommand ScanQrCodeCommand => _ScanQrCodeCommand ??=
            new Command(OnScanQrCodeCommandExecuted);
        private async void OnScanQrCodeCommandExecuted()
        {
            var scanner = new MobileBarcodeScanner();
            scanner.UseCustomOverlay = false;
            scanner.TopText = "Поместите QR-код в видоискатель камеры для его сканирования.";
            scanner.BottomText = "QR-код сканируется автоматически. Постарайтесь избегать теней и бликов. И старайтесь соблюдать расстояние между устройством и кодом в 15 см.";
            var result = await scanner.Scan(new MobileBarcodeScanningOptions { AutoRotate = false });
            if (result != null && !string.IsNullOrEmpty(result.Text))
            {
                var (errorQr, datas) = _ParseQrService.Get(TypeQrCode.OutTask, result.Text);
                if (string.IsNullOrEmpty(errorQr))
                {
                    var number = int.Parse(datas[1]);
                    OutTask selected;
                    if (number != 0)
                        selected = _Tasks.FirstOrDefault(ot => ot.Number == number);
                    else
                        selected = new OutTask { Id = 0 };
                    await Application.Current.MainPage.Navigation.PushAsync(new TaskDetailPage(selected));
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Сканирование не удалось", errorQr, "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Сканирование отменено", "Сканирование отменено", "OK");
            }
        }

        private ICommand _ShowDetailsOutTaskCommand;
        /// <summary> Просмотр детальной информации по заданию </summary>
        public ICommand ShowDetailsOutTaskCommand => _ShowDetailsOutTaskCommand ??=
            new Command(OnShowDetailsOutTaskCommandExecuted, CanShowDetailsOutTaskCommandExecute);
        private bool CanShowDetailsOutTaskCommandExecute(object p) => p is OutTaskView;
        private async void OnShowDetailsOutTaskCommandExecuted(object p)
        {
            var model = p as OutTaskView;
            OutTask selected;
            if (model.Id != 0)
                selected = _Tasks.FirstOrDefault(ot => ot.Id == model.Id);
            else
                selected = new OutTask { Id = 0 };
            await Application.Current.MainPage.Navigation.PushAsync(new TaskDetailPage(selected));
        }

        private ICommand _StartWorkTaskCommand;
        /// <summary> Начало выполнения задания </summary>
        public ICommand StartWorkTaskCommand => _StartWorkTaskCommand ??=
            new Command(OnStartWorkTaskCommandExecuted, CanStartWorkTaskCommandExecute);
        private bool CanStartWorkTaskCommandExecute(object p) => p is OutTaskView;
        private async void OnStartWorkTaskCommandExecuted(object p)
        {
            var model = p as OutTaskView;
            await _MobileTaskService.SetTaskWithNumber(model!.Number);
            if (model!.Number == 0)
                await Application.Current.MainPage.Navigation.PushAsync(new StartLoadTaskPage());
            else
            {
                _MobileTaskService.StartShippingTask(_MobileTaskService.OutTask);
                await Application.Current.MainPage.Navigation.PushAsync(new StartShippingTaskPage());
            }
        }

        private ICommand _UpdateOutTasksCommand;
        /// <summary> Обновить </summary>
        public ICommand UpdateOutTasksCommand => _UpdateOutTasksCommand ??= new Command(OnUpdateOutTasksCommandExecuted);
        private async void OnUpdateOutTasksCommandExecuted(object p)
        {
            RefreshingOutTasks = true;
            await UpdateDataAsync();
            RefreshingOutTasks = false;
        }

        #endregion

        #region Вспомогательное

        public async Task UpdateDataAsync()
        {
            _Tasks = (await _MobileTaskService.GetAllOutTasks(true));
            OutTasks.Clear();
            SelectedOutTask = null;
            OutTasks.Add(new OutTaskView
            {
                Id = 0,
                Name = "Заполнение склада",
                Number = 0,
                ProductTypeName = (_MobileTaskService.ProductType is { }) ? $"Поледний отгруженный вид товара:" : "Работа по заполнению склада товаром",
                TruckNumber = (_MobileTaskService.ProductType is { } productType) ? $"[{productType?.FormatNumber}] {productType?.Name}" : "",
            });
            foreach (var task in _Tasks)
                OutTasks.Add(new OutTaskView
                {
                    Id = task.Id,
                    Name = task.Name,
                    Number = task.Number,
                    ProductTypeName = $"Вид: [{task.ProductType?.FormatNumber}] {task.ProductType?.Name}",
                    TruckNumber = $"Номер: {task.TruckNumber}",
                    CountString = $"Отгружено: {task.Loaded} / {task.Count}",
                });
        }

        #endregion
    }

    #region Вспомогательные вьюмодели

    public class OutTaskView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public string ProductTypeName { get; set; }
        public string TruckNumber { get; set; }
        public string CountString { get; set; }
    }

    #endregion
}
