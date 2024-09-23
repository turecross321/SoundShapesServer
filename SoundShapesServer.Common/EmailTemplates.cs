﻿namespace SoundShapesServer.Common;

public static class EmailTemplates
{
  public static string VerifyEmail(string userName, string instanceName, string websiteUrl, string verifyEmailCode, bool hasUserFinishedRegistration)
  {
    string verifyUrl = $"{websiteUrl}/verifyEmail?code={verifyEmailCode}";

    string html = hasUserFinishedRegistration
      ? """
        <html lang="en" style="font-size: 10pt; font-family: Tahoma, serif;">
         <body style="color:#181515;">
           <h1>Hello, {USER}</h1>
           <p>Please click the button below to verify your email address.</p>
           <a href="{CODE_URL}" style="color: white; background-color: #F07167; padding: 0.5rem 1rem; border-radius: 0.5rem; font-size: x-large; text-decoration: none; display: inline-block;">Verify</a>
           <p>If you didn't request this, please ignore this email.</p>
           <p>Greetings, the {INSTANCE} team.</p>
          </body>
        </html>
        """
      : """
        <html lang="en" style="font-size: 10pt; font-family: Tahoma, serif;">
          <body style="color:#181515;">
           <h1>Hello, {USER}</h1>
           <p>Please click the button below to verify your email address and finish the registration of your account.</p>
           <a href="{CODE_URL}" style="color: white; background-color: #F07167; padding: 0.5rem 1rem; border-radius: 0.5rem; font-size: x-large; text-decoration: none; display: inline-block;">Verify</a>
           <p>If you didn't request this, please ignore this email.</p>
           <p>Greetings, the {INSTANCE} team.</p>
          </body>
        </html>
        """; 
    
    return html
      .Replace("{USER}", userName)
      .Replace("{INSTANCE}", instanceName)
      .Replace("{CODE_URL}", verifyUrl);;
  }
    
    public static string PasswordReset(string userName, string instanceName, string websiteUrl, string resetPasswordCode)
    {
      string passwordUrl = $"{websiteUrl}/resetPassword?code={resetPasswordCode}";

      
      return """
        <html lang="en" style="font-size: 10pt; font-family: Tahoma, serif;">
          <body style="color:#181515;">
            <h1>Hello, {USER}</h1>
            <p>You may click the button below to reset your password.</p>
            <a href="{CODE_URL}" style="color: white; background-color: #F07167; padding: 0.5rem 1rem; border-radius: 0.5rem; font-size: x-large; text-decoration: none; display: inline-block;">Reset Password</a>
            <p>If you didn't request this, please ignore this email.</p>
            <p>Greetings, the {INSTANCE} team.</p>
          </body>
        </html>
        """
        .Replace("{USER}", userName)
        .Replace("{INSTANCE}", instanceName)
        .Replace("{CODE_URL}", passwordUrl);
    }
}