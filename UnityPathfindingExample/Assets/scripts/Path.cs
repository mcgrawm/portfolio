using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path  {

	// a Path is simply a warpper for a sequence of Connections

	public List<Connection> connections;

	public Path () {
		connections = new List<Connection>();
	}


}
