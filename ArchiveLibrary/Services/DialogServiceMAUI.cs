using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMLayer.Navigation;

namespace ArchiveLibrary.Services
{
    public class DialogServiceMAUI : IDialogService
    {
        public Task<string?> Ask(string title, string message, string accepButtonText = "ОК", string cancelButtonText = "Отмена")
        {
            throw new NotImplementedException();
        }

        public Task<bool> AskYesNo(string title, string message, string trueButtonText = "Да", string falseButtonText = "Нет")
        {
            throw new NotImplementedException();
        }

        public Task ClosePopup(object? popupView, object? parameters = null)
        {
            throw new NotImplementedException();
        }

        public Task Notify(string title, string message, string buttonText = "ОК")
        {
            throw new NotImplementedException();
        }
    }
}
