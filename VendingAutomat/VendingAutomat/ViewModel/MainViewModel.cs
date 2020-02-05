using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using VendingAutomat.Model;

namespace VendingAutomat.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {


    private readonly IDataService _dataService;

        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string _welcomeTitle = string.Empty;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }
            set
            {
                Set(ref _welcomeTitle, value);
            }
        }

        public CashOwner User;
        public CashOwner VendingMachine;


    int userSumm;
        public int UserSumm
        {
            get
            {
                return userSumm;
            }
            set
            {
                if (userSumm == value)
                {
                    return;
                }

                userSumm = value;
                RaisePropertyChanged();
            }
        }
        ObservableCollection<MoneyViewModel> userWallet;
        public ObservableCollection<MoneyViewModel> UserWallet
        {
            get
            {
                return userWallet;
            }
            set
            {
                if (userWallet == value)
                {
                    return;
                }

                userWallet = value;
                RaisePropertyChanged();
            }
        }
        ObservableCollection<MoneyViewModel> automatWallet;
        public ObservableCollection<MoneyViewModel> AutomatWallet
        {
            get
            {
                return automatWallet;
            }
            set
            {
                if (automatWallet == value)
                {
                    return;
                }

                automatWallet = value;
                RaisePropertyChanged();

            }
        }
        int balance = 0;
        public int Balance
        {
            get
            {
                return balance;
            }
            set
            {
                balance = value;
                RaisePropertyChanged(() => Balance);
            }
        }
        ObservableCollection<ProductViewModel> productsInAutomat;
        public ObservableCollection<ProductViewModel> ProductsInAutomat
        {
            get
            {
                return productsInAutomat;
            }
            set
            {
                if (productsInAutomat == value)
                {
                    return;
                }

                productsInAutomat = value;
                RaisePropertyChanged();
            }
        }

        ObservableCollection<ProductViewModel> userBasket;
        public ObservableCollection<ProductViewModel> UserBasket
        {
            get
            {
                return userBasket;
            }
            set
            {
                if (userBasket == value)
                {
                    return;
                }

                userBasket = value;
                RaisePropertyChanged();
            }
        }

        public ICommand getChange;
        public ICommand GetChange
        {
            get
            {
                return getChange ?? (getChange = new RelayCommand(() =>
                {
                    using (VendingMachineEntities context = new VendingMachineEntities())
                    {
                        context.GetChange(VendingMachine.Id, User.Id);
                        UpdateFields(context);
                    }
                }));
            }
        }

        public void UpdateFields(VendingMachineEntities context)
        {
            UserSumm = (int)context.GetSumCashOwner(User.Id).ToList().First();
            Balance = (int)context.UserBalance.Where(x => x.cashOwnerId == User.Id).Select(y => y.balance).First();
            UserWallet = new ObservableCollection<MoneyViewModel>(context.GetWallet(User.Id).Select(p => new MoneyViewModel { IsButtonVisible = System.Windows.Visibility.Hidden, Name = p.name, Amount = p.amount, cashType = p.TypeCash}).ToList());
            AutomatWallet = new ObservableCollection<MoneyViewModel>(context.GetWallet(VendingMachine.Id).Select(p => new MoneyViewModel { IsButtonVisible = System.Windows.Visibility.Visible, Name = p.name, Amount = p.amount, cashType = p.TypeCash}).ToList());
            ProductsInAutomat = new ObservableCollection<ProductViewModel>(context.ProductRange.Select(x => new ProductViewModel { Name = x.name, Amount = x.amount, Price = x.price, Id = x.id }).ToList());
            UserBasket = new ObservableCollection<ProductViewModel>(context.ProductLatestSales().Select(x => new ProductViewModel { Name = x.name, Amount = x.amount, Price = x.price }).ToList());
        }


        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }

                    WelcomeTitle = item.Title;
                });
            using (VendingMachineEntities context = new VendingMachineEntities())
            {
                context.NewUserBasket();
                User = new CashOwner("User", context.CashOwner.Where(x => x.name == "User").Select(y => y.id).FirstOrDefault());
                VendingMachine = new CashOwner("VendingMachine", context.CashOwner.Where(x => x.name == "VendingMachine").Select(y => y.id).FirstOrDefault());
                UpdateFields(context);
            }
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}