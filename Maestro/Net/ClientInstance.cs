using System;
using System.Net;
using Lidgren.Network;

namespace Edge.Maestro.Net {
	public class ClientInstance {
		public Int64 UUID;
		public String UName;
		public NetConnection Connection;
		public ClientInstance (NetConnection connection, String uname) {
			UUID = connection.RemoteUniqueIdentifier;
			UName = uname;
			Connection = connection;
		}
	}
}

