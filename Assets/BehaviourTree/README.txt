

Behaviour Tree Editor

The Behaviour tree editor is a system in which you can use nodes on a graph to link together scripts.
tghese scripts can be used to control game objects in Unity and can be used to create AI.

Set Up
To set up an object with it's own behaviour tree an object needs its 3 components.
	1. The Behaviour Tree Runner component, located in the same folder as this file.
	2. Its own behaviour tree editor(which is a scriptable object that will be passed into the Behaviour Tree Runner Component), which can be created from the create 
		menu within Behavior Tree.
	3. Its own AI Agent script, which can also be created from the create menu within Behaviour Tree.

How it works
The Behaviour tree is a tree consisting of nodes. Each node contains four functions:
	1. OnStart - this function is called once when the node is activated
	2. OnStop - this function is called once when the node is deactivated
	3. OnUpdate - this function is called once everyframe while this node is active
	4. OnFixedUpdate - this function is called on the fixed time stamp, so once every 20ms by default
A Node has three states:
	1. State.Running. 
	2. State.Success. 
	3. State.Failure.

The Tree is run through in sequence, from top to bottom, left to right.
Starting from the Root Node, each Node will activate and deactivate in sequence.
A Node is, by default, in its Running State. The Behaviour Tree will stay in the Nodes OnUpdate() function until its state is either set to Success or Failure,
at which point it will move to the next node in sequence.
The script can access the attached AI Agent class by accessing the public variable "Agent."


Create Nodes
Nodes can be created from the Create menu within the behaviour tree. 
There are three types of nodes;
	1. Action Node - These are the "leaf" nodes of the tree, they have an input but not output, these nodes are, for the most part, used to control the AI Agent.
	2. Composite Node - These nodes are used to organise and control the flow of the behaviour tree, they have a single input and multi output.
	3. Decorator Node - These nodes can be used a number of ways, to influence the group of nodes either beneath or above them. 
		they have a single input and single output.
Once a node has been created, you can edit the script to control the flow of the tree and the attached AI Agent.

Using the Behaviour Tree Editor.
Once you have created The Editor, some nodes and have attached everything to the appropriate game object, you can now use the editor.
double click the Behaviour Tree Editor Scriptable Object you have created, or open it from the Window menu to view the editor.

From here you can right-click the graph itself to bring up the Context menu from which you can place nodes, then hook them up by clicking and dragging from the 
Output (bottom) of one node to the Input (top) of another.
