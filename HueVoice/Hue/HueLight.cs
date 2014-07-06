using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HueVoice
{
    class HueLight
    {

        HueLightState state;
        string type;
        string name;
        string modelID;
        string swversion;

        public HueLight(dynamic data)
        {
            state = new HueLightState(data);
            type = data["type"];
            name = data["name"];
            modelID = data["modelid"];
            swversion = data["swversion"];
        }
    }
}
