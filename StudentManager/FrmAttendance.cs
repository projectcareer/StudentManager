using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Models;
using DAL;

namespace StudentManager
{
    public partial class FrmAttendance : Form
    {
        private AttendanceService attendanceService = new AttendanceService();
        private StudentService objStuService = new StudentService();
        public FrmAttendance()
        {
            InitializeComponent();
            //获取考勤学员总数
            string number = attendanceService.GetAllStudent().ToString(); //获取考勤学员总数
            this.lblCount.Text = number;
            ShowStat();
        } 
        
        //显示应出勤总数和签到总数
        private void ShowStat()
        {
            this.lblReal.Text = attendanceService.GetAttendStudents(DateTime.Now, true).ToString();
            this.lblAbsenceCount.Text = (Convert.ToInt32(this.lblCount.Text) - Convert.ToInt32(this.lblReal.Text)).ToString();
        }

        //显示当前时间
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.lblYear.Text = DateTime.Now.Year.ToString();
            this.lblMonth.Text = DateTime.Now.Month.ToString();
            this.lblDay.Text = DateTime.Now.Day.ToString();
            this.lblTime.Text = DateTime.Now.ToLongTimeString();
            switch (DateTime.Now.DayOfWeek.ToString())
            {
                case "Monday":
                    this.lblWeek.Text = "一";
                    break;
                case "Tuesday":
                    this.lblWeek.Text = "二";
                    break;
                case "Wednesday":
                    this.lblWeek.Text = "三";
                    break;
                case "Thursday":
                    this.lblWeek.Text = "四";
                    break;
                case "Friday":
                    this.lblWeek.Text = "五";
                    break;
                case "Saturady":
                    this.lblWeek.Text = "六";
                    break;
                case "Sunday":
                    this.lblWeek.Text = "日";
                    break;
            }

        }
        //学员打卡        
        private void txtStuCardNo_KeyDown(object sender, KeyEventArgs e)
        {
           if(this.txtStuCardNo.Text.Trim().Length == 0 || e.KeyValue != 13)
            {
                return;
            }
            //显示学员信息
            StudentExt objStudent = objStuService.GetStudentByCardNo(this.txtStuCardNo.Text.Trim());
            if(objStudent == null)
            {
                MessageBox.Show("卡号不正确！", "信息提示");
                this.lblInfo.Text = "打开失败";
                this.txtStuCardNo.SelectAll();
                this.lblStuName.Text = "";
                this.lblStuClass.Text = "";
                this.lblStuId.Text = "";
                this.pbStu.Image = null;
            }
            else
            {
                this.lblStuName.Text = objStudent.StudentName;
                this.lblStuClass.Text = objStudent.ClassName;
                this.lblStuId.Text = objStudent.StudentId.ToString();
                if(objStudent.StuImage != null && objStudent.StuImage.Length != 0)
                {
                    this.pbStu.Image = (Image)new SerializeObjectToString().DeserializeObject(objStudent.StuImage);
                }
                else
                {
                    this.pbStu.Image = Image.FromFile("default.png");
                }
                //添加打开信息
                string result = attendanceService.AddRecord(this.txtStuCardNo.Text.Trim());
                if(result != "success")
                {
                    this.lblInfo.Text = "打卡失败!";
                    MessageBox.Show(result, "信息提示");
                }
                else
                {
                    this.lblInfo.Text = "打卡成功！";
                    ShowStat();
                    this.txtStuCardNo.Text = "";
                    this.txtStuCardNo.Focus();
                }
            }
        }
        //结束打卡
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
     
    }
}
