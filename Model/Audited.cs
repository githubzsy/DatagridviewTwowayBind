using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Audited
    {
        /// <summary>
        /// 数据状态
        /// </summary>
        [Column(200,"状态")]
        public ModelState State { get; set; }
    }
}
