using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorControlApp
{
    class Door
    {

        public string ID;
        public string label;
        public bool status;
        public Door(string ID, string label, bool status)
        {
            this.ID = ID;
            this.label = label;
            this.status = status;
        }

    }


}
