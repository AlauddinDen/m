using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Dtos
{
    public class StudentDto:INotifyPropertyChanged
    {
        private int id;
        private string name;
        private DateTime admissionDate;
        private bool isActive;
        private string? imageUrl;
        private string addressJson;
        private string baseImage64;
        private List<AddressDto> addresses = new List<AddressDto>() { new AddressDto() };

        public event PropertyChangedEventHandler? PropertyChanged;
        public void NPC([CallerMemberName] string propName = "") 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public int Id { get => id; set { id = value;NPC(); } }
        public string Name { get => name; set { name = value;NPC(); } }
        public DateTime AdmissionDate { get => admissionDate; set { admissionDate = value;NPC(); } }
        public bool IsActive { get => isActive; set { isActive = value;NPC(); } }
        public string? ImageUrl { get => imageUrl; set { imageUrl = value;NPC(); } }
        public string AddressJson { get => addressJson; set { addressJson = value;NPC(); } }
        public string BaseImage64 { get => baseImage64; set { baseImage64 = value;NPC(); } }
        public List<AddressDto> Addresses { get => addresses; set { addresses = value;NPC(); } }

    }

}
