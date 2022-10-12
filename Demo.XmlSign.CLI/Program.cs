using CommandLine;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace Demo.XmlSign.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);
        }

        static void RunOptions(Options opts)
        {
            try
            {
                string certificatePassword = opts.CertificatePassword;
                if (string.IsNullOrEmpty(certificatePassword))
                {
                    Console.Write($"Please enter password for {opts.Certificate}:");
                    certificatePassword = Utils.GetPassword();
                }
                X509Certificate2 signingCertificate = new X509Certificate2(opts.Certificate, certificatePassword);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(opts.InputFile);

                var xmlSigner = new XmlSigner();
                XmlDocument signedXml = xmlSigner.SignXml(xmlDoc, signingCertificate, opts.ReferenceID, opts.ReferenceIdAttributeName);
                Console.WriteLine("XML signed sucessfully!");

                Console.WriteLine($"Saving result to {opts.OutputFile}");
                signedXml.Save(opts.OutputFile);
                Console.WriteLine("Done!!!");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
            }
        }
        static void HandleParseError(IEnumerable<Error> errs)
        {
        }
    }
}
