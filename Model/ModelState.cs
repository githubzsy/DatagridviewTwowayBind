using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 枚举数据状态
    /// </summary>
    public enum ModelState
    {
        /// <summary>
        /// 不变
        /// </summary>
        Unchanged,
        /// <summary>
        /// 新增
        /// </summary>
        New,
        /// <summary>
        /// 修改
        /// </summary>
        Modified,
        /// <summary>
        /// 删除
        /// </summary>
        Deleted
    }
}
