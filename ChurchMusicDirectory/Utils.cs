using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChurchMusicDirectory
{
    internal static class Utils
    {
        public static DataGridView.HitTestInfo MousePositionInTable(DataGridView dataGridView, Point mousePosition)
        {
            Point clickLocationInTable = dataGridView.PointToClient(mousePosition);
            DataGridView.HitTestInfo cellInfo = dataGridView.HitTest(clickLocationInTable.X, clickLocationInTable.Y);
            return cellInfo;
        }
    }
}
