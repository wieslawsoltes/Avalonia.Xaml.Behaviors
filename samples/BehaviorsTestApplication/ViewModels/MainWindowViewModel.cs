﻿using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;

namespace BehaviorsTestApplication.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private int _value;

    [Reactive]
    public partial int Count { get; set; }

    [Reactive]
    public partial double Position { get; set; }

    [Reactive]
    public partial ObservableCollection<ItemViewModel>? Items { get; set; }

    public IObservable<int> Values { get; }

    public ICommand MoveLeftCommand { get; set; }

    public ICommand MoveRightCommand { get; set; }

    public ICommand ResetMoveCommand { get; set; }

    public MainWindowViewModel()
    {
        Count = 0;
        Position = 100.0;
        MoveLeftCommand = ReactiveCommand.Create(() => Position -= 5.0);
        MoveRightCommand = ReactiveCommand.Create(() => Position += 5.0);
        ResetMoveCommand = ReactiveCommand.Create(() => Position = 100.0);
        Items =
        [
            new("First Item")
            {
                Items =
                [
                    new("First Item Sub Item 1"), new("First Item Sub Item 2"), new("First Item Sub Item 3")
                ]
            },

            new("Second Item")
            {
                Items =
                [
                    new("Second Item Sub Item 1"), new("Second Item Sub Item 2"), new("Second Item Sub Item 3")
                ]
            },

            new("Third Item")
            {
                Items =
                [
                    new("Third Item Sub Item 1"), new("Third Item Sub Item 2"), new("Third Item Sub Item 3")
                ]
            },

            new("Fourth Item")
            {
                Items =
                [
                    new("Fourth Item Sub Item 1"), new("Fourth Item Sub Item 2"), new("Fourth Item Sub Item 3")
                ]
            },

            new("Fifth Item")
            {
                Items =
                [
                    new("Fifth Item Sub Item 1"), new("Fifth Item Sub Item 2"), new("Fifth Item Sub Item 3")
                ]
            },

            new("Sixth Item")
            {
                Items =
                [
                    new("Sixth Item Sub Item 1"), new("Sixth Item Sub Item 2"), new("Sixth Item Sub Item 3")
                ]
            }

        ];

        Values = Observable.Interval(TimeSpan.FromSeconds(1)).Select(_ => _value++);
    }

    public void IncrementCount() => Count++;

    public void DecrementCount(object? sender, object parameter) => Count--;
}
