using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ProjectAutoBackup
{
    class ClassBackup
    {
        public static Timer timer;
        private static int timerPeriod = 1000*10;

        private static List<string> listPaths;
        private static string pathFolderDestCa = @"D:\BACKUP\";
        private static string pathFolderDestCb = "";

        /// <summary>
        /// démarrage du timer dont la période est spécifiée en variable globale de classe
        /// </summary>
        public static void InitialisationTimer()
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

            BackupFolders(currentDate);
            DeleteOlderDirs(pathFolderDestCa, currentDate);

            Console.WriteLine("backup at {0:HH:mm:ss.fff}", e.SignalTime);
        }

        /// <summary>
        /// sauvegarder l'ensemble des dossiers contenus dans la liste
        /// </summary>
        /// <param name="laDate"></param>
        private static void BackupFolders(DateTime laDate)
        {
            try
            {
                pathFolderDestCb = laDate.ToFileTime() + "\\";

                foreach (string path in listPaths)
                {
                    string pathFolderDestFinal = pathFolderDestCa + pathFolderDestCb;

                    DirectoryInfo newFolder = new DirectoryInfo(pathFolderDestFinal);
                    newFolder.Create();

                    bool copied = CopyDir(path, pathFolderDestFinal);

                    if (copied)
                    {
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        /// <summary>
        /// charger la liste des adresses complètes des dossiers à sauvegarder
        /// </summary>
        private static void InitListPaths()
        {
            listPaths = new List<string>();
            listPaths.Add(@"C:\TOTO");
        }

        /// <summary>
        /// copier un dossier et l'ensemble de tout ce qu'il contient (sous-dossiers & fichiers)
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="destDir"></param>
        /// <returns></returns>
        private static bool CopyDir(string sourceDir, string destDir)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDir); if (dir.Exists)
            {
                string realDestDir;
                if (dir.Root.Name != dir.Name)
                {
                    realDestDir = Path.Combine(destDir, dir.Name);
                    if (!Directory.Exists(realDestDir))
                        Directory.CreateDirectory(realDestDir);
                }
                else realDestDir = destDir;
                foreach (string d in Directory.GetDirectories(sourceDir))
                    CopyDir(d, realDestDir);
                foreach (string file in Directory.GetFiles(sourceDir))
                {
                    string fileNameDest = Path.Combine(realDestDir, Path.GetFileName(file));
                    //if (!File.Exists(fileNameDest))

                    File.Copy(file, fileNameDest, true);
                }
            }
            dir = new DirectoryInfo(destDir);
            if (dir.Exists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// parcours des dossiers et détermination de ceux à supprimer selon les conditions spécifiées
        /// </summary>
        /// <param name="path"></param>
        /// <param name="laDate"></param>
        private static void DeleteOlderDirs(string path, DateTime laDate)
        {
            if (System.IO.Directory.Exists(path))
            {
                DirectoryInfo mainDirectory = new DirectoryInfo(path);
                DirectoryInfo[] directories = mainDirectory.GetDirectories();

                for (int i = 0; i < directories.Length; i++)
                {
                    DirectoryInfo dirA = directories[i];

                    for (int j = i + 1; j < directories.Length; j++)
                    {
                        DirectoryInfo dirB = directories[j];

                        if (dirA.CreationTime.Day == dirB.CreationTime.Day)
                        {
                            DeleteDir(dirA.FullName);

                            dirA = dirB;

                            directories = mainDirectory.GetDirectories();
                        }
                    }
                }
                while (directories.Length > 10)
                {
                    DeleteDir(directories[0].FullName);

                    directories = mainDirectory.GetDirectories();
                }
            }
        }

        /// <summary>
        /// supprimer un dossier dont l'adresse est spécifiée en paramètre
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool DeleteDir(string path)
        {
            bool bdel = false;
            try
            {
                if (System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.Delete(path, true);
                    bdel = true;
                }
                else
                    bdel = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return bdel;
        }
    }
}
