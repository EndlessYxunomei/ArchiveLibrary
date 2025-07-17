using ArchiveModels.DTO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ServiceLayer;
using ServiceLayer.Interfaces;
using System.Collections.ObjectModel;
using VMLayer.Navigation;

namespace VMLayer;

public class DocumentListViewModel : ObservableObject, INavigationParameterReceiver
{
    //Сервисы
    private readonly IDocumentService documentService;
    private readonly IDialogService dialogService;
    private readonly INavigationService navigationService;

    //Приватные поля
    private DocumentListDto? _selectedDocument;

    //Свойства
    public DocumentListDto? SelectedDocument
    {
        get => _selectedDocument;
        set
        {
            if (SetProperty(ref _selectedDocument, value))
            {
                DeleteCommand.NotifyCanExecuteChanged();
                EditCommand.NotifyCanExecuteChanged();
            }
        }
    }
    public ObservableCollection<DocumentListDto> DocumentList { get; set; } = [];

    //Кнопки
    public IAsyncRelayCommand CreateCommand { get; }
    public IAsyncRelayCommand DeleteCommand { get; }
    public IAsyncRelayCommand EditCommand { get; }
    private async Task CreateDocument() => await navigationService.GoToDocumentDetails();
    private async Task EditDocument()
    {
        if (SelectedDocument != null)
        {
            await navigationService.GoToDocumentDetails(SelectedDocument.Id);
        }
    }
    private async Task DeleteDocument()
    {
        if (SelectedDocument != null)
        {
            var result = await dialogService.AskYesNo("Удаление данных", $"Вы действительно хотите удалить {SelectedDocument.Name} от {SelectedDocument.Date : d}?");
            if (result)
            {
                //Удаление оригинала
                var delResult = await documentService.DeleteDocument(SelectedDocument.Id);
                if (delResult.IsSuccess)
                {
                    //обновление списка
                    DocumentList.Remove(SelectedDocument);
                    SelectedDocument = null;

                    await dialogService.Notify("Удалено", "Документ удалён");
                }
                else
                {
                    await dialogService.Notify("Ошибка удаления", delResult.ErrorCode);
                }

            }
        }
    }
    private bool CanEditDeleteDocument() => SelectedDocument != null;

    //конструктор
    public DocumentListViewModel(IDialogService dialogService, INavigationService navigationService, IDocumentService documentService)
    {
        this.documentService = documentService;
        this.dialogService = dialogService;
        this.navigationService = navigationService;

        CreateCommand = new AsyncRelayCommand(CreateDocument);
        DeleteCommand = new AsyncRelayCommand(DeleteDocument, CanEditDeleteDocument);
        EditCommand = new AsyncRelayCommand(EditDocument, CanEditDeleteDocument);

        LoadDocumentList();
    }

    private async Task LoadDocumentList()
    {
        var document_list = await documentService.GetDocumentListAsync();
        if (document_list.IsSuccess)
        {
            document_list.Data.ForEach(DocumentList.Add);
        }
    }

    //обработка навигации
    public Task OnNavigatedTo(Dictionary<string, object> parameters)
    {
        if (parameters.TryGetValue(NavParamConstants.DocumentList, out object? doc_list) && doc_list is DocumentListDto documentListDto)
        {
            UtilityService.UpdateList(DocumentList, documentListDto);
        }
        return Task.CompletedTask;
    }
}
