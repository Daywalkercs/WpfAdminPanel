using OnlineShop.Data.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public class CarsViewModel : INotifyPropertyChanged
{
    private readonly CarApiClient _client = new CarApiClient();

    public ObservableCollection<Car> Cars { get; set; } = new ObservableCollection<Car>();

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public async Task LoadCarsAsync()
    {
        var cars = await _client.GetCarsAsync();
        Cars.Clear();
        foreach (var car in cars)
        {
            Cars.Add(car);
        }
    }
}
