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
    public class ColumnAttribute:Attribute
    {
        /// <summary>
        /// 列文字
        /// </summary>
        public string HeaderText { get; set; }

        /// <summary>
        /// 当前列是否为行选择Checkbox
        /// </summary>
        public bool IsRowCheckbox { get; set; }

        /// <summary>
        /// 列的顺序(越小越靠前)
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerText">当前列文字</param>
        /// <param name="isRowChecked">当前列是否为行选择Checkbox(默认为否)</param>
        public ColumnAttribute(string headerText, int order = 101, bool isRowChecked = false)
        {
            this.HeaderText = headerText;
            IsRowCheckbox = isRowChecked;
            Order = order;
        }

    }
}
