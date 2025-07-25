using VMLayer;

namespace ArchiveLibrary;

public partial class PersonDetailPage : ContentPage
{
	public PersonDetailPage(PersonDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}