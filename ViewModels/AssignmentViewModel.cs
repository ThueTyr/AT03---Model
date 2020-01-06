using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AT03___Model.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace AT03___Model.ViewModels
{
    public class AssignmentViewModel: BindableBase
    {
        public AssignmentViewModel(string title, Assignment assignment)
        {
            Title = title;
            CurrentAssignment = assignment;
        }

        string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private Assignment _currentAssignment;

        public Assignment CurrentAssignment
        {
            get => _currentAssignment;
            set => SetProperty(ref _currentAssignment, value);
        }

        private bool _isValid = false;

        public bool IsValid
        {
            get
            {
                //if (string.IsNullOrWhiteSpace(CurrentAssignment.ClientName))
                //    return false;
                //if (string.IsNullOrWhiteSpace(CurrentAssignment.DurationInDays.ToString()))
                //    return false;
                //if (string.IsNullOrWhiteSpace(CurrentAssignment.StartDate.ToString()))
                //    return false;
                //if (string.IsNullOrWhiteSpace(CurrentAssignment.Location))
                //    return false;
                //if (string.IsNullOrWhiteSpace(CurrentAssignment.NumberOfModels.ToString()))
                //    return false;
                //if (string.IsNullOrWhiteSpace(CurrentAssignment.Comments))
                //    return false;
                _isValid = true;
                return _isValid;
            }
        }

        ICommand _okBtnCommand;

        public ICommand OkBtnCommand
        {
            get
            {
                return _okBtnCommand ?? (_okBtnCommand = new DelegateCommand(
                               OkBtnCommand_Execute, OkBtnCommand_CanExecute)
                           .ObservesProperty(() => CurrentAssignment.ClientName)
                           .ObservesProperty(() => CurrentAssignment.DurationInDays)
                           .ObservesProperty(() => CurrentAssignment.Location)
                           .ObservesProperty(() => CurrentAssignment.NumberOfModels)
                           .ObservesProperty(() => CurrentAssignment.Comments));
            }
        }

        private void OkBtnCommand_Execute()
        {
            // Nothing to do here
        }

        private bool OkBtnCommand_CanExecute()
        {
            return IsValid;
        }
    }
}
