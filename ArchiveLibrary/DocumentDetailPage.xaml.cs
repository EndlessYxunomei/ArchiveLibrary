using VMLayer;

namespace ArchiveLibrary;

public partial class DocumentDetailPage : ContentPage
{
    public DocumentDetailPage(DocumentDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}