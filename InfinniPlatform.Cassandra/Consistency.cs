namespace InfinniPlatform.Cassandra
{
	/// <summary>
	/// Represents consistency levels
	/// </summary>
	public enum Consistency
	{
		/// <summary>
		/// A write must be written to at least one node.
		/// A read returns a response from the closest replica.
		/// </summary>
		Any,

		/// <summary>
		/// A write must be written to the commit log and memory table of at least one replica node.
		/// A read returns a response from the closest replica.
		/// </summary>
		One,

		/// <summary>
		/// A write must be written to the commit log and memory table of at least two replica nodes.
		/// A read returns the most recent data from two of the closest replicas.
		/// </summary>
		Two,

		/// <summary>
		/// A write must be written to the commit log and memory table of at least three replica nodes.
		/// A read returns the most recent data from three of the closest replicas.
		/// </summary>
		Three,

		/// <summary>
		/// A write must be written to the commit log and memory table on a quorum of replica nodes.
		/// A read returns the record with the most recent timestamp after a quorum of replicas has responded.
		/// </summary>
		Quorum,

		/// <summary>
		/// A write must be written to the commit log and memory table on a quorum of replica nodes in the same data center as the coordinator node.
		/// A read returns the record with the most recent timestamp after a quorum of replicas in the current data center as the coordinator node has reported.
		/// </summary>
		LocalQuorum,

		/// <summary>
		/// A write must be written to the commit log and memory table on a quorum of replica nodes in all data centers.
		/// A read returns the record with the most recent timestamp after a quorum of replicas in each data center of the cluster has responded.
		/// </summary>
		EachQuorum,

		/// <summary>
		/// A write must be written to the commit log and memory table on all replica nodes in the cluster for that row key.
		/// A read returns the record with the most recent timestamp after all replicas have responded.
		/// </summary>
		All,
	}
}