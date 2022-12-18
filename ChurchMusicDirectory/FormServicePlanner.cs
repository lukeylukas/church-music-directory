using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChurchMusicDirectory
{
    public partial class FormServicePlanner : Form
    {
        public FormServicePlanner()
        {
            InitializeComponent();
            Setup();
        }
        private void Setup()
        {
            SelectDefaultServiceDate();
        }
        private void SelectDefaultServiceDate()
        {
            int addWeeks = 0;
            double days = (DayOfWeek.Sunday - DateTime.Now.DayOfWeek) % 7;
            if (days < 0)
            {
                days = (DayOfWeek.Sunday - DateTime.Now.DayOfWeek) % 7 + 7;
            }
            DateTime selectedDate = DateTime.Now.AddDays(days + addWeeks * 7);
            monthCalendarDatePicker.SelectionRange = new SelectionRange(selectedDate, selectedDate);
            SyncWithCalendar();
        }

        private void buttonCalendar_Click(object sender, EventArgs e)
        {
            monthCalendarDatePicker.Visible = !monthCalendarDatePicker.Visible;
        }

        private void monthCalendarDatePicker_DateSelected(object sender, DateRangeEventArgs e)
        {
            SyncWithCalendar();
            monthCalendarDatePicker.Visible = false;
        }
        private void SyncWithCalendar()
        {
            comboBoxServiceDate.Text = monthCalendarDatePicker.SelectionStart.ToLongDateString();
        }
    }
}
