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
    public class ProductViewModel: ObservableObject
    {
        public Visibility IsButtonVisible { get; }

        public int Id { get; set; }
        public string Name { get; set; }

        public int? Price { get; set; }
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


        ICommand buyCommand;
        public ICommand BuyCommand
        {
            get
            {
                return buyCommand ?? (buyCommand = new RelayCommand<ProductViewModel>(obj =>
                {
                    using (VendingMachineEntities context = new VendingMachineEntities())
                    {
                        context.BuyProduct(2, obj.Id);
                        ((ViewModelLocator)App.Current.Resources["Locator"]).Main.UpdateFields(context);
                        //obj.Amount = context.GetWallet(1).Where(p => p.TypeCash == obj.cashType).Select(p => p.amount).First();
                    }
                }));
            }
        }
    }
}
