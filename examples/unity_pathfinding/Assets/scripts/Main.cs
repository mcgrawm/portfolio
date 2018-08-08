using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	private NodeManager _nodeManager;
	private Node _startNode;
	private Node _endNode;

	void Awake () {
		_nodeManager = GameObject.FindObjectOfType<NodeManager>();
	}

	void Start() {
		Node.onClicked += OnNodeClickedHandler;
		StartCoroutine(SetUpGame());
	}

	private IEnumerator SetUpGame() {
		yield return _nodeManager.ResetNodesAndPaths();
	}

	public void ResetGame() {
		// reset stuff - note, this is called by the Reset button in the UI
		StopAllCoroutines();
		_startNode 	= null;
		_endNode 	= null;
		StartCoroutine(SetUpGame());
	}

	private void OnNodeClickedHandler(Node n) {
		if(_startNode == null) {
			// no nodes currently selectec, treat this as startNode
			_startNode = n;
			n.SetEndEndPointMaterial();
		} else {
			if(n == _startNode)
				// user clicked startNode a second time, ignore click
				return;
			if(_endNode == null) {
				// startNode already selected, so treat this node as the endNode
				_endNode = n;
				n.SetEndEndPointMaterial();

				// immediately kick off the pathfinding algorithm
				Path path = PathFinder.FindPath(_nodeManager.GetNodes(), _startNode, _endNode);
				// Path returns immediately, kick of the rendering of the path (coroutine)
				StartCoroutine(RenderPath(path));
			} else {
				// both nodes already chosen, clear the current coroutine, reset materials, reset startNode, clear endNode
				// TODO this only occurs if the user quickly clicks a third node while the path is rendering - needs testing.
				StopAllCoroutines();
				_nodeManager.ResetNodeMaterials();
				_startNode = n;
				n.SetEndEndPointMaterial();
				_endNode 	= null;
			}

		}
	}

	private IEnumerator RenderPath(Path path) {
		// This renders the path using some delays so user can see the path build
		yield return new WaitForSeconds(.1f);

		if(path.connections.Count == 0) {
			// if the Path returned has no connections in it, it means there was no valid path between the start and end nodes, so turn them both red
			_startNode.SetErrorMaterial();
			_endNode.SetErrorMaterial();
		} else {
			// valid path found, begin "animating" the path by highlighting each connection (a connector and a node) in sequence
			foreach(Connection c in path.connections) {
				if(c.node == path.connections[0].node) {
					// ignore the first connection in the path - it's already highlighted
				} else {
					// change visiual of connector (the stick part)
					c.connector.SetPathLook();
					yield return new WaitForSeconds(.15f);
					// change the visual of the node (the sphere part), unless it's the very last node in the path, as that's already highlighted
					if(path.connections[path.connections.Count-1] != c)
						c.node.SetMidPathMaterial();
				}
				yield return new WaitForSeconds(.25f);
			}
			// done animating the path
		}

	}


	void Update () {
	
		if(Input.GetMouseButtonDown(0)) {
			RaycastHit[] hits;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			hits = Physics.RaycastAll(ray,100f);

			if(hits.Length > 0 ) {
				Node n = hits[0].collider.gameObject.GetComponent<Node>();
				if(n != null) {
					OnNodeClickedHandler(hits[0].collider.gameObject.GetComponent<Node>());
				}

			}

		}
	}

}
