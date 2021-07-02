using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public static class GridViewDataBindHelper
    {
        public static GridViewDataBinder<T> Bind<T>(this DataGridView dataGridView,BindingCollection<T> data) where T:class
        {
            return new GridViewDataBinder<T>(dataGridView,data);
        } 
    }
}
