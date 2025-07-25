using VMLayer;

namespace ArchiveLibrary;

public partial class PersonListPage : ContentPage
{
	public PersonListPage(PersonListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}