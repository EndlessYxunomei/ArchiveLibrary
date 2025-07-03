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
        //базовые сообщения
        public async Task<string?> Ask(string title, string message, string accepButtonText = "ОК", string cancelButtonText = "Отмена")
        {
            return await Shell.Current.DisplayPromptAsync(title, message, accepButtonText, cancelButtonText);
            //для этого убрать асинк
            //return await Application.Current?.MainPage?.DisplayPromptAsync(title, message, accepButtonText, cancelButtonText) ?? throw new NullReferenceException();
        }
        public async Task<bool> AskYesNo(string title, string message, string trueButtonText = "Да", string falseButtonText = "Нет")
        {
            return await Shell.Current.DisplayAlert(title, message,trueButtonText, falseButtonText);
        }
        public async Task Notify(string title, string message, string buttonText = "ОК")
        {
            await Shell.Current.DisplayAlert(title, message, buttonText);
        }

        //специальные всплывашки
        //ранее использовался IPopupService от MAUITollkit
        public Task ClosePopup(object? popupView, object? parameters = null)
        {
            //заглушка
            return Task.CompletedTask;
        }
    }
}
