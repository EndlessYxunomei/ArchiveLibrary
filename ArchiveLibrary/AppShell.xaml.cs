﻿using ArchiveLibrary.Services;
using VMLayer.Navigation;

namespace ArchiveLibrary;

public partial class AppShell : Shell
{
    readonly INavigationInterceptor interceptor;

    public AppShell(INavigationInterceptor interceptor)
    {
        this.interceptor = interceptor;
        InitializeComponent();
    }

    protected override async void OnNavigated(ShellNavigatedEventArgs args)
    {
        var navigationType = AppShell.GetNavigationType(args.Source);

        base.OnNavigated(args);

        await interceptor.OnNavigatedTo(CurrentPage.BindingContext, navigationType);
    }

    private static NavigationType GetNavigationType(ShellNavigationSource source)
        => source switch
        {
            ShellNavigationSource.Push or
            ShellNavigationSource.Insert
                => NavigationType.Forward,
            ShellNavigationSource.Pop or
            ShellNavigationSource.PopToRoot or
            ShellNavigationSource.Remove
                => NavigationType.Back,
            ShellNavigationSource.ShellItemChanged or
            ShellNavigationSource.ShellSectionChanged or
            ShellNavigationSource.ShellContentChanged
                => NavigationType.SectionChange,
            _ => NavigationType.Unknown
        };
}
