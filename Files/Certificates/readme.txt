# Create the self signed certificate with subject DEMO_XML_SIGN and KeySpec Signatue (default KeySpec value is None and we had issues with code that used the private key of the certificate)
# see https://learn.microsoft.com/en-us/powershell/module/pki/new-selfsignedcertificate?view=windowsserver2022-ps
# we also specify the Microsoft Enhanced RSA and AES Cryptographic Provider to suppert the newer hasing algorithms that are required in the new versions of .NET Framework (see https://learn.microsoft.com/en-us/powershell/module/pki/new-selfsignedcertificate?view=windowsserver2022-ps)
$cert = New-SelfSignedCertificate -Subject DEMO_XML_SIGN -NotAfter (Get-Date).AddYears(10) -KeySpec Signature -Provider "Microsoft Enhanced RSA and AES Cryptographic Provider" -Certstorelocation Cert:\CurrentUser\My

# Export the previously created certificate to PFX (including the private key) named DEMO_XML_SIGN.pfx in the current directory using the password qwerty
# see https://learn.microsoft.com/en-us/powershell/module/pki/export-pfxcertificate?view=windowsserver2022-ps
$mypwd = ConvertTo-SecureString -String "qwerty" -Force -AsPlainText
Export-PfxCertificate -Cert $cert -FilePath DEMO_XML_SIGN.pfx -Password $mypwd

