using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Student : Audited
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
        [Column(1, ColumnType = ColumnType.CheckBox, IsRowCheckbox = true)]
        public bool RowSelected { get; set; }

        [Description("个人主页")]
        [Column(103, ColumnType = ColumnType.Link)]
        public string HostPage { get; set; }

        [Description("个人头像")]
        [Column(104, ColumnType = ColumnType.Image)]
        public string Image { get; set; }
    }
}
