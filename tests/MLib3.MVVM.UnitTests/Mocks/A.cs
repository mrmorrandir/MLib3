using System.ComponentModel;

namespace MLib3.MVVM.UnitTests.Mocks;

public class A : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private B _b = new B();
    public B B
    {
        get => _b;
        set
        {
            _b = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(B)));
        }
    }
}