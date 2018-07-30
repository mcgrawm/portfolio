using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour {

	public Node nodePrefab;
	public Connector connectorPrefab;

	public NodeContainer nodeContainer;


	// Use this for initialization
	private List<Node> _nodes;
	private List<Connector> _connectors;

	public List<Node> GetNodes() {
		return _nodes;
	}

	public IEnumerator ResetNodesAndPaths() {
		_nodes 			= new List<Node>(GameObject.FindObjectsOfType<Node>());
		_connectors 	= new List<Connector>(GameObject.FindObjectsOfType<Connector>());
		
		foreach(Connector c in _connectors) {
			Destroy(c.gameObject);
		}

		foreach(Node n in _nodes) {
			Destroy(n.gameObject);
		}

		_nodes = new List<Node>();
		_connectors = new List<Connector>();

		yield return CreateNodes(32, 1.5f);
		yield return ConnectNodes(5f);

	}

	public void ResetNodeMaterials() {
		foreach(Node n in _nodes) {
			n.SetDefaultMaterial();
			foreach(Connection c in n.connections) {
				c.connector.SetDefaultLook();
			}
		}
	}

	private IEnumerator CreateNodes(int nodesLeftToPlace, float minDistance) {

		float yCursor = 0f;

		// Place nodes
		while(nodesLeftToPlace > 0) {

			yCursor += 1f;
			//Debug.Log("Nodes Left to place " + nodesLeftToPlace);

			bool validLocation = false;

			while(!validLocation) {
				// keep trying to place this node until it is far enough from all other nodes
				yield return new WaitForSeconds(.002f);

				Vector3 direction = new Vector3(Random.Range(-180f,180f), Random.Range(-180f,180f), Random.Range(-180f,180f));
				Vector3 nodeLocation = this.transform.position + (direction.normalized * 5f);
				
				//Vector3 direction = new Vector3(Random.Range(0f,359f), 0f, Random.Range(0f,359f));
				//Vector3 nodeLocation = this.transform.position + Vector3.up * yCursor + (direction.normalized * 3f);

				// check distance to all other nodes before adding this one
				bool isBlocked = false;
				foreach(Node n in _nodes) {
					if(Vector3.Distance(nodeLocation,n.transform.position) < minDistance) {
						isBlocked = true;
						break;
					} 
				}

				if(!isBlocked) {
					validLocation = true;
					Node theNode = Instantiate<Node>(nodePrefab,nodeLocation,Quaternion.identity);
					_nodes.Add(theNode);
					theNode.gameObject.name = "Node " + nodesLeftToPlace;
					theNode.transform.parent = nodeContainer.transform;
					nodesLeftToPlace--;
				}

			}

		}

	}

	private IEnumerator ConnectNodes(float maxConnectDistance) {
		
		// connect near nodes with paths
		foreach(Node n in _nodes) {

			foreach(Node n2 in _nodes) {
				
				// same node?
				if(n == n2)
					continue;

				// does node have more than 3 connecitons already?
				if(n .connections.Count > 3 || n2.connections.Count > 3)
					continue;


				// Check for proximity
				float distance = Vector3.Distance(n.transform.position, n2.transform.position);

				if(distance <= 5f) {

					// check for line of sight 
					bool los 			= true;
					Vector3 direction 	= n2.transform.position - n.transform.position;
					
					RaycastHit[] hits;
					hits 				= Physics.RaycastAll(n.transform.position,direction, distance);
					for (int i = 0; i < hits.Length; i++) {
						GameObject go = hits[i].collider.transform.parent.gameObject;
						if(go.tag == "blocksConnection") {
							if(go != n.gameObject && go != n2.gameObject) {
								los = false;
								break;
							} 
						} 
					}	

					if(los) {

						Connector c = Instantiate<Connector>(connectorPrefab, (n.transform.position + n2.transform.position)/2f, Quaternion.identity);
						c.transform.LookAt(n.transform);
						//Debug.Log("Distance "+distance);
						c.transform.localScale = new Vector3(1f,1f,Mathf.Max(distance, .1f));
						c.transform.parent = nodeContainer.transform;

						n.ConnectNode(n2,c);
						n2.ConnectNode(n,c);
					}

				}
			}

		}
		yield return null;
	}


}
