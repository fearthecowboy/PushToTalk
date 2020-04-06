using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PushToTalk
{
    public partial class MicState : Form
    {
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;
        public MicState()
        {
            InitializeComponent();
            off();
        }

        protected override void WndProc(ref Message message)
        {
            if (message.Msg == 163)
            {
                return;
            }
            base.WndProc(ref message);

            if (message.Msg == WM_NCHITTEST && (int)message.Result == HTCLIENT)
                message.Result = (IntPtr)HTCAPTION;
        }

        public void on()
        {
            this.BackgroundImage = global::PushToTalk.Properties.Resources.green;
        }

        public void off()
        {
            this.BackgroundImage = global::PushToTalk.Properties.Resources.red;
        }

       
    }
}
