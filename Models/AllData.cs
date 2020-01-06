using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AT03___Model.Models
{
    public class AllData
    {
        public ObservableCollection<Model> Models = new ObservableCollection<Model>();
        public ObservableCollection<Assignment> NewAssignments = new ObservableCollection<Assignment>();
        public ObservableCollection<Assignment> PlannedAssignments = new ObservableCollection<Assignment>();

        public AllData() { }

        public AllData(ObservableCollection<Model> models, ObservableCollection<Assignment> newAss, ObservableCollection<Assignment> plannedAssignments)
        {
            Models = models;
            NewAssignments = newAss;
            PlannedAssignments = plannedAssignments;
        }
    }
}
