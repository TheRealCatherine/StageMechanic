using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

[DataContract(Name="Property")]
public class PropertyJsonDelegate
{
	[DataMember]
	public string Key;
	[DataMember]
	public string Value;

	public PropertyJsonDelegate(KeyValuePair<string,string> value)
	{
		Key = value.Key;
		Value = value.Value;
	}
}

