using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ProjectAutoBackup
{
    class ClassBackup
    {
        private static Timer timer;
        private static int timerPeriod;

        public ClassBackup()
        {

        }

        /// <summary>
        /// démarrage du timer dont la période est spécifiée en variable globale de classe
        /// </summary>
        private static void InitialisationTimer()
        {
            //instanciation
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);    //création d'un évènement sur le temps écoulé

            //valorisation des propriétés
            timer.Interval = (timerPeriod);
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();
        }

        /// <summary>
        /// méthode qui contient les évènements déclenchés lorsque le timer arrive à zéro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            Console.WriteLine("backup at {0:HH:mm:ss.fff}", e.SignalTime);
        }
    }
}
