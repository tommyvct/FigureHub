using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FigureHub.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            GitHubLink = new Command(async () => await Browser.OpenAsync(new Uri("https://github.com/tommyvct/FigureHub")).ConfigureAwait(false));
            LicenceLink = new Command(async () => await Browser.OpenAsync(new Uri("https://github.com/tommyvct/FigureHub")).ConfigureAwait(false));
        }

        public ICommand GitHubLink { get; }
        public ICommand LicenceLink { get; }
    }
}