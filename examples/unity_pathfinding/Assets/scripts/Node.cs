using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour {

	// Nodes are the balls that are connected to one another in a mesh by Connections
	public static event Action<Node> onClicked = delegate {};

	public Material defaultMaterial;
	public Material midPathMaterial;
	public Material endPointMaterial;
	public Material errorMaterial;
	public GameObject sphere;

	private List<Connection> _connections;


	// Use this for initialization
	void Awake () {
		_connections 	= new List<Connection>();
		SetDefaultMaterial();
	}

	public List<Connection> connections {
		get { return _connections;}
	}

	public void ConnectNode(Node otherNode, Connector connector) {
		// only add the connection if one with the other connector hasn't already been added
		bool newConnection = true;
		foreach(Connection c in _connections)
		{
			if(c.node == otherNode) {
				newConnection = false;
				break;
			}
		}

		if(newConnection) {
			_connections.Add(new Connection(otherNode, connector));
		}
	}
	
	private void SetMaterial(Material m) {
		sphere.GetComponent<Renderer>().material = m;
	}

	public void SetErrorMaterial() {
		SetMaterial(errorMaterial);
	}

	public void SetEndEndPointMaterial() {
		SetMaterial(endPointMaterial);
	}

	public void SetMidPathMaterial() {
		SetMaterial(midPathMaterial);
	}

	public void SetDefaultMaterial() {
		SetMaterial(defaultMaterial);
	}



}
