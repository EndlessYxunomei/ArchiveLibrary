using VMLayer;

namespace ArchiveLibrary;

public partial class CompanyListPage : ContentPage
{
	public CompanyListPage(CompanyListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}