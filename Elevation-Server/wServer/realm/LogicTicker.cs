#region

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using log4net;
using wServer.realm.entities.player;
using System.Collections.Generic;

#endregion

namespace wServer.realm
{
    public class LogicTicker
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (LogicTicker));
        public static RealmTime CurrentTime;
        private readonly ConcurrentQueue<Action<RealmTime>>[] pendings;

        public int TPS;

        public LogicTicker(RealmManager manager)
        {
            Manager = manager;
            pendings = new ConcurrentQueue<Action<RealmTime>>[5];
            for (var i = 0; i < 5; i++)
                pendings[i] = new ConcurrentQueue<Action<RealmTime>>();

            TPS = manager.TPS;
        }

        public RealmManager Manager { get; private set; }

        public void AddPendingAction(Action<RealmTime> callback) => AddPendingAction(callback, PendingPriority.Normal);

        public void AddPendingAction(Action<RealmTime> callback, PendingPriority priority) => pendings[(int)priority].Enqueue(callback);

        public void TickLoop()
        {
            log.Info("Logic loop started.");

            var sw = Stopwatch.StartNew();

            long elapsed = 0;

            do
            {
                if (Manager.Terminating)
                    break;

                var ems = sw.ElapsedMilliseconds;
                var dt = (int)(ems - elapsed);

                var t = new RealmTime();
                t.thisTickTimes = dt;

                foreach (var i in pendings)
                {
                    while (i.TryDequeue(out var callback))
                    {
                        try
                        {
                            callback(t);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                        }
                    }
                }
                TickWorlds1(t);

                var tradingPlayers = TradeManager.TradingPlayers.Where(_ => _.Owner == null).ToArray();
                foreach (var player in tradingPlayers)
                    TradeManager.TradingPlayers.Remove(player);

                var requestPlayers = TradeManager.CurrentRequests.Where(_ => _.Key.Owner == null || _.Value.Owner == null).ToArray();
                foreach (var players in requestPlayers)
                    TradeManager.CurrentRequests.Remove(players);
                elapsed = ems;
                var esmr = (int)(sw.ElapsedMilliseconds - ems);
                var sleep = (1000 / TPS) - esmr;
                Thread.Sleep(sleep < 0 ? 0 : sleep);
            }
            while (true);
            sw.Stop();

            //var watch = new Stopwatch();
            //long dt = 0;
            //long count = 0;

            //watch.Start();
            //var t = new RealmTime();
            //do
            //{
            //    if (Manager.Terminating)
            //        break;

            //    var times = dt/MsPT;
            //    dt -= times*MsPT;
            //    times++;

            //    var b = watch.ElapsedMilliseconds;

            //    count += times;
            //    if (times > 3)
            //        log.Warn("LAGGED!| time:" + times + " dt:" + dt + " count:" + count + " time:" + b + " tps:" + count / (b / 1000.0));

            //    t.thisTickTimes = (int) (times * MsPT);

            //    foreach (var i in pendings)
            //    {
            //        while (i.TryDequeue(out var callback))
            //        {
            //            try
            //            {
            //                callback(t);
            //            }
            //            catch (Exception ex)
            //            {
            //                log.Error(ex);
            //            }
            //        }
            //    }
            //    TickWorlds1(t);

            //    var tradingPlayers = TradeManager.TradingPlayers.Where(_ => _.Owner == null).ToArray();
            //    foreach (var player in tradingPlayers)
            //        TradeManager.TradingPlayers.Remove(player);

            //    var requestPlayers = TradeManager.CurrentRequests.Where(_ => _.Key.Owner == null || _.Value.Owner == null).ToArray();
            //    foreach (var players in requestPlayers)
            //        TradeManager.CurrentRequests.Remove(players);

            //    //string[] accIds = Manager.Clients.Select(_ => _.Value.Account.AccountId).ToArray();

            //    //List<string> curGStructAccIds = new List<string>();

            //    //foreach(var i in GuildManager.CurrentManagers.Values)
            //    //    curGStructAccIds.AddRange(i.GuildStructs.Select(_ => _.Key));

            //    //foreach (var i in curGStructAccIds)
            //    //    if (!accIds.Contains(i))
            //    //        GuildManager.RemovePlayerWithId(i);

            //    //var m = GuildManager.CurrentManagers;
            //    //try
            //    //{
            //    //    foreach (var g in m)
            //    //        if (g.Value.Count == 0)
            //    //            GuildManager.CurrentManagers.Remove(g.Key);
            //    //}
            //    //catch (Exception ex)
            //    //{
            //    //    log.Error(ex);
            //    //}

            //    try
            //    {
            //        GuildManager.Tick(CurrentTime);
            //    }
            //    catch (Exception ex)
            //    {
            //        log.Error(ex);
            //    }

            //    Thread.Sleep(MsPT);
            //    dt += Math.Max(0, watch.ElapsedMilliseconds - b - MsPT);
            //} while (true);
            log.Info("Logic loop stopped.");
        }

        private void TickWorlds1(RealmTime t)
        {
            CurrentTime = t;
            foreach (var i in Manager.Worlds.Values.Distinct())
                i.Tick(t);
        }
    }
}