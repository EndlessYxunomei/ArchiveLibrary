using VMLayer;

namespace ArchiveLibrary;

public partial class DocumentListPage : ContentPage
{
    public DocumentListPage(DocumentListViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}