using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BricksWarehouse.Mobile.Services;
using BricksWarehouse.Mobile.ViewModels.Base;
using Xamarin.Forms;

namespace BricksWarehouse.Mobile.ViewModels.Control
{
    public class TaskListViewModel : ViewModel
    {
        #region Данные

        private readonly MobileTaskService _mobileTaskService;

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

        public TaskListViewModel(MobileTaskService mobileTaskService)
        {
            _mobileTaskService = mobileTaskService;

            Task.Run(async () =>
            {
                await UpdateDataAsync();
                RefreshingOutTasks = false;
            });
        }

        #region Команды

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
            var tasks = (await _mobileTaskService.GetAllOutTasks(true));
            OutTasks.Clear();
            OutTasks.Add(new OutTaskView
            {
                Id = 0,
                Name = "Заполнение склада",
                Number = 0,
                ProductTypeName = (_mobileTaskService.ProductType is { }) ? $"Поледний отгруженный вид товара:" : "Работа по заполнению склада товаром",
                TruckNumber = (_mobileTaskService.ProductType is { } productType) ? $"[{productType?.FormatNumber}] {productType?.Name}" : "",
            });
            foreach (var task in tasks)
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
