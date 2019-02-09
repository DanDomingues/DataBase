The Grid Pathfinding module contains assembly and pathfinding for a grid-based game, such as a entry of the "Tatics" genre

Pathfinding methods use a Unity-sensitive version of A*, using Euclidian distance as the Heuristic
In the current state, the path is found within the same frame as it is requested
If used in larger maps, improving to a progressive algorithm is advised, specially if deployed to mobile devices

As to the Grid Assembly, it is ran through several frames, as to avoid crashes and alike.
Once the setup is finished, an delegate is called. Best practices suggest deploying a loading screen while the setup runs

----------------------------------------------------------------------------------------------------------------------------
Uploaded at 02/09/2019. Free usage is granted AS IS by Danilo Domingues
Further alterations are allowed, but any effects of such are no longer under accountability of the original developer.  
----------------------------------------------------------------------------------------------------------------------------