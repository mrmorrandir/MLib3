using System.ComponentModel;

namespace MLib3.MVVM.UnitTests.Mocks;

public class C : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private string _name = nameof(C);
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
        }
    }
}