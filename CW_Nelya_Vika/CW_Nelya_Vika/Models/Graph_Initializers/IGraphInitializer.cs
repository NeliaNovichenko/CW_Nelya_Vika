using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CW_Nelya_Vika.Models
{
    /// <summary>
    /// Задає поведінку для всіх класів, що можуть створити граф: 
    /// згенерувати, считати з файлу, відновити з бд.
    /// </summary>
    public interface IGraphInitializer
    {
        Graph Initialize();
      
    }
}
