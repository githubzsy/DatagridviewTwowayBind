using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// dgv数据源
    /// </summary>
    public class SourceBase
    {
        [Column("Id",2)]
        [Key]
        public Guid Id { get; set; }
    }
}
