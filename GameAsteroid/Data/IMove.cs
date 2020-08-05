using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAsteroid.Data
{
    /// <summary>
    /// Интерфейс описывает методы перемещения
    /// </summary>
    interface IMove
    {
        /// <summary>
        /// Перемещение вверх
        /// </summary>
        void Up();
        
        /// <summary>
        /// Перемещение вниз
        /// </summary>
        void Down();
        
        /// <summary>
        /// Перемещение влево
        /// </summary>
        void Left();

        /// <summary>
        /// Перемещение вправо
        /// </summary>
        void Right();

        /// <summary>
        /// Перемещение вслед за курсором мышки
        /// </summary>
        void MouseMove();
    }
}
