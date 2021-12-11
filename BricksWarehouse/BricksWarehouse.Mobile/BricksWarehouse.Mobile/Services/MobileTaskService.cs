using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BricksWarehouse.Domain.Models;

namespace BricksWarehouse.Mobile.Services
{
    /// <summary> Сервис выполнения заданий в мобильном устройстве </summary>
    public class MobileTaskService
    {
        public MobileTaskService()
        {
            
        }

        public Task<IEnumerable<OutTask>> GetNonCompletedAllOutTasks()
        {
            return null;
        }

        public Task<OutTask> GetOneOutTask()
        {
            return null;
        }
    }
}
