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
            int addWeeks = 0;
            double days = (DayOfWeek.Sunday - DateTime.Now.DayOfWeek) % 7;
            if (days < 0)
            {
                days = (DayOfWeek.Sunday - DateTime.Now.DayOfWeek) % 7 + 7;
            }
            dateTimePicker1.Value = DateTime.Now.AddDays(days + addWeeks * 7);
        }
    }
}
