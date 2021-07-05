using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 标注当前列的显示文字
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        /// <summary>
        /// 当前列是否为行选择Checkbox
        /// </summary>
        public bool IsRowCheckbox { get; set; }

        /// <summary>
        /// 列的顺序(越小越靠前)
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 标题文字
        /// </summary>
        public string HeaderText { get; set; }

        /// <summary>
        /// 列类型
        /// </summary>
        public ColumnType ColumnType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerText">当前列文字</param>
        /// <param name="isRowChecked">当前列是否为行选择Checkbox(默认为否)</param>
        /// <param name="columnType">当前列类型(默认为Textbox)</param>
        /// <param name="order">当前列顺序(越小越靠前)</param>
        public ColumnAttribute(int order = 101, string headerText = null, ColumnType columnType = ColumnType.TextBox, bool isRowChecked = false)
        {
            IsRowCheckbox = isRowChecked;
            Order = order;
            HeaderText = headerText;
            ColumnType = columnType;
        }

    }
}
