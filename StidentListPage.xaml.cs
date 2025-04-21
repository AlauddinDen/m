using MauiApp1.Dtos;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace MauiApp1;

public partial class StidentListPage : ContentPage
{
    public ObservableCollection<StudentDto> StudentList { get; set; }=new ObservableCollection<StudentDto>();


    public StidentListPage()
	{
		InitializeComponent();
        BindingContext=this;
        _ = LodeStudent();
	}

    private async Task LodeStudent()
    {
        var client = new HttpClient
        {
            BaseAddress = new Uri
            (DeviceInfo.Platform == DevicePlatform.Android ?
                "http://10.0.2.2:5286" : "http://localhost:5286"
            )
        };

        var res = await client.GetAsync("/api/stu");

        if (res.IsSuccessStatusCode) 
        {
            var student = await res.Content.ReadAsStringAsync();

            StudentList = JsonSerializer.Deserialize<ObservableCollection<StudentDto>>
                (student, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (StudentList != null)
            {
                StudentListView.ItemsSource = StudentList;
            }
            else 
            {
                await DisplayAlert("eror", "Error Occour", "Ok");
            }
        }
    }

    private void ASBtn(object sender, EventArgs e)
    {
		Navigation.PushAsync(new AddStudentPage());
    }

    private async void DeleteBtn(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is StudentDto studentDto) 
        {
            bool result = await DisplayAlert("Delete", $"Delete this student {studentDto.Name}", "Yes", "No");
            if (result) 
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri
                    (DeviceInfo.Platform == DevicePlatform.Android ?
                        "http://10.0.2.2:5286" : "http://localhost:5286"
                    )
                };

                var res = await client.DeleteAsync($"/api/stu/{studentDto.Id}");
                if (res.IsSuccessStatusCode)
                {
                    await DisplayAlert("Succes", "Deleted", "Ok");
                    await Navigation.PushAsync(new StidentListPage());

                }
                else 
                {
                    await DisplayAlert("eror", "Error Occour", "Ok");
                }

            }
        }
    }
}