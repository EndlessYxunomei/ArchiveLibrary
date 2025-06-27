using VMLayer;

namespace ArchiveLibrary;

public partial class OriginalDetailPage : ContentPage
{
	public OriginalDetailPage(OriginalDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}