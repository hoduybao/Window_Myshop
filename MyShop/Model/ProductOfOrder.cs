using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Model
{
    public class ProductOfOrder
    {
        public int STT { get; set; }
        public string TenSanPham { get; set; }
        public decimal DonGia { get; set; }
        public int SoLuong { get; set; }
        public decimal TongTien => DonGia * SoLuong;
    }
}
