using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using bai6.Model; //gọi khai báo CSDL

namespace bai6
{
    public partial class frmQLSV : Form
    {
    
        private TextBox txtStudentID;

        public frmQLSV()
        {
            InitializeComponent();
        }

        private void frmQLSV_Load(object sender, EventArgs e)
        {
            try
            {
                StudentContextDB context = new StudentContextDB();
                List<Faculty> listFalcultys = context.Faculties.ToList(); //lấy các khoa
                List<Student> listStudent = context.Students.ToList(); //lấy sinh viên
                FillFalcultyCombobox(listFalcultys);
                BindGrid(listStudent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BindGrid(List<Student> listStudent)
        {
            dgvStudent.Rows.Clear();
            foreach (var item in listStudent)
            { 
            
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells[0].Value = item.StudentID;
                dgvStudent.Rows[index].Cells[1].Value = item.FullName;
                dgvStudent.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dgvStudent.Rows[index].Cells[3].Value = item.AverageScore;
            }
        }



        //Hàm binding list có tên hiện thị là tên khoa, giá trị là Mã khoa
        private void FillFalcultyCombobox(List<Faculty> listFalcultys)
        {
            this.cmbFaculty.DataSource = listFalcultys;
            this.cmbFaculty.DisplayMember = "FacultyName";
            this.cmbFaculty.ValueMember = "FacultyID";
        }



       

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Create a new instance of the StudentContextDB class.
                StudentContextDB context = new StudentContextDB();

                // Get the list of students from the database.
                List<Student> studentLst = context.Students.ToList();

                // Check if the student ID already exists in the database.
                if (studentLst.Any(s => s.StudentID == txtMaSoSV.Text))
                {
                    // If the student ID already exists, show a warning message and return.
                    MessageBox.Show("Mã SV đã tồn tại. Vui lòng nhập một mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create a new student object.
                var newStudent = new Student
                {
                    // Set the properties of the new student object.
                    StudentID = txtMaSoSV.Text,
                    FullName = txtFullname.Text,
                    AverageScore = double.Parse(txtAverageScore.Text),
                    FacultyID = int.Parse(cmbFaculty.SelectedValue.ToString())
                };

                // Add the new student to the list of students.
                context.Students.Add(newStudent);

                // Save the changes to the database.
                context.SaveChanges();

                // Reload the data in the grid.
                BindGrid(context.Students.ToList());

                // Show a success message to the user.
                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // If an exception occurs, show an error message to the user.
                MessageBox.Show($"Lỗi khi thêm dữ liệu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                // Create a new instance of the database context
                StudentContextDB context = new StudentContextDB();

                // Get a list of all students from the database
                List<Student> studentList = context.Students.ToList();

                // Find the student with the specified ID
                var student = studentList.FirstOrDefault(s => s.StudentID == txtMaSoSV.Text);

                // If the student is found
                if (student != null)
                {
                    // Remove the student from the database
                    context.Students.Remove(student);

                    // Save the changes to the database
                    context.SaveChanges();

                    // Update the grid to reflect the changes
                    BindGrid(context.Students.ToList());

                    // Display a message box to confirm that the student was deleted successfully
                    MessageBox.Show("Sinh viên đã được xoá thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                // If the student is not found
                else
                {
                    // Display a message box indicating that the student was not found
                    MessageBox.Show("Sinh viên không tìm thấy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            // If an exception occurs
            catch (Exception ex)
            {
                // Display a message box showing the error message
                MessageBox.Show($"Lỗi khi cập nhật dữ liệu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                // Initialize database context and retrieve student list
                StudentContextDB context = new StudentContextDB();
                List<Student> students = context.Students.ToList();

                // Find student by ID
                var student = students.FirstOrDefault(s => s.StudentID == txtMaSoSV.Text);

                // Check if student exists
                if (student != null)
                {
                    // Check if student ID already exists
                    if (students.Any(s => s.StudentID == txtMaSoSV.Text && s.StudentID != student.StudentID))
                    {
                        MessageBox.Show("Mã SV đã tồn tại. Vui lòng nhập một mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Update student information
                    student.FullName = txtFullname.Text;
                    student.AverageScore = double.Parse(txtAverageScore.Text);
                    student.FacultyID = int.Parse(cmbFaculty.SelectedValue.ToString());

                    // Save changes to the database
                    context.SaveChanges();

                    // Reload data and display success message
                    BindGrid(context.Students.ToList());
                    MessageBox.Show("Chỉnh sửa thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Display error message if student is not found
                    MessageBox.Show("Sinh viên không tìm thấy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Display error message if exception occurs
                MessageBox.Show($"Lỗi khi cập nhật dữ liệu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvStudent_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dgvStudent.Rows[e.RowIndex];
                txtMaSoSV.Text = selectedRow.Cells[0].Value.ToString();
                txtFullname.Text = selectedRow.Cells[1].Value.ToString();
                cmbFaculty.Text = selectedRow.Cells[2].Value.ToString();
                txtAverageScore.Text = selectedRow.Cells[3].Value.ToString();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                
                this.Close();
            }
            
        }
    }
 }
