using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using AT03___Model.Models;
using AT03___Model.ViewModels;
using AT03___Model.Views;
using Prism.Commands;
using Prism.Mvvm;

namespace AT03___Model
{
    public class MainWindowViewModel: BindableBase
    {
        #region Fields
        
        private string _filename = "";
        private readonly string AppTitle = "GUI-V-2020";
        private ObservableCollection<Model> _models;
        private Model _currentModel = null;
        private int _currentIndex = 0;
        private ObservableCollection<Assignment> _plannedAssignments = new ObservableCollection<Assignment>();
        private Assignment _currentPlannedAssignment = null;
        private ObservableCollection<Assignment> _newAssignments;
        private Assignment _currentNewAssignment = null;

        #endregion

        #region Properties

        public ObservableCollection<Model> Models
        {
            get => _models;
            set => SetProperty(ref _models, value);
        }

        public Model CurrentModel
        {
            get => _currentModel;
            set => SetProperty(ref _currentModel, value);
        }

        public int CurrentIndex
        {
            get => _currentIndex;
            set => SetProperty(ref _currentIndex, value);
        }

        public ObservableCollection<Assignment> PlannedAssignments
        {
            get => _plannedAssignments;
            set => SetProperty(ref _plannedAssignments, value);
        }

        public Assignment CurrentPlannedAssignment
        {
            get => _currentPlannedAssignment;
            set => SetProperty(ref _currentPlannedAssignment, value);
        }

        public ObservableCollection<Assignment> NewAssignments
        {
            get => _newAssignments;
            set => SetProperty(ref _newAssignments, value);
        }

        public Assignment CurrentNewAssignment
        {
            get => _currentNewAssignment;
            set => SetProperty(ref _currentNewAssignment, value);
        }

        #endregion

        public MainWindowViewModel()
        {
            _models = new ObservableCollection<Model>()
            {
                new Model("Jimmy", "555-1234", "Street 5", 170, 52, "brown", "Some comment..."),
                new Model("Joaquin", "555-3334", "Road 2 1st", 210, 95, "Bald", "Some Other comment...")
            };
            _newAssignments = new ObservableCollection<Assignment>()
            {
                new Assignment("Jim", DateTime.Now, 3, "Tvaer", 4, "Comment1"),
                new Assignment("Jimbo", new DateTime(2018, 11, 12), 5, "Tvaedfgr", 4, "Comments about stuff")
            };

            CurrentModel = null;
            CurrentNewAssignment = null;
        }

        #region SaveDataToFile

        private ICommand _saveToFileCommand;

        public ICommand SaveToFileCommand
        {
            get
            {
                return _saveToFileCommand ?? (_saveToFileCommand =
                           new DelegateCommand(SaveToFileCommand_Execute, SaveToFileCommand_CanExecute)
                               .ObservesProperty(() => Models.Count)
                               .ObservesProperty(() => PlannedAssignments.Count)
                               .ObservesProperty(() => NewAssignments.Count));
            }
        }

        void SaveToFileCommand_Execute()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Model>));
            TextWriter streamWriter = new StreamWriter(_filename);
            serializer.Serialize(streamWriter, Models);
            serializer.Serialize(streamWriter, PlannedAssignments);
            serializer.Serialize(streamWriter, NewAssignments);
            streamWriter.Close();
        }

        private bool SaveToFileCommand_CanExecute()
        {
            return (_filename != "") && (Models.Count > 0);
        }

        private ICommand _saveAsCommand;

        public ICommand SaveAsCommand =>
            _saveAsCommand ?? (_saveAsCommand = new DelegateCommand<string>(SaveAsCommand_Execute));

        void SaveAsCommand_Execute(string f)
        {
            if (f == "")
            {
                MessageBox.Show("Input filename", "Filename missing",
                    MessageBoxButton.OK);
            }
            else
            {
                _filename = f;
                SaveToFileCommand_Execute();
            }
        }

        private ICommand _newFileCommand;

        public ICommand NewFileCommand =>
            _newFileCommand ?? (_newFileCommand = new DelegateCommand(NewFileCommand_Execute));

        void NewFileCommand_Execute()
        {
            MessageBoxResult msbResult = MessageBox.Show(
                "Unsaved data will be lost", "Creating new file",
                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            if (msbResult != MessageBoxResult.Yes) return;

            Models.Clear();
            _filename = "";
        }

        ICommand _OpenFileCommand;

        public ICommand OpenFileCommand => _OpenFileCommand ?? (_OpenFileCommand = new DelegateCommand<string>(OpenFileCommand_Execute));

        private void OpenFileCommand_Execute(string argFilename)
        {
            if (argFilename == "")
            {

                MessageBox.Show("You must enter a file name in the File Name textbox!", "Unable to save file",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                _filename = argFilename;
                var tempModels = new ObservableCollection<Model>();
                var tempPlanned = new ObservableCollection<Assignment>();
                var tempNew = new ObservableCollection<Assignment>();

                // Create an instance of the XmlSerializer class and specify the type of object to deserialize.
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Model>));
                try
                {
                    TextReader reader = new StreamReader(_filename);
                    // Deserialize all the Models.
                    tempModels = (ObservableCollection<Model>)serializer.Deserialize(reader);
                    tempPlanned = (ObservableCollection<Assignment>) serializer.Deserialize(reader);
                    tempNew = (ObservableCollection<Assignment>)serializer.Deserialize(reader);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Unable to open file", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                Models = tempModels;
                PlannedAssignments = tempPlanned;
                NewAssignments = tempNew;
            }
        }

        ICommand _closeAppCommand;
        public ICommand CloseAppCommand => _closeAppCommand ?? (_closeAppCommand = new DelegateCommand(() =>
                                                         {
                                                             App.Current.MainWindow.Close();
                                                         }));

        #endregion

        #region Dirty and filename

        private bool _dirty = false;
        public bool Dirty
        {
            get => _dirty;
            set
            {
                SetProperty(ref _dirty, value);
                RaisePropertyChanged("Title");
            }
        }

        public string FileName
        {
            get => _filename;
            set
            {
                SetProperty(ref _filename, value);
                RaisePropertyChanged("Title");
            }
        }

        public string Title
        {
            get
            {
                var s = "";
                if (_dirty)
                    s = "*";
                return FileName + s + " - " + AppTitle;
            }
        }

        #endregion

        #region DivCommands

        private ICommand _addAssignmentCommand;

        public ICommand AddAssignmentCommand
        {
            get
            {
                return _addAssignmentCommand ?? (_addAssignmentCommand = new DelegateCommand(() =>
                {
                    var newAssignment = new Assignment();
                    var viewModel = new AssignmentViewModel("Add New Assignment", newAssignment);
                    var view = new AssignmentView {DataContext = viewModel};
                    if (view.ShowDialog() != true) return;
                    NewAssignments.Add(newAssignment);
                    CurrentNewAssignment = newAssignment;
                    Dirty = true;
                }));
            }
        }

        private ICommand _assignModelCommand;

        public ICommand AssignModelCommand =>
            _assignModelCommand ?? (_assignModelCommand = new DelegateCommand(AssignModelCommand_Execute, AssignModelCommand_CanExecute).ObservesProperty(()=>CurrentModel).ObservesProperty(()=>CurrentNewAssignment));

        private void AssignModelCommand_Execute()
        {
            if (CurrentNewAssignment.AssignedModels.Contains(CurrentModel))
                return;

            CurrentNewAssignment.AssignedModels.Add(CurrentModel);
        }

        private bool AssignModelCommand_CanExecute()
        {
            return CurrentNewAssignment != null && CurrentModel != null;
        }

        private ICommand _planAssignmentCommand;
        public ICommand PlanAssignmentCommand => new DelegateCommand(PlanAssignment_Execute,PlanAssignment_CanExecute).ObservesProperty(() => CurrentNewAssignment);

        private void PlanAssignment_Execute()
        {
            PlannedAssignments.Add(CurrentNewAssignment);
            NewAssignments.Remove(CurrentNewAssignment);
        }

        private bool PlanAssignment_CanExecute()
        {
            return CurrentNewAssignment != null;
        }

        private ICommand _nextAssignmentCommand;
        public ICommand NextAssignmentCommand => new DelegateCommand(NextAssignmentCommand_Execute, NextAssignmentCommand_CanExecute).ObservesProperty(() => CurrentIndex);

        private void NextAssignmentCommand_Execute()
        {
            CurrentIndex++;
            CurrentNewAssignment = NewAssignments[CurrentIndex];
        }

        private bool NextAssignmentCommand_CanExecute()
        {
            return CurrentIndex < NewAssignments.Count-1;
        }

        private ICommand _previousAssignmentCommand;
        public ICommand PreviousAssignmentCommand => new DelegateCommand(PreviousAssignmentCommand_Execute, PreviousAssignmentCommand_CanExecute).ObservesProperty(() => CurrentIndex);

        private void PreviousAssignmentCommand_Execute()
        {
            CurrentIndex--;
            CurrentNewAssignment = NewAssignments[CurrentIndex];
        }

        private bool PreviousAssignmentCommand_CanExecute()
        {
            return CurrentIndex > 0;
        }

        #endregion
    }
}
