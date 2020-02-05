using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VendingAutomat.Model;

namespace VendingAutomat.ViewModel
{
    public class MoneyViewModel : ObservableObject
    {
        public string Icon => "..\\Images\\coin.jpg";
        public Visibility IsButtonVisible { get; set; }
        public string Name { get; set; }
        int? amount;
        public int? Amount
        {
            get
            {
                return amount;
            }
            set
            {
                if (amount == value)
                {
                    return;
                }

                amount = value;
                RaisePropertyChanged();
            }
        }
        public int? cashType { get; set; }

        ICommand insertCashCommand;
        public ICommand InsertCashCommand
        {
            get
            {
                return insertCashCommand ?? (insertCashCommand = new RelayCommand<MoneyViewModel>(obj =>
                {
                    using (VendingMachineEntities context = new VendingMachineEntities())
                    {                        
                        context.ChangeCashAmount(((ViewModelLocator)App.Current.Resources["Locator"]).Main.User.Id,
                                                ((ViewModelLocator)App.Current.Resources["Locator"]).Main.VendingMachine.Id,
                                                obj.cashType);                        
                        ((ViewModelLocator)App.Current.Resources["Locator"]).Main.UpdateFields(context);
                       // obj.Amount = context.GetWallet(1).Where(p => p.TypeCash == obj.cashType).Select(p => p.amount).First();
                    }
                }));
            }
        }


    }
}
