using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCalculator1.ViewModels;
using WpfCalculator1.Models;
using System.Windows;
using Wrapper;
using Microsoft.VisualBasic;

namespace WpfCalculator1
{
    class DataInsert : CommandBase
    {
        private MainWindowViewModels viewmodel;

        public DataInsert(MainWindowViewModels viewmodel)
        {
            this.viewmodel = viewmodel;
        }
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            //데이터 인풋

            if (viewmodel.DisplayData == "0" && viewmodel.DisplayData.Length < 2 && parameter.ToString() != ".")
            {
                viewmodel.InputData = viewmodel.DisplayData = string.Empty;
                viewmodel.InputData += parameter;
                viewmodel.DisplayData += parameter;
            }
            else
            {
                if (viewmodel.InputData == "0" && parameter.ToString() != ".")
                {
                    viewmodel.InputData = string.Empty;
                    viewmodel.InputData += parameter;
                }
                else
                {
                    viewmodel.InputData += parameter;
                    viewmodel.DisplayData += parameter;
                }
            }
        }
    }

    class DataDotInsert : CommandBase
    {
        private MainWindowViewModels viewmodel;

        public DataDotInsert(MainWindowViewModels viewmodel)
        {
            this.viewmodel = viewmodel;
        }
        public override bool CanExecute(object parameter)
        {
            bool result = viewmodel.InputData.Contains(".");
            return !result;
        }

        public override void Execute(object parameter)
        {
            //데이터 인풋
            

            viewmodel.InputData += parameter;
            viewmodel.DisplayData += parameter;
            
            
        }
    }

    class DataBack : CommandBase
    {
        private MainWindowViewModels viewmodel;

        public DataBack(MainWindowViewModels viewmodel)
        {
            this.viewmodel = viewmodel;
        }
        public override bool CanExecute(object parameter)
        {
            return viewmodel.DisplayData.Length > 0;
        }

        public override void Execute(object parameter)
        {
            int Len = viewmodel.InputData.Length - 1;
            int DisplayLen = viewmodel.DisplayData.Length - 1;
            if (0 < Len)
            {
                viewmodel.InputData = viewmodel.InputData.Substring(0, Len);
                viewmodel.DisplayData = viewmodel.DisplayData.Substring(0, DisplayLen);
            }
            else
            {
                viewmodel.DisplayData = string.Format("{0}{1}", viewmodel.Firstdata, viewmodel.Oper);
                viewmodel.InputData = "0";
            }
            //데이터 지우기
        }
    }

    class DataClear : CommandBase
    {
        private MainWindowViewModels viewmodel;

        public DataClear(MainWindowViewModels viewmodel)
        {
            this.viewmodel = viewmodel;
        }
        public override bool CanExecute(object parameter)
        {
            return viewmodel.InputData.Length > 0;
        }

        public override void Execute(object parameter)
        {
            viewmodel.InputData = "0";
            viewmodel.Firstdata = 0;
            //데이터 클리어
        }
    }

    class DataAllClear : CommandBase
    {
        private MainWindowViewModels viewmodel;

        public DataAllClear(MainWindowViewModels viewmodel)
        {
            this.viewmodel = viewmodel;
        }
        public override bool CanExecute(object parameter)
        {
            return viewmodel.DisplayData.Length > 0;
        }

        public override void Execute(object parameter)
        {
            viewmodel.InputData = viewmodel.DisplayData = "0";
            viewmodel.Oper = "";
            viewmodel.Firstdata = 0;
            //데이터 클리어
        }
    }

    class DataOperator : CommandBase
    {
        private MainWindowViewModels viewmodel;

        public DataOperator(MainWindowViewModels viewmodel)
        {
            this.viewmodel = viewmodel;
        }
        public override bool CanExecute(object parameter)
        {
            return 0 < viewmodel.InputData.Length;
        }

        public override void Execute(object parameter)
        {
            string operate = parameter.ToString();
            double pfirstdata;

            if (double.TryParse(viewmodel.InputData, out pfirstdata))
            {
                viewmodel.Oper = operate;
                viewmodel.Firstdata = pfirstdata;
                viewmodel.InputData = "";
                viewmodel.DisplayData += operate;
            }
            else if (viewmodel.InputData == "" && operate == "-")
            {
                viewmodel.DisplayData += viewmodel.InputData = "-";
            }
            //데이터 연산자
        }
    }

    class DataCalculator : CommandBase
    {
        private MainWindowViewModels viewmodel;
        private MyArithmetic arithmetic = new MyArithmetic();
        public int Data
        {
            get; set;
        }

        public DataCalculator(int data)
        {
            this.Data = data;
        }

        public DataCalculator(MainWindowViewModels viewmodel)
        {
            this.viewmodel = viewmodel;
        }

        public override bool CanExecute(object parameter)
        {
            double result;
            return viewmodel.Oper != null && double.TryParse(viewmodel.InputData, out result);
        }


        public override void Execute(object parameter)
        {
            List<double> datalist = new List<double>();
            List<char> opreatelist = new List<char>();
            double result = 0;
            string _history = "";

            GetCalclurateList(datalist, opreatelist);

            for (int j = opreatelist.Count() - 1; j >= 0; j--)
            {
                if (opreatelist[j] == '/')
                {
                    datalist[j] = MyDivde(datalist[j], datalist[j + 1]);
                    datalist[j + 1] = 0;
                    opreatelist[j] = '+';

                }
                else if (opreatelist[j] == '*')
                {
                    datalist[j] = Mymultiply(datalist[j], datalist[j + 1]);
                    datalist[j + 1] = 0;
                    opreatelist[j] = '+';
                }
                result = datalist[j];
            }

            for (int j = 0; j < opreatelist.Count(); j++)
            {
                if (opreatelist[j] == '+')
                {
                    datalist[j + 1] = Myadd(datalist[j], datalist[j + 1]);
                }
                else if (opreatelist[j] == '-')
                {
                    datalist[j + 1] = Mysubtract(datalist[j], datalist[j + 1]);
                }
                result = datalist[j + 1];
            }

            if (viewmodel.Oper == "")
            {
                result = double.Parse(viewmodel.InputData);
            }

            _history = string.Format("{0}{1}{2}", viewmodel.DisplayData, "=", result);
            viewmodel.InputData = viewmodel.DisplayData = string.Format("{0}",result);
            viewmodel.AddHistory(_history);
            viewmodel.Firstdata = 0;
     
        //데이터 출력
    }

        private double Myadd(double _int1, double _int2)
        {
            double value = arithmetic.Add(_int1, _int2);
            return (double)value;
        }
        private double Mysubtract(double _int1, double _int2)
        {
            double value = arithmetic.Subtract(_int1, _int2);
            return (double)value;
        }
        private double Mymultiply(double _int1, double _int2)
        {
            double value = arithmetic.Multiply(_int1, _int2);
            return (double)value;
        }
        private double MyDivde(double _int1, double _int2)
        {
            double value = arithmetic.Divide(_int1, _int2);
            return (double)value;
        }


        public void GetCalclurateList(List<double> dlist, List<char> olist)
        {
            string splitpoint = "";
            string palldata = viewmodel.DisplayData;

            for (int i = 0; i < palldata.Length; i++)
            {
                if (palldata[i] == '*' || palldata[i] == '-' || palldata[i] == '/' || palldata[i] == '+')
                {
                    olist.Add(palldata[i]);
                    dlist.Add(double.Parse(splitpoint));
                    splitpoint = "";
                }
                else
                {
                    splitpoint += palldata[i];
                }
            }

            dlist.Add(double.Parse(splitpoint));
        }
    }

    class DataExpendCalculator : CommandBase
    {
        private MainWindowViewModels viewmodel;
        private MyArithmetic arithmetic = new MyArithmetic();
        public int Data
        {
            get; set;
        }

        public DataExpendCalculator(int data)
        {
            this.Data = data;
        }

        public DataExpendCalculator(MainWindowViewModels viewmodel)
        {
            this.viewmodel = viewmodel;
        }

        public override bool CanExecute(object parameter)
        {
            double result;
            return double.TryParse(viewmodel.InputData, out result);
        }


        public override void Execute(object parameter)
        {
            string _history = "";
            string Expend = parameter.ToString();
            double result = 0;
            double pfirstdata = 0;
            double penddata = 0;

            if (double.TryParse(viewmodel.InputData, out pfirstdata))
            {
                penddata = viewmodel.Firstdata;

                if (Expend.Contains("Exponent"))
                {
                    result = MyExponent(pfirstdata);

                }
                else if (Expend.Contains("Square"))
                {
                    result = MySquare(pfirstdata);
                }
                else if (Expend.Contains("Root"))
                {
                    result = MyRoot(pfirstdata);
                }
                else if (Expend.Contains("Percent"))
                {
                    result = MyPercent(pfirstdata, penddata);
                }
                else if (Expend.Contains("Negative"))
                {
                    result = MyNegative(pfirstdata);
                }
            }
            else if (viewmodel.InputData == "")
            {
                viewmodel.DisplayData += "";
            }


            if (Expend.Contains("Percent"))
            {
                _history = string.Format("{0}({1},{2})={3}", Expend, viewmodel.Firstdata, viewmodel.InputData, result);
                viewmodel.DisplayData = string.Format("{0}{1}{2}", viewmodel.Firstdata, viewmodel.Oper, result); ;
                viewmodel.AddHistory(_history);
            }
            else
            {
                _history = string.Format("{0}({1}){2}{3}", Expend, viewmodel.InputData, "=", result);
                viewmodel.DisplayData = "0";
                viewmodel.AddHistory(_history);
            }
            viewmodel.InputData = string.Format("{0}", result);
            viewmodel.Firstdata = 0;
            //데이터 출력
        }

        private double MyExponent(double _int1)
        {
            double value = arithmetic.Exponent(_int1);
            return (double)value;
        }
        private double MyRoot(double _int1)
        {
            double value = arithmetic.Root(_int1);
            return (double)value;
        }
        private double MyPercent(double _int1, double _int2)
        {
            double value = arithmetic.Percent(_int1, _int2);
            return (double)value;
        }

        private double MySquare(double _int1)
        {
            double value = arithmetic.Square(_int1);
            return (double)value;
        }
        private double MyNegative(double _int1)
        {
            double value = arithmetic.Multiply(_int1, -1);
            return (double)value;
        }

    }
}
