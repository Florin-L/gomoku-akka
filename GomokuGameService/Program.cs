using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Akka.Actor;
using Akka.Configuration;

using Topshelf;
using Topshelf.Logging;


namespace Gomoku.Service
{
    /// <summary>
    /// The game service class.
    /// </summary>
    public class GameService
    {
        private readonly LogWriter _log = HostLogger.Get<GameService>();

        private ActorSystem _system;
        private IActorRef _serverActor;

        public void Start()
        {
            _log.Info("Starting Gomoku game service.");

            // creates the actors system
            var config = ConfigurationFactory.ParseString(@"
                akka {
                    log-remote-lifecycle-events = off
                    log-config-on-start = off
                    stdout-loglevel = INFO
                    loglevel = INFO

                    # this config section will be referenced as akka.actor
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

                    # here we're configuring the Akka.Remote module
                    remote {
                        helios.tcp {
                            transport-class =
                                ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
                            transport-protocol = tcp
                            port = 9000
                            hostname = localhost
                            tcp-keepalive = on
                        }

                        transport-failure-detector {
                            heartbeat-interval = 60 s # default 4s
                            acceptable-heartbeat-pause = 20 s # default 10s
                        }
                    }
                }");

            _system = ActorSystem.Create("GomokuActorSystem", config);

            // creates the game server actor
            _serverActor = _system.ActorOf<Gomoku.Actors.GameServerActor>("GameService");

            _log.Info("Gomoku game service has started.");
        }

        public void Stop()
        {
            _log.Info("Stopping Gomoku game service.");
            _system.Terminate();
            _log.Info("Gomoku game service halted.");
        }
    }

    /// <summary>
    /// Entry point class.
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(configure =>
            {
                configure.UseNLog();
                configure.UseAssemblyInfoForServiceInfo();

                configure.Service<GameService>(service =>
                {
                    service.ConstructUsing(s => new GameService());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });

                configure.RunAsLocalService();
                configure.SetServiceName("Gomoku Game Service");
                configure.SetDisplayName("Gomoku Game Service");
                configure.SetDescription("Gomoku Game Service");
            });
        }
    }
}
