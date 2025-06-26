using VMLayer;

namespace ArchiveLibrary;

public partial class OriginalListPage : ContentPage
{
	public OriginalListPage(OriginalListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}