using Demo.XmlSign.Lib.Interfaces;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Demo.XmlSign.Lib
{
    public class XmlSigner
    {
        private readonly ISignedXmlFactory _signedXmlFactory;

        public XmlSigner(ISignedXmlFactory signedXmlFactory)
        {
            _signedXmlFactory = signedXmlFactory ?? throw new ArgumentNullException(nameof(_signedXmlFactory));
        }

        public XmlDocument SignXml(XmlDocument xmlToSign, X509Certificate2 signingCertificate, string referenceId, string referenceIdAttributeName)
        {
            if (xmlToSign == null)
                throw new ArgumentNullException(nameof(xmlToSign));
            if (signingCertificate == null)
                throw new ArgumentNullException(nameof(signingCertificate));
            if (signingCertificate.PrivateKey == null)
                throw new ArgumentException("Certificate should contain a private key", nameof(signingCertificate));
            if (referenceId == null)
                throw new ArgumentNullException(nameof(referenceId));
            if (referenceId == string.Empty)
                throw new ArgumentException("Should contain a value", nameof(referenceId));

            XmlDocument xml = (XmlDocument)xmlToSign.Clone();
            SignedXml signedXml = _signedXmlFactory.CreateSignedXml(xml, referenceIdAttributeName);
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
            reference.Uri = $"#{referenceId}";
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigExcC14NTransform());

            signedXml.AddReference(reference);
            signedXml.ComputeSignature();

            // Get the XML representation of the signature and save it to an XmlElement object.
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            // Append the element to the element with the id for which the signature was generated.
            XmlElement idelement = signedXml.GetIdElement(xml, referenceId);
            if (idelement != null)
            {
                idelement.AppendChild(xml.ImportNode(xmlDigitalSignature, deep: true));
            }
            return xml;
        }

        public XmlDocument SignXml(XmlDocument xmlToSign, X509Certificate2 signingCertificate, string referenceId)
        {
            return SignXml(xmlToSign, signingCertificate, referenceId, referenceIdAttributeName: null);
        }
    }
}
