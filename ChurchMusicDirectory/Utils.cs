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
        public static string EscapeSpecialCharacters(string input)
        {
            string safeString = input;
            char[] specialChars = new char[] { '\'' };
            for (int specialCharIndex = 0; specialCharIndex < specialChars.Length; specialCharIndex++)
            {
                int nextSpecialChar = safeString.IndexOf(specialChars[specialCharIndex]);
                while (nextSpecialChar != -1)
                {
                    if (specialChars[specialCharIndex] == '\'')
                    {
                        safeString = safeString.Insert(nextSpecialChar, "'");
                    }
                    else
                    {
                        safeString = safeString.Insert(nextSpecialChar, @"\");
                    }
                    if (nextSpecialChar == safeString.Length - 2)
                    {
                        break;
                    }
                    nextSpecialChar = safeString.IndexOf(specialChars[specialCharIndex], nextSpecialChar + 2);
                }
            }
            return safeString;
        }
        public static DataGridView.HitTestInfo MousePositionInTable(DataGridView dataGridView, Point mousePosition)
        {
            Point clickLocationInTable = dataGridView.PointToClient(mousePosition);
            DataGridView.HitTestInfo cellInfo = dataGridView.HitTest(clickLocationInTable.X, clickLocationInTable.Y);
            return cellInfo;
        }
    }
}
