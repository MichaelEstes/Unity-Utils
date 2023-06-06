using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;


namespace MatchMaking
{

    [System.Serializable]
    public class MatchManager : MonoBehaviour
    {
        MatchSpawner spawner;

        Dictionary<System.Guid, Match> matches = new Dictionary<System.Guid, Match>();

        System.Action<Match> onMatchStateChanged;

        private WaitForSeconds raiderUpdateTime = new WaitForSeconds(2f);

        private const float raiderTargetDistance = 26f;

        public void Init(System.Action<Match> onMatchStateChanged)
        {
            this.onMatchStateChanged = onMatchStateChanged;
            spawner = GetComponent<MatchSpawner>();
        }

        public void CreateBackgroundMatch()
        {
            Match match = CreateNewMatch();
            match.SetBackgroundMatch();
        }

        public void MatchSceneLoaded()
        {
            spawner.GetSpawnPoints();
        }

        public void AddPlayerToOpenMatch(NetworkConnection playerConn)
        {
            Match match = GetOpenMatch();
            if (match is null || !match.HasRoom())
            {
                match = CreateNewMatch();
            }

            match.AddPlayer(playerConn);
        }

        public void AddPlayerToHostOnlyMatch(NetworkConnection playerConn)
        {
            Match match = CreateNewMatch();
            match.AddPlayerHostOnly(playerConn);
        }

        public void RemovePlayerFromMatch(NetworkConnection playerConn)
        {
            foreach (Match match in matches.Values)
            {
                if (match.RemovePlayer(playerConn))
                {
                    return;
                }
            }
        }

        Match GetMatchForPlayerConn(NetworkConnection conn)
        {
            foreach (Match match in matches.Values)
            {
                if (match.HasPlayerConn(conn))
                {
                    return match;
                }
            }

            return null;
        }

        public void RemoveAIHumanFromMatch(NetworkIdentity aiHumanId)
        {
            foreach (Match match in matches.Values)
            {
                if (match.RemoveAIHuman(aiHumanId))
                {
                    if (match.aiHumans.Count <= spawner.MinRaiders())
                    {
                        spawner.AddRaidersToMatch(match);
                    }
                    return;
                }
            }
        }

        public Match GetMatch(System.Guid matchId)
        {
            if (!matches.ContainsKey(matchId))
            {
                Debug.LogError("Match not found");
                return null;
            }

            return matches[matchId];
        }

        public IEnumerable<Match> GetMatches()
        {
            return matches.Values;
        }

        Match GetOpenMatch()
        {
            foreach (Match match in matches.Values)
            {
                if (match.state == MatchState.Open)
                {
                    return match;
                }
            }

            return null;
        }

        Match CreateNewMatch()
        {
            Match match = new Match(OnMatchStateChanged);
            matches.Add(match.id, match);
            return match;
        }

        void OnMatchStateChanged(Match match)
        {
            if (match.state == MatchState.Closed)
            {
                matches.Remove(match.id);
            }

            onMatchStateChanged(match);
        }

        public void HandlePlayerMessageForMatch(NetworkConnection playerConn, InitPlayerMessage message, Match match)
        {
            if (!(message.companionData is null))
            {
                spawner.SpawnCompanionForMatch(playerConn, message.companionData, match);
            }
        }

        public void MatchReadyForSpawning(Match match)
        {
            spawner.HandleMatchSpawns(match);
        }
    }
}
