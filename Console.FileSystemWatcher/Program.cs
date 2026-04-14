//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Lifeprojects.de">
//     Class: Program
//     Copyright © Lifeprojects.de 2026
// </copyright>
// <Template>
// 	Version 3.0.2026.1, 08.1.2026
// </Template>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>03.03.2026 14:26:39</date>
//
// <summary>
// Konsolen Applikation mit Menü
// </summary>
//-----------------------------------------------------------------------

namespace Console.FileSystemWatcher
{
    /* Imports from NET Framework */
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Loader;
    using System.Windows;

    public class Program
    {
        private static BufferedFileSystemWatcher bfsw;
        private static string path = @"c:\_DownLoads\";

        public Program()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
        }

        private static void Main(string[] args)
        {
            CMenu mainMenu = BuildMenu();
            mainMenu.Show();
        }

        private static void ApplicationExit()
        {
            Environment.Exit(0);
        }

        private static CMenu BuildMenu()
        {
            Console.Clear();
            CMenu mainMenu = new CMenu("BufferedFileSystemWatcher");
            mainMenu.AddItem("Überwachen von Änderungen im Verzeichnis", MenuPoint1);
            mainMenu.AddItem("Stop", MenuPoint2);
            mainMenu.AddItem("Beenden", () => ApplicationExit());
            return mainMenu;
        }

        private static void MenuPoint1()
        {
            Console.Clear();

            bfsw = new BufferedFileSystemWatcher(path);
            WeakEventManager<BufferedFileSystemWatcher, FileSystemEventArgs>.AddHandler(bfsw, "Created", OnCreatedOrDelete);
            WeakEventManager<BufferedFileSystemWatcher, FileSystemEventArgs>.AddHandler(bfsw, "Deleted", OnCreatedOrDelete);
            WeakEventManager<BufferedFileSystemWatcher, FileSystemEventArgs>.AddHandler(bfsw, "Changed", OnCreatedOrDelete);
            WeakEventManager<BufferedFileSystemWatcher, FileSystemEventArgs>.AddHandler(bfsw, "Renamed", OnCreatedOrDelete);
            bfsw.SetBufferedChangeTypes(BufferedChangeTypes.Created | BufferedChangeTypes.Deleted);
            bfsw.StartFileSystemWatcher();

            Console.WriteLine($"Überwache Änderungen im Verzeichnis '{path}' gestartet; ({bfsw.GetFileSystemWatcherInfo()})");

            Console.Wait();
        }

        private static void MenuPoint2()
        {
            Console.Clear();

            bfsw.StopFileSystemWatcher();

            Console.WriteLine($"Überwache Änderungen im Verzeichnis '{path}' gestoppt; ({bfsw.GetFileSystemWatcherInfo()})");

            CMenu.Wait();
        }

        private static void OnCreatedOrDelete(object sender, FileSystemEventArgs e)
        {
            try
            {
                var file = Path.Combine(path, e.Name);
                var changeType = e.ChangeType;
                Console.WriteLine($"Datei '{file}' (ChangeType: {changeType})");
            }
            catch (Exception ex)
            {
                string errorText = $"Fehler: {ex.Message}";
            }
        }
    }
}
