using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Student
    {
        [Description("姓名")]
        [Column(101)]
        public string Name { get; set; }

        [Description("年龄")]
        [Column(102)]
        public byte Age { get; set; }

        [Key]
        public Guid Id { get; set; }

        [Description("选择")]
        [Column(1, true)]
        public bool RowSelected { get; set; }
    }
}
