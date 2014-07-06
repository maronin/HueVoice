using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueVoice
{
    class HueLightState
    {
        bool on;
        int brightness;
        int hue;
        int saturation;
        double xColorCord;
        double yColorCord;
        string alert;
        string effect;
        string colormode;
        bool reachable;

        public HueLightState(dynamic data)
        {
            on = data["state"].on;
            brightness = data["state"].bri;
            hue = data["state"].hue;
            saturation = data["state"].sat;
            xColorCord = data["state"].xy[0];
            yColorCord = data["state"].xy[1];
            alert = data["state"].alert;
            effect = data["state"].effect;
            colormode = data["state"].colormode;
            reachable = data["state"].reachable;
        }
    }


}
