using VMLayer;

namespace ArchiveLibrary;

public partial class CompanyDetailPage : ContentPage
{
	public CompanyDetailPage(CompanyDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}