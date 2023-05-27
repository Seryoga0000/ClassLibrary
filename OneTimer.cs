using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace ClassLibrary
{
    public class OneTimer:Timer
    {
        public OneTimer()
        {
            Tick += OneTimer_Tick;
        }
        public OneTimer(Action action, int pause):this()
        {
            Interval = pause;
            _action = action;
        }
        Action _action;
        private void OneTimer_Tick(object sender, EventArgs e)
        {
            _action?.Invoke();
            Stop();
        }

        public void Start(Action action,int pause)
        {
            Interval=pause;
            _action= action;
            Start();
        }
    }
}
