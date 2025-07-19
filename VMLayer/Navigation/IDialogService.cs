namespace VMLayer.Navigation;

public interface IDialogService
{
    Task Notify(string title, string message, string buttonText = "ОК");
    Task<bool> AskYesNo(string title, string message, string trueButtonText = "Да", string falseButtonText = "Нет");
    Task<string> Ask(string title, string message, string accepButtonText = "ОК", string cancelButtonText = "Отмена");
    Task<string> Ask(string title, string message, int maxLength = 100, string accepButtonText = "ОК", string cancelButtonText = "Отмена");
    Task<string> Ask(string title, string message, string oldValue, int maxLength = 100, string accepButtonText = "ОК", string cancelButtonText = "Отмена");

    //Task ClosePopup(object? popupView, object? parameters = null);

    //Task<object?> ShowCompanyDetailPopup(int id = 0);
    //Task<object?> ShowApplicabilityPopup(int originalId = 0, object? parameters = null);
}
