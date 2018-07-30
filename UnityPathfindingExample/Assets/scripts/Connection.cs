using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection {

	// A Connection is a wrapper object containing one connector (stick) and one Node (ball)

	private Connector _connector;
	private Node _node;

	public Connection(Node node, Connector connector) {
		this._node = node;
		this._connector	= connector;
	}

	public Connector connector {
		get { 
			return _connector;
		}
	}
	public Node node {
		get { 
			return _node;
		}
	}

}
