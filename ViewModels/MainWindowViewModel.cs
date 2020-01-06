using AT03___Model.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using Prism.Commands;

namespace AT03___Model.ViewModels
{
    public class MainWindowViewModel: BindableBase
    {
        #region Fields

        private string _filename = "";
        private readonly string AppTitle = "GUI-V-2020";
        private ObservableCollection<Model> _models;
        private Model _currentModel = null;
        private int _currentIndex = -1;

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

        #endregion

        public MainWindowViewModel()
        {
            _models = new ObservableCollection<Model>()
            {
                new Model("Jimmy", "555-1234", "Street 5", 170, 52, "brown", "Some comment..."),
                new Model("Joaquin", "555-3334", "Road 2 1st", 210, 95, "Bald", "Some Other comment...")
            };

            CurrentModel = null;
        }

        #region SaveDataToFile

        private ICommand _saveToFileCommand;

        public ICommand SaveToFileCommand
        {
            get
            {
                return _saveToFileCommand ?? (_saveToFileCommand =
                           new DelegateCommand(SaveToFileCommand_Execute, SaveToFileCommand_CanExecute)
                               .ObservesProperty(() => Models.Count));
            }
        }

        void SaveToFileCommand_Execute()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Model>));
            TextWriter streamWriter = new StreamWriter(_filename);
            serializer.Serialize(streamWriter, Models);
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

                // Create an instance of the XmlSerializer class and specify the type of object to deserialize.
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Model>));
                try
                {
                    TextReader reader = new StreamReader(_filename);
                    // Deserialize all the Models.
                    tempModels = (ObservableCollection<Model>)serializer.Deserialize(reader);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Unable to open file", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                Models = tempModels;
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
    }
}
