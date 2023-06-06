using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Mirror;

namespace MatchMaking
{

    public enum MatchState
    {
        Open,
        InProgress,
        Closed
    }

    [System.Serializable]
    public class Match
    {
        public System.Guid id;
        public MatchState state;
        public HashSet<NetworkConnection> players = new HashSet<NetworkConnection>();
        public HashSet<NetworkIdentity> aiHumans = new HashSet<NetworkIdentity>();
        public HashSet<NetworkIdentity> loot = new HashSet<NetworkIdentity>();
        public HashSet<NetworkIdentity> items = new HashSet<NetworkIdentity>();

        bool backgroundMatch = false;

        System.Action<Match> onMatchStateChanged;

        const int maxPlayerCount = 2;
        const int openTime = 1;

        public Match(System.Action<Match> onMatchStateChanged)
        {
            id = System.Guid.NewGuid();
            this.onMatchStateChanged = onMatchStateChanged;
            StartMatchOpenTimer();
        }

        public void SetBackgroundMatch()
        {
            backgroundMatch = true;
        }

        public bool IsBackgroundMatch()
        {
            return backgroundMatch;
        }

        async void StartMatchOpenTimer()
        {
            await Task.Delay(TimeSpan.FromSeconds(openTime));
            StartMatch();
        }

        public bool HasRoom()
        {
            return state == MatchState.Open && players.Count < maxPlayerCount;
        }

        public void AddPlayer(NetworkConnection playerConn)
        {
            players.Add(playerConn);
            if (!HasRoom())
            {
                StartMatch();
            }
        }

        public void AddPlayerHostOnly(NetworkConnection playerConn)
        {
            players.Add(playerConn);
            StartMatch();
        }

        public bool RemovePlayer(NetworkConnection playerConn)
        {
            bool removed = players.Remove(playerConn);

            if (removed)
            {
                CheckMatchStatus();
            }

            return removed;
        }

        public bool HasPlayerConn(NetworkConnection playerConn)
        {
            return players.Contains(playerConn);
        }

        void StartMatch()
        {
            if (state != MatchState.Open) return;

            state = MatchState.InProgress;
            onMatchStateChanged(this);
        }

        public bool IsReadyToStart()
        {
            foreach (NetworkConnection conn in players)
            {
                if (!conn.isReady)
                {
                    return false;
                }
            }

            return true;
        }

        void CheckMatchStatus()
        {
            if (players.Count == 0)
            {
                CloseMatch();
            }
        }

        void CloseMatch()
        {
            state = MatchState.Closed;
            onMatchStateChanged(this);
        }

        public void AddAIHuman(NetworkIdentity aiHuman)
        {
            aiHumans.Add(aiHuman);
        }

        public bool RemoveAIHuman(NetworkIdentity aiHuman)
        {
            return aiHumans.Remove(aiHuman);
        }

        public void AddLoot(NetworkIdentity lootItem)
        {
            loot.Add(lootItem);
        }

        public void AddItem(NetworkIdentity item)
        {
            items.Add(item);
        }

        public void RemoveItem(NetworkIdentity item)
        {
            items.Remove(item);
        }

        public List<NetworkIdentity> GetAllIdentities()
        {
            List<NetworkIdentity> ids = new List<NetworkIdentity>();

            foreach (NetworkConnection conn in players)
            {
                ids.Add(conn.identity);
            }

            ids.AddRange(aiHumans);
            ids.AddRange(loot);
            ids.AddRange(items);

            return ids;
        }
    }
}
