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
            students = new BindingCollection<Student>();
        }
        BindingCollection<Student> students;
        GridViewDataBinder<Student> dataBinder;

        private void Form1_Load(object sender, EventArgs e)
        {
            dgvStudent.AllowUserToAddRows = false;
            dataBinder= dgvStudent.Bind(students);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            dataBinder.DeleteSelectedRows();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var student = dataBinder.AddRow();
            student.Id = Guid.NewGuid();
        }
    }
}
