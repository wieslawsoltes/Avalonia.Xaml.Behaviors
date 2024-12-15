using System.Collections.ObjectModel;

namespace BehaviorsTestApplication.ViewModels;

public partial class ItemViewModel : ViewModelBase
{
    public ItemViewModel(string value)
    {
        _value = value;
    }

    [Reactive]
    public partial string? Value { get; set; }

    [Reactive]
    public partial ObservableCollection<ItemViewModel>? Items { get; set; }

    public override string ToString() => _value ?? string.Empty;
}
