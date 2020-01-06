using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;

namespace AT03___Model.Models
{
    public class Assignment
    {
        public string ClientName { get; set; }
        public DateTime StartDate { get; set; }
        public int DurationInDays { get; set; }
        public string Location { get; set; }
        public int NumberOfModels { get; set; }
        public string Comments { get; set; }
        public ObservableCollection<Model> AssignedModels { get; set; }

        public Assignment() { }

        public Assignment(string clientName, DateTime startDate, int durationInDays, string location, int numberOfModels,
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
    }
}
