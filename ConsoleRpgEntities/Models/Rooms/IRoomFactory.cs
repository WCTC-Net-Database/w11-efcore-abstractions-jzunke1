using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRpgEntities.Models.Rooms
{
    public interface IRoomFactory
    {
        IRoom CreateRoom(string roomType);
    }

}
