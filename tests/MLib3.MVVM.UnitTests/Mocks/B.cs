using System.ComponentModel;

namespace MLib3.MVVM.UnitTests.Mocks;

public class B : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private C _c = new C();
    public C C
    {
        get => _c;
        set
        {
            _c = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(C)));
        }
    }
}