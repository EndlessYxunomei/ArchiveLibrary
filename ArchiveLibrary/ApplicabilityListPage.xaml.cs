using VMLayer;

namespace ArchiveLibrary;

public partial class ApplicabilityListPage : ContentPage
{
	public ApplicabilityListPage(ApplicabilityListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}