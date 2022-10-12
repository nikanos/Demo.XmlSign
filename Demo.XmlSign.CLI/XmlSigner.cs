using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Demo.XmlSign.CLI
{
    class XmlSigner
    {
        public XmlDocument SignXml(XmlDocument xmlToSign, X509Certificate2 signingCertificate, string referenceID, string referenceIDAttributeName = null)
        {
            if (xmlToSign == null)
                throw new ArgumentNullException(nameof(xmlToSign));
            if (signingCertificate == null)
                throw new ArgumentNullException(nameof(signingCertificate));
            if (signingCertificate.PrivateKey == null)
                throw new ArgumentException("Certificate should contain a private key", nameof(signingCertificate));
            if (referenceID == null)
                throw new ArgumentNullException(nameof(referenceID));
            if (referenceID == string.Empty)
                throw new ArgumentException("Should contain a value", nameof(referenceID));

            XmlDocument xml = (XmlDocument)xmlToSign.Clone();

            SignedXml signedXml;
            if (string.IsNullOrEmpty(referenceIDAttributeName))
                signedXml = new SignedXml(xml);
            else
                signedXml = new SignedXmlWithReferenceIdAttributeName(xml, referenceIDAttributeName);

            KeyInfo keyInfo = new KeyInfo();

            // Add the key to the SignedXml document.
            signedXml.SigningKey = signingCertificate.PrivateKey;
            KeyInfoX509Data keyInfoData = new KeyInfoX509Data();
            keyInfoData.AddCertificate(signingCertificate);
            keyInfo.AddClause(keyInfoData);

            signedXml.KeyInfo = keyInfo;
            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;

            // Create a reference to be signed.
            Reference reference = new Reference();
            reference.Uri = $"#{referenceID}";
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigExcC14NTransform());

            signedXml.AddReference(reference);
            signedXml.ComputeSignature();

            // Get the XML representation of the signature and save it to an XmlElement object.
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            // Append the element to the element with the id for which the signature was generated.
            XmlElement idelement = signedXml.GetIdElement(xml, referenceID);
            if (idelement != null)
            {
                idelement.AppendChild(xml.ImportNode(xmlDigitalSignature, deep: true));
            }
            return xml;
        }
    }
}
