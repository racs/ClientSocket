using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;


namespace ConsoleApp1
{
    class Client
    {
        private Socket _tcpSocket;
        private IPAddress _enderecoIP;
        private bool Conectado;
        private string _resposta;
        NetworkStream stream = null;
        StreamReader reader = null;
        private Thread mensagemThread;
        public const int BufferSize = 1024;
        public Byte[] buffer = new Byte[BufferSize];
        public StringBuilder sb = new StringBuilder();
        public string dadosRecebidos;



        public void InicializaConexao()
        {
            if (!Conectado)
            {
                try
                {
                    // Trata o endereço IP informado em um objeto IPAdress
                    _enderecoIP = IPAddress.Parse("10.3.52.25");
                    // Inicia uma nova conexão TCP com o servidor chat
                    _tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    _tcpSocket.Connect(_enderecoIP, 5060);

                    // AJuda a verificar se estamos conectados ou não
                    Conectado = true;
                    //mensagemThread = new Thread(new ThreadStart(ReceberMensagem));
                    //mensagemThread.Start();

                    _tcpSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro : " + ex.Message, "Erro na conexão com servidor");
                }

            }
            else
            {
                Console.WriteLine("Cliente já está conectado!");
            }
        }

        public void FecharConexao()
        {
            if (Conectado)
            {
                _tcpSocket.Disconnect(true);
                //_tcpSocket.Close();

                if (stream != null)
                {
                    stream.Close();
                }

                if (reader != null)
                {
                    reader.Close();
                }

                Conectado = false;

            }
            else
            {
                Console.WriteLine("Cliente já está desconectado!");
            }

        }

        public void Send(string data)
        {
            if (Conectado)
            {
                byte[] byteData = Encoding.UTF8.GetBytes(data);
                _tcpSocket.BeginSend(byteData, 0, byteData.Length,
                    0, new AsyncCallback(SendCallBack), _tcpSocket);
            }
            else
            {
                Console.WriteLine("Cliente está desconectado");
            }
            
        }

        private static void SendCallBack(IAsyncResult ar)
        {
            Socket handler = (Socket)ar.AsyncState;

            Console.ForegroundColor = ConsoleColor.Gray;
            //Console.WriteLine("Server Send: {0}", Server._sProtocolResponse);
            Console.WriteLine("Dados enviados");
            Console.ResetColor();

        }        


        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                if (Conectado)
                {
                    int size = _tcpSocket.EndReceive(ar);

                    // se o tamanho do buffer for maior que zero, escreve na tela os bytes recebidos
                    if (size > 0)
                    {
                        Array.Resize(ref this.buffer, size);
                        sb.Append(Encoding.UTF8.GetString(buffer, 0, size));
                        dadosRecebidos = sb.ToString();
                        sb.Clear();

                        Console.WriteLine($"{dadosRecebidos}");

                    }
                    else
                    {
                        //atualizar a lista do servidor retirando esse cliente da lista                        
                        //evento disparado sempre que um cliente disconecta

                        Console.WriteLine("Sem dados");


                    }

                }                
            }
            finally
            {
                if (Conectado)
                {
                    try
                    {
                        buffer = new Byte[4000];
                        _tcpSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }

            }





        }

    }
}
