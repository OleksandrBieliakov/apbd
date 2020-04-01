using System;
using System.Collections.Generic;
using System.Text;

namespace tuto2
{
    interface ISerializer
    {
        void Serialize(string toPath, University university);
    }
}
