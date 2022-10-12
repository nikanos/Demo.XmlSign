using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Demo.XmlSign.CLI
{
    class SignedXmlWithReferenceIdAttributeName : SignedXml
    {
        private string _referenceIdAttributeName;
        public SignedXmlWithReferenceIdAttributeName(XmlDocument xmlDocument, string referenceIdAttributeName)
            : base(xmlDocument)
        {
            if (referenceIdAttributeName == null)
                throw new ArgumentNullException(nameof(referenceIdAttributeName));

            _referenceIdAttributeName = referenceIdAttributeName;
        }

        public SignedXmlWithReferenceIdAttributeName(XmlElement xmlElement, string referenceIdAttributeName)
            : base(xmlElement)
        {
            if (referenceIdAttributeName == null)
                throw new ArgumentNullException(nameof(referenceIdAttributeName));

            _referenceIdAttributeName = referenceIdAttributeName;
        }
        public override XmlElement GetIdElement(XmlDocument document, string idValue)
        {
            XmlNodeList xmlNodeList = document.SelectNodes(string.Format("//*[@{0}='{1}']", _referenceIdAttributeName, idValue));
            if (xmlNodeList == null || xmlNodeList.Count == 0)
                return null;
            if (xmlNodeList.Count == 1)
                return xmlNodeList[0] as XmlElement;
            else
                throw new CryptographicException($"Should not exist more than one elements with attribute {_referenceIdAttributeName} and value {idValue}.");
        }
    }
}
