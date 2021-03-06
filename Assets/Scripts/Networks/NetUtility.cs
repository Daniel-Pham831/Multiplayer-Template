using System;
using Unity.Networking.Transport;
using UnityEngine;

public enum OpCode
{
    KEEP_ALIVE = 1,
    WELCOME = 2,
}

public static class NetUtility
{
    // Net messages
    public static Action<NetMessage> C_KEEP_ALIVE;
    public static Action<NetMessage> C_WELCOME;
    public static Action<NetMessage, NetworkConnection> S_KEEP_ALIVE;
    public static Action<NetMessage, NetworkConnection> S_WELCOME;

    public static void OnData(DataStreamReader streamReader, NetworkConnection cnn, Server server = null)
    {
        NetMessage msg = null;
        var OpCode = (OpCode)streamReader.ReadByte();
        switch (OpCode)
        {
            case OpCode.KEEP_ALIVE:
                msg = new NetKeepAlive(streamReader);
                break;

            case OpCode.WELCOME:
                msg = new NetWelcome(streamReader);
                break;

            default:
                Debug.LogError("Message received has no OpCode");
                break;
        }

        if (server != null)
            msg.ReceivedOnServer(cnn);
        else
            msg.ReceivedOnClient();
    }
}
