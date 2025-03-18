using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace WpfAdminPanel.Models
{
    public class Product
    {
        private int _id;
        public int Id
        {
            get => _id; 
            set => _id = value;
        }

        private string _carModel = "";
        public string CarModel
        {
            get { return _carModel; }
            set { _carModel = value; }
        }

        private string _shortDescription = "";
        public string ShortDescription
        {
            get => _shortDescription; 
            set => _shortDescription = value; 
        }

        private string _longDescription = "";
        public string LongDescription
        {
            get => _longDescription;
            set => _longDescription = value;
        }

        private string _img = "";
        public string Img
        {
            get => _img;
            set => _img = value;
        }

        private decimal _price;
        public decimal Price
        {
            get => _price; 
            set => _price = value;
        }


        //private bool _isFavourite;


        //private bool _available;


        //private int _categoryId;

      
        //public virtual required Category Category { get; set; }
    }
}
