using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder  {

	/* 	this is the application's path finding logic.
		FindPath is called, passed the mesh of Nodes, the starting node, and the ending node.
		One Path is returned - it will either be empty of Connections (meaning no valid path was found),
		or it will contain a sequence of Connections (1 connection has exactly one connector (stick) and one Node (ball)),
		starting with StartNode and ending at endNode
	*/


	public static Path FindPath(List<Node> nodes, Node startNode, Node endNode) {

		// create a working list of paths to be examined next - each path is a sequence of 1 or more connected Nodes (Connections). 
		List<Path> seekPaths = new List<Path>();

		// Start with one Path in the seekPaths list, containing 1 new Connection which contains the starting node.
		Path startPath 	= new Path();
		startPath.connections.Add(new Connection(startNode,null));
		seekPaths.Add(startPath);

		// now explore outwards from this starting path to create longer paths, ignoring loops (paths that cross the same node more than once)
		while(seekPaths.Count > 0){		

			// take the top path off stack, and expand it if possible to form one or more longer paths (longer by one connection only)
			Path rootPath = seekPaths[0];
			seekPaths.Remove(rootPath);

			// the last connection in the path is the one we want to extend in each possible direction, thereby creating 0...n new Paths
			Connection endOfRootPath = rootPath.connections[rootPath.connections.Count-1];

			// each Connection off of our rootPath's end Connection could be a potential new path to explore...
			foreach(Connection newConnection in endOfRootPath.node.connections) {

				// check to see if each Connection's node is already in our path somewhere - this would be a loop, which we will avoid.
				bool thisNodeAlreadyInPath = false;
				foreach(Connection existingConnection in rootPath.connections) {
					if(existingConnection.node == newConnection.node) {
						thisNodeAlreadyInPath = true;
						break;
					}
				}

				// if the node is not yet in our path, create a new path by cloning our root path, and adding this last connection to the end of it
				if(!thisNodeAlreadyInPath) {
					Path newPath = new Path();
					// this new path clones our targetPath, adding one to connection to the end
					foreach(Connection rootPathConnection in rootPath.connections) {
						newPath.connections.Add(rootPathConnection);
					}
					// then add the newly discovered connection to the end, which forms a new, slightly longer path
					newPath.connections.Add(newConnection);

					// by the way, is our endNode at the end of our new path?
					if(newConnection.node == endNode) {
						// if yes, for now, just return this path - it might not be the only path or the best path, but it works...
						// TODO : this algorithm could be smarter about exploring all the paths and then choosing the shortest,, but for now, this works fine
						return newPath;
					} else {
						// if we haven't found the endNode yet, just stick this new path in our seekPaths working list - we'll get back to it soon...
						seekPaths.Add(newPath);
					}
				}
			}

		} 

		// We've fallen out of the while loop, meaning we ran out of paths to explore, so we'll return an empty path, indicating there is no valid route
		return new Path();

	}

}
