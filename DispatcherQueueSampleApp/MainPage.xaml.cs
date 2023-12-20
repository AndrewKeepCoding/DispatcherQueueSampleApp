using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace DispatcherQueueSampleApp;
public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();
    }

    public MainPageViewModel ViewModel { get; } = new();

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        await Task.Run(() =>
        {
            this.DispatcherQueue.TryEnqueue(() =>
            {
                this.DateTimeTextBlock.Text = DateTime.Now.ToString();
            });
        });
    }
}
