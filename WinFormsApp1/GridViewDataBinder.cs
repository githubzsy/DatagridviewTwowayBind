using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public class GridViewDataBinder<T> where T: Audited
    {
        private readonly DataGridView dgv;

        private readonly BindingCollection<T> data;

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
            dgv.Columns.Clear();
            AddColumns();
            dgv.CellBeginEdit += Dgv_CellBeginEdit;
            dgv.CellEndEdit += Dgv_CellEndEdit;
            dgv.CellClick += Dgv_CellClick;
            dgv.CellContentClick += Dgv_CellContentClick;
            dgv.DataBindingComplete += Dgv_DataBindingComplete;
            dgv.DataSource = data;
        }

        private void Dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            var dgv = (DataGridView)sender;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                foreach (var keyValue in imageColumnAdded)
                {
                    Image image = null;
                    try
                    {
                        image = Image.FromFile(row.Cells[keyValue.Key.Name].Value.ToString());
                    }
                    catch(Exception ex)
                    {

                    }
                    finally
                    {
                        if (row.Cells[keyValue.Value.Name] is DataGridViewImageCell cell)
                        {
                            cell.Value = image;
                            if (image != null && image.Height>120)
                            {
                                cell.OwningRow.Height = 120;
                                cell.ImageLayout = DataGridViewImageCellLayout.Zoom;
                            }
                            else
                            {
                                cell.ImageLayout = DataGridViewImageCellLayout.Normal;
                                cell.DataGridView.AutoResizeRow(cell.RowIndex);
                            }
                            
                        }
                    }
                }
            }
        }

        private void Dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var dgv = (DataGridView)sender;
            var cell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
            var keyValue = dic.SingleOrDefault(a => a.Key.Name == cell.OwningColumn.DataPropertyName);
            if (keyValue.Key==null) return;
            // 若点击的是链接
            if (keyValue.Value.ColumnType == ColumnType.Link)
            {
                System.Diagnostics.Process.Start("explorer.exe",cell.Value.ToString());
            }
        }

        /// <summary>
        /// 编辑之前的值
        /// </summary>
        object beginEditValue = null;
        private void Dgv_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var dgv = (DataGridView)sender;
            var cell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
            var keyValue = dic.Single(a => a.Key.Name == cell.OwningColumn.DataPropertyName);
            // 选择框状态改变不用处理
            if (keyValue.Value.IsRowCheckbox) return;
            // 记录开始编辑之前的值
            beginEditValue = cell.Value;
        }

        private void Dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var dgv = (DataGridView)sender;
            var cell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
            var keyValue = dic.Single(a => a.Key.Name == cell.OwningColumn.DataPropertyName);
            // 选择框状态改变不用处理
            if (keyValue.Value.IsRowCheckbox) return;
            
            if (dgv.Rows[e.RowIndex].DataBoundItem is T t)
            {
                if (t.State == ModelState.Unchanged && cell.Value != beginEditValue)
                {
                    t.State = ModelState.Modified;
                }
                data.ResetItem(e.RowIndex);
            }
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            DataGridView dgv = (DataGridView)sender;
            var cell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if(cell is DataGridViewCheckBoxCell c)
            {
                Console.WriteLine(c.Value);
                c.Value = !(bool)c.Value;
                data.ResetItem(e.RowIndex);
            }
        }

        Dictionary<DataGridViewColumn, DataGridViewColumn> imageColumnAdded = new Dictionary<DataGridViewColumn, DataGridViewColumn>();
        private void AddColumns()
        {
            foreach (var keyValue in dic)
            {
                DataGridViewColumn column;
                switch (keyValue.Value.ColumnType)
                {
                    case ColumnType.CheckBox:
                        column = new DataGridViewCheckBoxColumn();
                        column.ReadOnly = true;
                        break;
                    case ColumnType.ComboBox:
                        column = new DataGridViewComboBoxColumn();
                        break;
                    case ColumnType.Button:
                        column = new DataGridViewButtonColumn();
                        break;
                    case ColumnType.Image:
                        column = new DataGridViewTextBoxColumn();
                        var columnImage = new DataGridViewImageColumn();
                        columnImage.HeaderText = keyValue.Value.HeaderText;
                        columnImage.DataPropertyName = keyValue.Key.Name + "_Image";
                        dgv.Columns.Add(columnImage);
                        columnImage.Name = columnImage.DataPropertyName;
                        imageColumnAdded.Add(column,columnImage);
                        break;
                    case ColumnType.Link:
                        column = new DataGridViewLinkColumn();
                        break;
                    default:
                        column = new DataGridViewTextBoxColumn();
                        break;
                }

                if (keyValue.Key.PropertyType == typeof(ModelState))
                {
                    column.ReadOnly = true;
                }

                column.HeaderText = keyValue.Value.HeaderText;
                column.DataPropertyName = keyValue.Key.Name;
                column.Name = keyValue.Key.Name;
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
            // 是否存在软删除
            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                if ((bool)property.GetValue(item))
                {
                    // 新增状态的直接删除
                    if (item.State == ModelState.New)
                    {
                        removeRows.Add(item);
                    }
                    // 否则打上删除标记
                    else
                    {
                        item.State = ModelState.Deleted;
                        data.ResetItem(i);
                    }
                }
            }
            foreach (var item in removeRows)
            {
                data.Remove(item);
            }
        }

        public T AddNew()
        {
            var t = data.AddNew();
            t.State = ModelState.New;
            return t;
        }

        /// <summary>
        /// 提交修改
        /// </summary>
        /// <param name="action"></param>
        public void SubmitChanges(Func<BindingCollection<T>,bool> submitChanges)
        {
            if (!submitChanges(data)) return;
            var removeItems = new HashSet<T>();
            foreach (var item in data)
            {
                switch (item.State)
                {
                    case ModelState.Deleted:
                        removeItems.Add(item);
                        break;
                    default:
                        item.State = ModelState.Unchanged;
                        break;
                }
            }
            foreach (var item in removeItems)
            {
                data.Remove(item);
            }
            data.ResetBindings();
        }

    }
}
