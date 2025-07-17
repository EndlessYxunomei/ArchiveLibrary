using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using VMLayer.Navigation;

namespace VMLayer
{
    public class BaseDetailViewModel : ObservableValidator, INavigationParameterReceiver
    {
        //Сервисы
        private protected readonly INavigationService navigationService;
        private protected readonly IDialogService dialogService;

        //Кнопки Сохранить и отмена
        public IAsyncRelayCommand AcseptCommand { get; }
        public IAsyncRelayCommand CancelCommand { get; }

        private protected virtual Task SaveChanges()
        {
            return Task.CompletedTask;
        }
        private protected async Task CancelChanges()
        {
            ErrorsChanged -= ViewModel_ErrorsChanged;
            await navigationService.GoBack();
        }

        public BaseDetailViewModel(INavigationService navigationService, IDialogService dialogService)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;

            AcseptCommand = new AsyncRelayCommand(SaveChanges, () => !HasErrors);
            CancelCommand = new AsyncRelayCommand(CancelChanges);

            ErrorsChanged += ViewModel_ErrorsChanged;
            ValidateAllProperties();
        }

        public virtual Task OnNavigatedTo(Dictionary<string, object> parameters)
        {
            return Task.CompletedTask;
        }

        //Обработка валидатора
        private protected void ViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e) => AcseptCommand.NotifyCanExecuteChanged();
    }
}
