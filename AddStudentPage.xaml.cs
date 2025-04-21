using MauiApp1.Dtos;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;

namespace MauiApp1;

public partial class AddStudentPage : ContentPage
{
    private StudentDto studentDto;
    private ImageSource pIS;

    public ImageSource PIS { get => pIS; set { pIS = value;OnPropertyChanged(); } }

    public ObservableCollection<AddressDto> AddressList { get; set; }=new ObservableCollection<AddressDto>();

    public AddStudentPage()
	{
		InitializeComponent();
        studentDto=new StudentDto();
        BindingContext = this;
	}

    private void AABtn(object sender, EventArgs e)
    {
        AddressList.Add(new AddressDto
        {
            City = EntryCity.Text,
            Street = EntryStreet.Text,
        });
        EntryCity.Text = "";
        EntryStreet.Text = "";

    }

    private async void UPIBtn(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions { PickerTitle = "Select Image" });
        if (result != null) 
        {
            var s = await result.OpenReadAsync();
            var ms= new MemoryStream();
            await s.CopyToAsync(ms);
            byte[] data = ms.ToArray();
            studentDto.BaseImage64 = Convert.ToBase64String(data);
            PIS = ImageSource.FromFile(result.FullPath);
        }
    }

    private async void SSBtn(object sender, EventArgs e)
    {
        studentDto.Name = EntryName.Text;
        studentDto.AdmissionDate = ADPaker.Date;
        studentDto.IsActive=CHBox.IsChecked;
        studentDto.ImageUrl = studentDto.BaseImage64;
        studentDto.Addresses = AddressList.ToList();
        studentDto.AddressJson=JsonSerializer.Serialize(AddressList);


        var client = new HttpClient
        {
            BaseAddress = new Uri
            (DeviceInfo.Platform == DevicePlatform.Android ?
                "http://10.0.2.2:5286" : "http://localhost:5286"
            )
        };

        var content = new StringContent
        (
            System.Text.Json.JsonSerializer.Serialize(studentDto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            }), Encoding.UTF8, "application/json"

        );

        using var res = await client.PostAsync("/api/stu", content);
        if (res.IsSuccessStatusCode)
        {
            await DisplayAlert("Seccess", "Crested Student", "Ok");
            await Navigation.PushAsync(new StidentListPage());
        }
        else 
        {
            await DisplayAlert("Error", "Error Occour", "Ok");

        }

    }
}