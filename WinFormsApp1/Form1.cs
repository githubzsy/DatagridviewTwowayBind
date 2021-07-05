using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        
        GridViewDataBinder<Student> dataBinder;

        private void Form1_Load(object sender, EventArgs e)
        {
            BindingCollection<Student> students = new BindingCollection<Student>() {
                new Student() { State = ModelState.Unchanged, HostPage="https://baidu.com",Image="d:\\1.png" }
            };
            dgvStudent.AllowUserToAddRows = false;
            dataBinder= dgvStudent.Bind(students);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            dataBinder.DeleteSelectedRows();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var student = dataBinder.AddNew();
            student.Id = Guid.NewGuid();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            dataBinder.SubmitChanges(data=> 
            {
                var addedCount = data.Count(a => a.State == ModelState.New);
                var modifiedCount = data.Count(a => a.State == ModelState.Modified);
                var deletedCount = data.Count(a => a.State == ModelState.Deleted);
                var dr = MessageBox.Show($"共新增{addedCount}条, 修改{modifiedCount}条, 删除{deletedCount}条, 确定提交吗?", "请确认", MessageBoxButtons.OKCancel);
                if(dr == DialogResult.OK)
                {
                    // TODO 提交到数据库

                    return true;
                }

                return false;
            });
        }
    }
}
