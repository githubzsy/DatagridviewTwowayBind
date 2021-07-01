using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 能被选择行的数据源
    /// </summary>
    public class SourceCanSelected:SourceBase
    {
        [Column("选择",1,true)]
        public bool RowSelected { get; set; }
    }
}
