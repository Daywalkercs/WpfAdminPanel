using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Data.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string CarModel { get; set; } = "";
        public string ShortDescription { get; set; } = "";
        public string LongDescription { get; set; } = "";
        public string Img { get; set; } = "";

        // Абсолютный путь к изображению
        public string FullImg
        {
            get
            {
                // Является абсолютной ссылкой (http/https/файл)
                if (Uri.TryCreate(Img, UriKind.Absolute, out var uri))
                {
                    if (uri.IsFile || uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
                        return Img;
                }

                // Иначе считаем, что путь относительный и дополняем его
                return $"https://localhost:44378{Img}";
            }
        }
        public ushort Price { get; set; }
        public bool IsFavourite { get; set; }
        public bool Available { get; set; }
        public int CategoryID { get; set; }

        //public virtual required Category Category { get; set; }
    }
}
