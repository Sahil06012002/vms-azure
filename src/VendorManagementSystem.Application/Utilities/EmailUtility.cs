namespace VendorManagementSystem.Application.Utilities
{
    public static class EmailUtility
    {
        public static string GetInvitationBody(string toName, string fromName, string link)
        {
            toName = toName.Replace("$", " ");
            Console.WriteLine(toName);
            return @$"<p>Hello {toName},</p>
                        <p>You have been invited to join EX Squared Vendor Management System by {fromName}</p>
                        <p>Please click on the following link to set your password to get started:</p>
                        <a href={link}>Generate New Password</a>
                        <p>Best Regards,</p>
                        <p>VMS team</p>";
        }
        public static string ForgetPasswordBody(string toName, string link)
        {
            toName = toName.Replace("$", " ");
            return @$"<p>Hello {toName},</p
                        <p>Please click on the following link to reset your password to get started:</p>
                        <a href={link}>Generate New Password</a>
                        <p>Best Regards,</p>
                        <p>VMS team</p>";
        }
    }
}
