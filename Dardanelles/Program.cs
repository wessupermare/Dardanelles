using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dardanelles
{
    class Program
    {
        static void Main(string[] args) { new Program(); }

        private readonly Config Config;
        public Program()
        {
            Config = new Config("data.json");
            if (!Config.ContainsKey("SetupComplete"))
                Setup();
            else if (!(bool)Config["SetupComplete"])
            {
                Config.DeleteAll();
                Config = new Config("data.json");
                Setup();
            }

            Config.DeleteAll();
        }

        /// <summary>
        /// Sets up a new config file with student info.
        /// </summary>
        private void Setup()
        {
            Config["SetupComplete"] = false;

            Console.Write("Setting up project Dardanelles. One moment please ");

            AutoResetEvent mres = new AutoResetEvent(false);
            Task.Run(() =>
            {
                SetupSpinner(mres);
            });



            mres.Set();

            Config["SetupComplete"] = true;
        }

        /// <summary>
        /// Spins a bar around on-screen.
        /// </summary>
        private void SetupSpinner(AutoResetEvent e)
        {
            bool flag = false;
            Task.Run(() =>
            {
                char c = '|';
                while (!flag)
                {
                    Console.Write($"{c}\b");
                    switch (c)
                    {
                        case '|': c = '/'; break;
                        case '/': c = '-'; break;
                        case '-': c = '\\'; break;
                        case '\\': c = '|'; break;
                    }
                    Thread.Sleep(100);
                }
            });

            e.WaitOne();
            flag = true;
        }
    }
}