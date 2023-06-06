using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using MatchMaking;
public class MatchInterestManager : InterestManagement
{
    public Dictionary<NetworkIdentity, System.Guid> identityToMatch = new Dictionary<NetworkIdentity, System.Guid>();

    public override bool OnCheckObserver(NetworkIdentity identity, NetworkConnection newObserver)
    {
        if (identityToMatch.ContainsKey(identity) && identityToMatch.ContainsKey(newObserver.identity))
        {
            return identityToMatch[identity] == identityToMatch[newObserver.identity];
        }

        Debug.LogError("Identities not found");
        return false;
    }

    public override void OnRebuildObservers(NetworkIdentity identity, HashSet<NetworkConnection> newObservers, bool initialize)
    {
        if (!identityToMatch.ContainsKey(identity)) return;

        System.Guid matchId = identityToMatch[identity];

        foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
        {
            if (identityToMatch.ContainsKey(conn.identity) && matchId == identityToMatch[conn.identity])
            {
                newObservers.Add(conn);
            }
        }
    }

    public void UpdateMatch(Match match)
    {
        if (!NetworkServer.active) return;

        System.Guid id = match.id;

        foreach (NetworkIdentity identity in match.GetAllIdentities())
        {
            AddToMatch(identity, id);
        }

        RebuildAll();
    }

    public void UpdateMatches(MatchManager matches)
    {
        if (!NetworkServer.active) return;

        identityToMatch.Clear();

        foreach (Match match in matches.GetMatches())
        {
            System.Guid id = match.id;

            foreach (NetworkIdentity identity in match.GetAllIdentities())
            {
                AddToMatch(identity, id);
            }
        }

        RebuildAll();
    }

    void AddToMatch(NetworkIdentity identity, System.Guid matchId)
    {
        if (!identityToMatch.ContainsKey(identity))
        {
            identityToMatch.Add(identity, matchId);
        }
    }

    public System.Guid GetMatchIDForNetId(NetworkIdentity identity)
    {
        return identityToMatch[identity];
    }

    public void RemoveMatch(Match match)
    {
        if (!NetworkServer.active) return;

        foreach (NetworkIdentity identity in match.GetAllIdentities())
        {
            RemoveFromMatch(identity);

            if (GameManager.IsOnline)
            {
                NetworkServer.Destroy(identity.gameObject);
            }
        }

        RebuildAll();
    }

    public void RemoveFromMatch(NetworkIdentity identity)
    {
        identityToMatch.Remove(identity);
    }
}
