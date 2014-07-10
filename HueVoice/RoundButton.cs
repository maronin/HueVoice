using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace HueVoice
{
    class RoundButton : Button
    {
        protected override void OnResize(EventArgs e)
        {
            using (var path = new GraphicsPath())
            {
                path.AddEllipse(new Rectangle(10, 10, this.Width - 20, this.Height - 20));
                this.Region = new Region(path);
            }
            base.OnResize(e);
        }
    }
}
