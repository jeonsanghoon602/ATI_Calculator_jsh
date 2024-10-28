using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfCalculator1.Models;
using Wrapper;

namespace WpfCalculator1.ViewModels
{
    public class MainWindowViewModels : BaseViewModel
    {

        public string Oper { get; set; }
        public double Firstdata { get; set; }

        private string inputData = "";
        private string displayData = "";
        private ObservableCollection<ModelMemory> _modelMemories;
        private ObservableCollection<ModelHistory> _modelHistories;
        private ModelMemory _selectedmemory;
        private MyArithmetic arithmetic = new MyArithmetic();


        public ICommand DataOperator { protected get; set; }
        public ICommand DataInsert { protected get; set; }
        public ICommand DataBack { protected get; set; }
        public ICommand DataClear { protected get; set; }
        public ICommand DataAllClear { protected get; set; }
        public ICommand DataCalculator { protected get; set; }
        public ICommand DataExpendCalculator { protected get; set; }
        public ICommand SaveMemoryCommand { protected get; set; }
        public ICommand MemorySum { protected get; set; }
        public ICommand MemorySubtrac { protected get; set; }
        public ICommand MemoryClear { protected get; set; }
        public ICommand DataDotInsert { protected get; set; }

        public MainWindowViewModels()
        {
            _modelMemories = new ObservableCollection<ModelMemory>();
            _modelHistories = new ObservableCollection<ModelHistory>();

            this.DataInsert = new DataInsert(this);
            this.DataOperator = new DataOperator(this);
            this.DataBack = new DataBack(this);
            this.DataClear = new DataClear(this);
            this.DataCalculator = new DataCalculator(this);
            this.DataExpendCalculator = new DataExpendCalculator(this);
            this.DataAllClear = new DataAllClear(this);
            this.DataDotInsert = new DataDotInsert(this);
            SaveMemoryCommand = new RelayCommand<string>(SaveMemory);
            MemorySum = new RelayCommand<string>(SumMemory);
            MemorySubtrac = new RelayCommand<string>(SubtracMemory);
            MemoryClear = new RelayCommand<string>(ClearMemory);
        }
        public string InputData
        {
            internal set
            {
                if (inputData != value)
                {
                    inputData = value;
                    OnPropertyChanged(nameof(InputData));
                }
            }
            get
            {
                return inputData;
            }
        }

        public string DisplayData
        {
            internal set
            {
                if (displayData != value)
                {
                    displayData = value;
                    OnPropertyChanged(nameof(DisplayData));
                }
            }
            get
            {
                return displayData;
            }
        }

        public ObservableCollection<ModelMemory> Memory
        {
            get { return _modelMemories; }
            set
            {
                _modelMemories = value;
                OnPropertyChanged(nameof(Memory));
            }
        }
        public ObservableCollection<ModelHistory> Hisory
        {
            get { return _modelHistories; }
            set
            {
                _modelHistories = value;
                OnPropertyChanged(nameof(Hisory));
            }
        }

        public ModelMemory SelectedMemory
        {
            get
            {
                if (_selectedmemory != null)
                {
                    InputData = _selectedmemory.Memory;
                }
                return _selectedmemory;
            }
            set
            {
                _selectedmemory = value;
                OnPropertyChanged(nameof(SelectedMemory));
            }
        }

        public void AddHistory(string paramerter)
        {
            if (InputData != "")
            {
                _modelHistories.Add(new ModelHistory { History = paramerter });
            }
        }

        private void SaveMemory(string paramerter)
        {
            if (InputData != "")
            {
                _modelMemories.Add(new ModelMemory { Memory = InputData });
            }
        }

        private void SumMemory(string paramerter)
        {
            double result = 0;
            string str = "";
            double value = 0;
            double value2 = 0;

            if (InputData != "" && _modelMemories.Count > 0)
            {
                str = _modelMemories[0].Memory;

                value = double.Parse(str);
                value2 = double.Parse(InputData);
                result = arithmetic.Add(value, value2);
                _modelMemories.RemoveAt(0);
                _modelMemories.Insert(0, new ModelMemory { Memory = string.Format("{0:0.00#}",result) });
            }
        }

        private void SubtracMemory(string paramerter)
        {
            double result = 0;
            string str = "";
            double value = 0;
            double value2 = 0;

            if (InputData != "" && _modelMemories.Count > 0)
            {
                str = _modelMemories[0].Memory;
                value = double.Parse(str);
                value2 = double.Parse(InputData);
                result = arithmetic.Subtract(value, value2);
                _modelMemories.RemoveAt(0);
                _modelMemories.Insert(0, new ModelMemory { Memory = string.Format("{0:0.00#}", result) });
            }
        }
        private void ClearMemory(string paramerter)
        {
            _modelMemories.Clear();
        }

    }
}
