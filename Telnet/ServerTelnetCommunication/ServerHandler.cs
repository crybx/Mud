﻿using Mud;
using Objects.Global;
using Objects.Mob.Interface;
using Shared.TagWrapper;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TelnetCommunication;
using TelnetCommunication.Interface;
using static Objects.Global.Logging.LogSettings;
using static Shared.TagWrapper.TagWrapper;

namespace ServerTelnetCommunication
{
    [ExcludeFromCodeCoverage]
    public class ServerHandler : TelnetHandler
    {
        private static ConcurrentDictionary<string, IPlayerCharacter> GuidToCharacter = new ConcurrentDictionary<string, IPlayerCharacter>();

        private LoginState _loginState { get; set; }
        private string _userName { get; set; }
        private string _password { get; set; }

        public string UserName
        {
            get
            {
                return _userName;
            }

            set
            {
                _userName = value;
            }
        }

        public ServerHandler(TcpClient clientSocket, IMudMessage mudMessage) : base(mudMessage)
        {
            _guid = Guid.NewGuid().ToString();
            _clientSocket = clientSocket;
            _loginState = LoginState.AsciiArt;
            Thread messageHandlerLoopThread = new Thread(MessageHandlerLoop);
            messageHandlerLoopThread.IsBackground = true;
            messageHandlerLoopThread.Start();

            Thread messageLoop = new Thread(GetMessageLoop);
            messageLoop.IsBackground = true;
            messageLoop.Name = "GetMessageLoop";
            messageLoop.Start();

            messageLoop = new Thread(SendMessageLoop);
            messageLoop.IsBackground = true;
            messageLoop.Name = "SendMessageLoop";
            messageLoop.Start();
        }

        private void MessageHandlerLoop()
        {
            bool continueToLoop = true;
            DateTime lastMessage = DateTime.Now;
            try
            {
                while (continueToLoop && _clientSocket.Connected)
                {
                    //Need to make the thread sleep
                    Thread.Sleep(10);

                    IPlayerCharacter pc = null;

                    TimeOutIdleConnection(lastMessage, pc);

                    #region Get Message From Client
                    //Handle message from client
                    try
                    {
                        string messageFromClient;
                        if (InQueue.TryDequeue(out messageFromClient))
                        {
                            switch (_loginState)
                            {
                                case LoginState.UserName:
                                    _userName = messageFromClient;
                                    _loginState = LoginState.Password;
                                    OutQueue.Enqueue(GlobalReference.GlobalValues.TagWrapper.WrapInTag("What is your password."));

                                    break;
                                case LoginState.Password:
                                    _password = messageFromClient;

                                    pc = GlobalReference.GlobalValues.World.LoadCharacter(_userName);
                                    if (pc == null)
                                    {
                                        GlobalReference.GlobalValues.Logger.Log(LogLevel.ALL, string.Format("{0} is an unknown user, offered to make a new one.", _userName, _password));
                                        _loginState = LoginState.CreateCharacter;
                                        OutQueue.Enqueue(GlobalReference.GlobalValues.TagWrapper.WrapInTag("Character not found.  Would you like to create the character?"));
                                    }
                                    else
                                    {
                                        if (pc.Password == _password)
                                        {
                                            GlobalReference.GlobalValues.Logger.Log(LogLevel.ALL, string.Format("{0} logged in successfully.", _userName));
                                            GlobalReference.GlobalValues.World.AddPlayerQueue.Enqueue(pc);
                                            GuidToCharacter.AddOrUpdate(_guid, pc, (k, v) => v = pc);
                                            _loginState = LoginState.LoggedIn;
                                        }
                                        else
                                        {
                                            GlobalReference.GlobalValues.Logger.Log(LogLevel.ALL, string.Format("{0} failed to log in with password {1}.", _userName, _password));
                                            _loginState = LoginState.AsciiArt;
                                            OutQueue.Enqueue(GlobalReference.GlobalValues.TagWrapper.WrapInTag("Invalid username/password."));
                                        }
                                    }
                                    break;
                                case LoginState.LoggedIn:
                                    //player character should be loaded
                                    GuidToCharacter.TryGetValue(_guid, out pc);
                                    if (pc != null)
                                    {
                                        pc.EnqueueCommand(messageFromClient);
                                        if (messageFromClient.ToUpper() == "LOGOUT")
                                        {
                                            continueToLoop = false;
                                        }
                                    }
                                    //not sure why we could not find the player character.  Relogin.
                                    else
                                    {
                                        _loginState = LoginState.AsciiArt;
                                    }
                                    break;
                                case LoginState.CreateCharacter:
                                    if (messageFromClient.Substring(0, 1).ToUpper() == "Y")
                                    {
                                        pc = GlobalReference.GlobalValues.World.CreateCharacter(_userName, _password);
                                        GuidToCharacter.AddOrUpdate(_guid, pc, (k, v) => v = pc);
                                        _loginState = LoginState.LoggedIn;
                                    }
                                    else if (messageFromClient.Substring(0, 1).ToUpper() == "N")
                                    {
                                        _loginState = LoginState.AsciiArt;
                                    }
                                    else
                                    {
                                        OutQueue.Enqueue(GlobalReference.GlobalValues.TagWrapper.WrapInTag("Character not found.  Would you like to create the character? Yes/No"));
                                    }
                                    break;
                            }

                            lastMessage = DateTime.Now;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException != null && ex.InnerException.Message != "A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond")
                        {
                            throw;
                        }
                    }
                    #endregion Get Message From Client

                    #region Send Message To Client
                    switch (_loginState)
                    {
                        case LoginState.AsciiArt:
                            OutQueue.Enqueue(GlobalReference.GlobalValues.TagWrapper.WrapInTag(GlobalReference.GlobalValues.Settings.AsciiArt, TagType.AsciiArt));

                            OutQueue.Enqueue(GlobalReference.GlobalValues.TagWrapper.WrapInTag("Welcome adventure. What is your name?"));
                            _loginState = LoginState.UserName;
                            break;
                    }


                    if (_guid != null)
                    {
                        pc = null;
                        GuidToCharacter.TryGetValue(_guid, out pc);
                        if (pc != null)
                        {
                            string messageToClient = pc.DequeueMessage();
                            if (messageToClient != null)
                            {
                                OutQueue.Enqueue(messageToClient);
                            }
                        }
                    }
                    #endregion Send Mesage To Client
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(" >> " + ex.ToString());
            }
        }


        private void TimeOutIdleConnection(DateTime lastMessage, IPlayerCharacter pc)
        {
            //if the player has not sent any command for 30 minutes log the player out
            if (DateTime.Now.Subtract(lastMessage).TotalMinutes > 30)
            {
                GuidToCharacter.TryGetValue(_guid, out pc);
                if (pc != null)
                {
                    pc.EnqueueCommand("exit");
                }
                throw new TimeoutException("No communication for 30 minutes");
            }
        }

        private enum LoginState
        {
            AsciiArt,
            UserName,
            Password,
            LoggedIn,
            CreateCharacter
        }
    }
}
