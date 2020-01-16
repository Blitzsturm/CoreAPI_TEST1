using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace core
{
	public class GetShit
	{
		private HttpClient client;
		private DataContractJsonSerializerSettings jsonSerializerSettings;

		public GetShit()
		{
			client = new HttpClient();
			jsonSerializerSettings = new DataContractJsonSerializerSettings()
            {
                UseSimpleDictionaryFormat = true,
                DateTimeFormat = new DateTimeFormat("yyyy-MM-ddTHH:mm:ss.fff+0000")
            };
		}

		public async Task<string> Get()
		{
			string strURL = "https://pfq08qkv82.execute-api.us-west-2.amazonaws.com/default/usppa_schools";
			HttpResponseMessage response = await client.GetAsync(strURL);
			return await response.Content.ReadAsStringAsync();
		}
		
		public async Task<List<Dictionary<string,string>>> GetObj()
		{
			string strURL = "https://pfq08qkv82.execute-api.us-west-2.amazonaws.com/default/usppa_schools";
			HttpResponseMessage response = await client.GetAsync(strURL);
			string strResponse = await response.Content.ReadAsStringAsync();
			return (List<Dictionary<string,string>>)Serialize(strResponse, typeof(List<Dictionary<string,string>>));
		}
		

        // Object to JSON Text Serialization
        private string Serialize(object objInput)
        {
            if (objInput == null) return "null";
            DataContractJsonSerializer js = new DataContractJsonSerializer(objInput.GetType(), jsonSerializerSettings);
            using (MemoryStream ms = new MemoryStream())
            {
                js.WriteObject(ms, objInput);
                ms.Position = 0;
                using (StreamReader sr = new StreamReader(ms))
                    return sr.ReadToEnd();
            }
        }

        // JSON Text to Object Deserialization
        private object Serialize(string strInput, Type typObject)
        {
            return Serialize(new MemoryStream(ASCIIEncoding.ASCII.GetBytes(strInput)), typObject);
        }

        // JSON Stream to Object Deserialization
        private object Serialize(MemoryStream strInput, Type typObject)
        {
            DataContractJsonSerializer js = new DataContractJsonSerializer(typObject, jsonSerializerSettings);
            return js.ReadObject(strInput);
        }
	}
}
