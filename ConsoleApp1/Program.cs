using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    class Program
    {
        private static bool isclosing = false;

        static void Main(string[] args)
        {
            Client cliente = new Client();

            SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);


            while (true)
            {

                switch (Console.ReadLine())
                {
                    case "connect":
                        cliente.InicializaConexao();
                        break;
                    case "disconnect":
                        cliente.FechaConexao();
                        break;
                    case "send":
                        Console.WriteLine("Digite a mensagem");
                        string msg = Console.ReadLine();
                        cliente.EnviarMensagem(msg);
                        break;
                    default:
                        break;
                }
                
            }
            

        }

        [DllImport("Kernel32")]

        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

        public delegate bool HandlerRoutine(CtrlTypes CtrlType, Client cliente);

        public enum CtrlTypes

        {

            CTRL_C_EVENT = 0,

            CTRL_BREAK_EVENT,

            CTRL_CLOSE_EVENT,

            CTRL_LOGOFF_EVENT = 5,

            CTRL_SHUTDOWN_EVENT

        }

        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType, Client cliente)

        {

            // Put your own handler here

            switch (ctrlType)

            {

                case CtrlTypes.CTRL_C_EVENT:

                    isclosing = true;
                    cliente.FechaConexao();
                    Console.WriteLine("CTRL+C received!");

                    break;



                case CtrlTypes.CTRL_BREAK_EVENT:

                    isclosing = true;
                    cliente.FechaConexao();
                    Console.WriteLine("CTRL+BREAK received!");

                    break;



                case CtrlTypes.CTRL_CLOSE_EVENT:

                    isclosing = true;

                    cliente.FechaConexao();

                    Console.WriteLine("Program being closed!");

                    break;



                case CtrlTypes.CTRL_LOGOFF_EVENT:
                    cliente.FechaConexao();
                    break;

                case CtrlTypes.CTRL_SHUTDOWN_EVENT:

                    isclosing = true;
                    cliente.FechaConexao();
                    Console.WriteLine("User is logging off!");

                    break;



            }

            return true;

        }

    }
}
