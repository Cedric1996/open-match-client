using System;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Grpc.Core;
using System.Collections.Generic;
using UnityEngine.Ucg.Matchmaking;

public class MatchMaking
{
    Matchmaker m_matchmaker;
    public String endpoint;

    public MatchMaking(String endpoint) {
        this.endpoint = endpoint;
    }

    public void Start()
    {
        try
        {
            m_matchmaker = new Matchmaker(endpoint, OnMatchmakingSuccess, OnMatchmakingError);

            MatchmakingPlayerProperties playerProps = new MatchmakingPlayerProperties() { hats = 5 };
            MatchmakingGroupProperties groupProps = new MatchmakingGroupProperties() { mode = 0 };

            m_matchmaker.RequestMatch("User_1", playerProps, groupProps);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void Update()
    {
        m_matchmaker.Update();
    }

    void OnMatchmakingSuccess(Assignment assignment)
    {
        if (string.IsNullOrEmpty(assignment.ConnectionString))
        {
            Debug.Log("Matchmaking finished, but did not return a game server.  Ensure your server has been allocated and is running then try again.");
            Debug.Log($"MM Error: {assignment.AssignmentError ?? "None"}");
        }
        else
        {
            Debug.Log($"Matchmaking has found a game! The server is at {assignment.ConnectionString}.  Attempting to connect...");
        }
        m_matchmaker = null;
    }

    void OnMatchmakingError(string errorInfo)
    {
        Debug.LogError($"Matchmaking failed! Error is: {errorInfo}");
        m_matchmaker = null;
    }

}
