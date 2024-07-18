using UnityEngine.Networking;

public class CustomCertificateHandler : CertificateHandler
{
    // Override the ValidateCertificate method
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        // Simply return true to accept any certificate (use with caution)
        return true;
    }
}

