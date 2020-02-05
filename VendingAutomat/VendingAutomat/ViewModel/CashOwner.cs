using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingAutomat.ViewModel
{
    public class CashOwner
    {
        public string Name { get; }
        public int Id { get; }

        public CashOwner(string name, int id)
        {
            Name = name;
            Id = id;
        }
    };
    
}
