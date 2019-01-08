using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace ConsoleApp1
{
    class Client
    {
        private TcpClient _tcpSocket;
        private IPAddress _enderecoIP;
        private bool Conectado;
        private string _resposta;
        NetworkStream stream = null;
        StreamReader reader = null;


        public void InicializaConexao()
        {
            if (!Conectado)
            {
                try
                {
                    // Trata o endereço IP informado em um objeto IPAdress
                    _enderecoIP = IPAddress.Parse("10.3.52.25");
                    // Inicia uma nova conexão TCP com o servidor chat
                    _tcpSocket = new TcpClient();
                    _tcpSocket.Connect(_enderecoIP, 5060);

                    // AJuda a verificar se estamos conectados ou não
                    Conectado = true;
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

        public void FechaConexao()
        {
            if (Conectado)
            {
                _tcpSocket.Close();

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

        public void EnviarMensagem(string mensagem)
        {
            if (Conectado)
            {
                byte[] msg = Encoding.UTF8.GetBytes(mensagem);
                stream = _tcpSocket.GetStream();
                try
                {
                    stream.Write(msg, 0, msg.Length);
                    ReceberMensagem();
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                    FechaConexao();
                }

            }
            else
            {
                Console.WriteLine("Cliente Desconectado, para enviar mensagens é necessário que o cliente esteja conectado!");
            }

        }

        public void ReceberMensagem()
        {
            StringBuilder sb = new StringBuilder();
            stream = _tcpSocket.GetStream();
            reader = new StreamReader(stream, Encoding.UTF8);

            while (reader.Peek() >= 0)
            {
                sb.Append((char)reader.Read());
                //Console.Write((char)reader.Read());
            }

            Console.WriteLine(sb);
            sb.Clear();

        }



    }


}
