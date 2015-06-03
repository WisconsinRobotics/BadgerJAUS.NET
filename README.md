# BadgerJAUS.NET
A C# implementation of the Joint Architecture for Unmanned Systems protocol.

BadgerJAUS is licensed under the 3-clause BSD license.

BadgerJAUS was created by the Wisconsin Robotics team at the University of
Wisconsin-Madison. The original implementation was a Java library derived from
the discontinued OpenJAUS Java implementation. Since the original fork the
library saw substantial modification and updates and the C# version represents
an effective rewrite of the code while still employing some of the ideas and
structure employed in the original OpenJAUS code.

In its current form BadgerJAUS is meant to be compiled and used as an external
library. It is flexible and can be used purely for parsing JAUS messages or it
can serve as the core of a system following the JAUS architecture with systems,
nodes, components, and services. The design of BadgerJAUS itself is meant to
be object oriented with clean encapsulation and heavy code reuse between
messages of the same category. This is intended not only to reduce redundant
code but also to make it easier to create new messages. For further
documentation on how to use or extend BadgerJAUS please see the documentation
on the project's github wiki.

BadgerJAUS is not yet incomplete, several messages and services are missing
outright and many others have not been finished or have known bugs or
incorrect implementations. The documentation is in a similar if not more
non-existent state.