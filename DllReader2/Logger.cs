using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DllReader2
{
    class Logger
    {
        #region Fields
        private TextBox logbox;
        #endregion

        #region Funcs
        public Logger(TextBox logBox)
        {
            this.logbox = logBox;
        }

        public void Log(string title, string msg)
        {
            if (title == null) title = "";
            if (title != "")
            {
                title = title.Insert(0, "[");
                title = title.Insert(title.Length - 1, "]");
            }

            if (msg == null) msg = "";
            if (msg != "")
            {
                {
                    msg = msg.Insert(0, "[");
                    msg = msg.Insert(msg.Length - 1, "]");
                }
            }

            logbox.Text += '[' + DateTime.Now.Date.ToString() + "]" + title + msg + "\n";
        }
        public void Log(List<Exception> excs)
        {
            for (int i = 0; i < excs.Count; i++)
                Log("", excs[i].Message);
        }
        public void Clear()
        {
            logbox.Clear();
        }
        #endregion

        #region Props
        #endregion
    }
}
