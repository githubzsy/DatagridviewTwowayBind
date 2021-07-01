using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public class DataBinder<T> where T: SourceBase
    {
        private DataGridView dgv;

        private BindingCollection<T> data;

        /// <summary>
        /// 属性与列特性的对应关系
        /// </summary>
        Dictionary<PropertyInfo, ColumnAttribute> dic = new Dictionary<PropertyInfo, ColumnAttribute>();

        public DataBinder(DataGridView dgv, BindingCollection<T> collection)
        {
            this.dgv = dgv;
            this.data = collection;
            foreach (var property in typeof(T).GetProperties())
            {
                var attributes= property.GetCustomAttributes(typeof(ColumnAttribute), true);
                if (attributes != null && attributes.Length > 0 && attributes[0] is ColumnAttribute attr)
                {
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
                
                dgv.Columns.Add(column);
            }
        }

        /// <summary>
        /// 删除所选行
        /// </summary>
        public void DeleteSelectedRows()
        {
            if (!typeof(SourceCanSelected).IsAssignableFrom(typeof(T)))
            {
                return;
            }

            var removeRows = new HashSet<T>();
            foreach (var item in data)
            {
                if((item as SourceCanSelected).RowSelected)
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
