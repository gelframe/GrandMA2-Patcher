using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GelFrame.Telnet
{
    class Connection : IDisposable
    {
        // Define object vars 
        private readonly TcpClient tcpClient;
        private readonly NetworkStream networkStream;
        private Byte[] command = Encoding.ASCII.GetBytes("\n");
        public bool activeConnection = false;

        /// <summary>
        /// Setup Grandma connection and login
        /// </summary>
        public Connection()
        {
            try
            {
                // Establish connection
                tcpClient = new TcpClient(Settings.Data.GetValue(Config.Settings.maConsoleIp), Config.Telnet.consolePort);
                networkStream = tcpClient.GetStream();
                networkStream.ReadTimeout = GelFrame.Functions.Numbers.StringToPostiveInt(Settings.Data.GetValue(Config.Settings.maTimeOut));

                // Set active connection to true
                activeConnection = true;

                // Login
                SendCommands(new List<string>() { Config.Telnet.commandLogin });
            }
            catch
            {
                MessageBox.Show(Config.ErrorMessages.telnetConnection);
            }
        }

        /// <summary>
        /// Runs commands in order from given list
        /// </summary>
        /// <param name="commands">List of strings containing commands to run</param>
        public void SendCommands(List<string> commands)
        {
            // Only send commands if connection has been established
            if (activeConnection)
            {
                // Loop through commands
                foreach (string inputCommand in commands)
                {
                    try
                    {
                        // Execute commands
                        Thread.Sleep(GelFrame.Functions.Numbers.StringToPostiveInt(Settings.Data.GetValue(Config.Settings.maCommandDelay)));
                        command = Encoding.ASCII.GetBytes(inputCommand + "\r");
                        networkStream.Write(command, 0, command.Length);
                    }
                    catch
                    {
                        activeConnection = false;
                        MessageBox.Show(Config.ErrorMessages.telnetConnection);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Return the network stream
        /// </summary>
        /// <returns>Network stream</returns>
        public NetworkStream Stream()
        {
            return networkStream;
        }

        /// <summary>
        /// Close connection on disposal
        /// </summary>
        public void Dispose()
        {
            if (activeConnection)
                tcpClient.Close();
        }
    }
}
