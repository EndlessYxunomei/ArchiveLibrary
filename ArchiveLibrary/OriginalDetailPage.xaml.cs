using System.Text.RegularExpressions;
using VMLayer;

namespace ArchiveLibrary;

public partial class OriginalDetailPage : ContentPage
{
	public OriginalDetailPage(OriginalDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    // Method triggered by TextChanged.
    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        // If the text field is empty or null then leave.
        string regex = e.NewTextValue;
        if (string.IsNullOrEmpty(regex))
            return;

        // If the text field only contains numbers then leave.
        if (!Regex.Match(regex, "^[0-9]+$").Success)
        {
            // This returns to the previous valid state.
            var entry = sender as Entry;
            entry!.Text = string.IsNullOrEmpty(e.OldTextValue)
                    ? string.Empty
                    : e.OldTextValue;
        }
    }
}