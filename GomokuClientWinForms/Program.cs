using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Akka.Actor;
using Akka.Configuration;

namespace GomokuClient
{
    static class Program
    {
        public static ActorSystem GameActors;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //
            var config = ConfigurationFactory.ParseString(@"
                akka {
                    log-remote-lifecycle-events = off
                    
                    actor {
                        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
                        debug {
                                receive = on 
                                autoreceive = on
                                lifecycle = on
                                event-stream = on
                                unhandled = on
                        }
                    }

                    remote {
                        helios.tcp {
                            transport-class =
                                ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
                            transport-protocol = tcp
                            port = 0
                            hostname = localhost
                            tcp-keepalive = on
                        }

                        transport-failure-detector {
                            heartbeat-interval = 60 s # default 4s
                            acceptable-heartbeat-pause = 20 s # default 10s
                        }
                    }}");

            GameActors = ActorSystem.Create("ClientActorSystem", config);
            //

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
