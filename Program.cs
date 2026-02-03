using System;
using System.Windows.Forms;
using smartLibraryForC_.Views;

namespace smartLibraryForC_
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Global Security Protocol Setup
            // Force TLS 1.2 and 1.1 support
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
            
            // Bypass SSL Certificate Validation (Fixes "The underlying connection was closed: Could not establish trust relationship")
            // WARNING: Use with caution in production. Useful for University Projects/Debugging.
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            // Ensure connection to database or other global setup if needed
            
            Application.Run(new MainForm());
        }
    }
}
