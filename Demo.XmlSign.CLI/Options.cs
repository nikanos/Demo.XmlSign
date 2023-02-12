using CommandLine;

namespace Demo.XmlSign.CLI
{
    class Options
    {
        [Option('i', "input", Required = true, HelpText = "Input XML file to be signed.")]
        public string InputFile { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output Signed XML files")]
        public string OutputFile { get; set; }

        [Option('c', "certificate", Required = true, HelpText = "Certificate to be used for signing (should have a private key)")]
        public string Certificate { get; set; }

        [Option('r', "referenceid", Required = true, HelpText = "Reference ID for element to be signed")]
        public string ReferenceId { get; set; }

        [Option('n', "name", Required = false, HelpText = "Reference ID attribute name")]
        public string ReferenceIdAttributeName { get; set; }

        [Option('p', "password", Required = false, HelpText = "Password for certificate")]
        public string CertificatePassword { get; set; }

    }
}
