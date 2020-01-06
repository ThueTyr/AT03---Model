using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;
using AT03___Model.Annotations;

namespace AT03___Model.Models
{
    public class Assignment : INotifyPropertyChanged
    {
        public string ClientName { get; set; }
        public DateTime StartDate { get; set; }
        public int DurationInDays { get; set; }
        public string Location { get; set; }
        public int NumberOfModels { get; set; }
        public string Comments { get; set; }
        public ObservableCollection<Model> AssignedModels { get; set; }

        public Assignment()
        {
        }

        public Assignment(string clientName, DateTime startDate, int durationInDays, string location,
            int numberOfModels,
            string comments)
        {
            ClientName = clientName;
            StartDate = startDate;
            DurationInDays = durationInDays;
            Location = location;
            numberOfModels = NumberOfModels;
            Comments = comments;
            AssignedModels = new ObservableCollection<Model>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
