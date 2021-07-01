using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [Source]
    public class Student : SourceCanSelected
    {

        [Column("姓名",101)]
        public string Name { get; set; }

        [Column("年龄",102)]
        public byte Age { get; set; }

    }
}
