using Demo.XmlSign.Lib.Interfaces;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Demo.XmlSign.Lib
{
    public class SignedXmlFactory : ISignedXmlFactory
    {
        public SignedXml CreateSignedXml(XmlDocument xmlDocument)
        {
            return CreateSignedXml(xmlDocument, referenceIdAttributeName: null);
        }

        public SignedXml CreateSignedXml(XmlDocument xmlDocument, string referenceIdAttributeName)
        {
            if (string.IsNullOrEmpty(referenceIdAttributeName))
                return new SignedXml(xmlDocument);
            return new SignedXmlWithReferenceIdAttributeName(xmlDocument, referenceIdAttributeName);
        }
    }
}
