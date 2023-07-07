namespace ObjectMapper
{
    using System.Collections.Generic;
    using System.Xml;

    public class LocalizationSerializer : ISerializer<RepositoryDto<KeyStringPair>>
    {
        private string xml;

        public LocalizationSerializer(string xml)
        {
            this.xml = xml;
        }

        public void Serialize(RepositoryDto<KeyStringPair> obj)
        {
        }

        public RepositoryDto<KeyStringPair> Deserialize()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            System.IO.StringReader stringReader = new System.IO.StringReader(this.xml);
            stringReader.Read();
            this.xml = stringReader.ReadToEnd();
            stringReader.Dispose();
#endif
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(this.xml);
            XmlNodeList texts = xmlDoc.GetElementsByTagName("TEXT");
            List<KeyStringPair> result = new List<KeyStringPair>();
            for (int i = 0; i < texts.Count; i++)
            {
                string nodeKey = texts[i].ChildNodes[0].InnerText;
                string nodeValue = texts[i].ChildNodes[1].InnerText;
                KeyStringPair newItem = new KeyStringPair(nodeKey, nodeValue);
                result.Add(newItem);
            }

            return new RepositoryDto<KeyStringPair>(result);
        }
    }
}