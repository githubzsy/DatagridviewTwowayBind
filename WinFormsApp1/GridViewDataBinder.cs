using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public class GridViewDataBinder<T> where T: class
    {
        private DataGridView dgv;

        private BindingCollection<T> data;

        /// <summary>
        /// 属性与列特性的对应关系
        /// </summary>
        Dictionary<PropertyInfo, ColumnAttribute> dic = new Dictionary<PropertyInfo, ColumnAttribute>();

        public GridViewDataBinder(DataGridView dgv, BindingCollection<T> collection)
        {
            this.dgv = dgv;
            this.data = collection;
            foreach (var property in typeof(T).GetProperties())
            {
                // 查询当前是否有Column特性
                if (property.GetCustomAttribute(typeof(ColumnAttribute), true) is ColumnAttribute attr)
                {
                    // 若特性上没有标题文字信息则继续查找Description特性
                    if (attr.HeaderText == null && property.GetCustomAttribute(typeof(DescriptionAttribute), true) is DescriptionAttribute attr2)
                    {
                        attr.HeaderText = attr2.Description;
                    }
                    dic.Add(property, attr);
                }
            }
            // 对其按照列排序排序
            dic = dic.OrderBy(a => a.Value.Order).ToDictionary(a => a.Key, a => a.Value);
            dgv.AutoGenerateColumns = false;
            dgv.DataSource = collection;
            AddColumns();
        }

        private void AddColumns()
        {
            foreach (var keyValue in dic)
            {
                DataGridViewColumn column;
                if (keyValue.Key.PropertyType==typeof(bool))
                {
                    column = new DataGridViewCheckBoxColumn();
                }
                else
                {
                    column = new DataGridViewTextBoxColumn();
                }
                column.HeaderText = keyValue.Value.HeaderText;
                column.DataPropertyName = keyValue.Key.Name;
                dgv.Columns.Clear();
                dgv.Columns.Add(column);
            }
        }

        /// <summary>
        /// 删除所选行
        /// </summary>
        public void DeleteSelectedRows()
        {
            var keyValue = dic.SingleOrDefault(a => a.Value.IsRowCheckbox == true);
            // 获取IsRowCheckbox标注的列
            var property = keyValue.Key;
            if (property == null)
            {
                return;
            }
            var removeRows = new HashSet<T>();
            foreach (var item in data)
            {
                if ((bool)property.GetValue(item))
                {
                    removeRows.Add(item);
                }
            }
            foreach (var item in removeRows)
            {
                data.Remove(item);
            }
        }

        public T AddRow()
        {
            return data.AddNew();
        }

    }
}
