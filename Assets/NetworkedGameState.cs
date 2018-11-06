using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkedGameState : NetworkBehaviour
{


	[SyncVar] public bool isAwating = false;
	[SyncVar] public bool isApproved = false;
	[SyncVar] public int nodeID;

	[Command]
	public void CmdSetAwating(bool val)
	{
		isAwating = val;
	}
	
	[Command]
	public void CmdSetApproved(bool val)
	{
		isApproved = val;
	}
	
	[Command]
	public void CmdSetNodeID(int val)
	{
		nodeID = val;
	}
}
