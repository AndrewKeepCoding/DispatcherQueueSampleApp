using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;
using System;
using System.Threading.Tasks;

namespace DispatcherQueueSampleApp;

public static class DispatcherQueueExtensions
{
    public static bool TryEnqueue(
        this DispatcherQueue dispatcherQueue,
        Action enqueueingAction,
        Action<Exception> exceptionAction)
    {
        return dispatcherQueue.TryEnqueue(() =>
        {
            try
            {
                enqueueingAction();
            }
            catch (Exception exception)
            {
                exceptionAction(exception);
            }
        });
    }
}

public partial class MainPageViewModel : ObservableObject
{
    private readonly DispatcherQueue dispatcherQueue;

    [ObservableProperty]
    private string text = string.Empty;

    public MainPageViewModel()
    {
        this.dispatcherQueue = DispatcherQueue.GetForCurrentThread();
    }

    [RelayCommand]
    private async Task UpdateText()
    {
        await Task.Run(() =>
        {
            this.dispatcherQueue.TryEnqueue(() =>
            {
                Text = DateTime.Now.ToString();
                throw new Exception("Something went wrong!");
            },
            exception =>
            {
                Text = exception.Message;
            });
        });
    }
}