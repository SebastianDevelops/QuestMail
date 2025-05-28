using System.Diagnostics;
using MedPulse.Infrastructure;
using MedPulse.Plugins.Types;
using MedPulse.ViewModel;
using PostmarkDotNet;

namespace MedPulse.Services;

public class PostmarkService(Settings settings, ISemanticKernelService semanticKernelService,
                             ICompanionService companionService) : IPostmarkService
{
    public async Task SendEmailAsync()
    {
        var client = new PostmarkClient(Environment.GetEnvironmentVariable("Settings.Postmark.apikey"));
        Console.WriteLine("Successfully created Postmark client with API key.");
        try
        {
            var email = await FormatEmailAsync();
            var sendResult = await client.SendMessageAsync(email);

            if (sendResult.Status == PostmarkStatus.Success){ Console.WriteLine($"Successfully sent email to {email.To}"); }
            else { 
                Debug.WriteLine($"Failed to send email to {email.To}. Error: {sendResult.Message}");
                throw new Exception($"Postmark API error: {sendResult.Message}");
            }

        }catch{
        }
    }
    
    private async Task<PostmarkMessage> FormatEmailAsync()
    {
        var email = await semanticKernelService.GetResponseAsync();
        var cid = $"{Guid.NewGuid()}:{Context.CompanionName}";
        Console.WriteLine("Calling image url creator");
        
        PostmarkMessage message = new PostmarkMessage();
        message.From = $"{Context.CompanionName} <{Environment.GetEnvironmentVariable("Settings.Postmark.fromEmail")}>";
        message.ReplyTo = $"{Environment.GetEnvironmentVariable("Settings.Postmark.replyTo")}";
        message.To = $"{Context.ToEmail}";
        message.Subject = email?.Subject;
        message.HtmlBody = $"{PrepareEmailBody(email, cid)}";
        message.MessageStream = "outbound";
        PostmarkMessageAttachment attachment = new PostmarkMessageAttachment();
        attachment.Name = Context.CompanionName;
        attachment.ContentType = "image/png";
        attachment.ContentId = cid;
        attachment.Content = await companionService.GetCompanionImageBase64();
        message.Attachments.Add(attachment);

        return message;
    }
    
    private string PrepareEmailBody(Email? email, string cid)
    {
        return $@"<!DOCTYPE html>
<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"" xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office"">
<head>
    <meta charset=""utf-8"">
    <meta name=""viewport"" content=""width=device-width"">
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
    <meta name=""x-apple-disable-message-reformatting"">
    <title>A Message from the Innkeeper of Eldoria</title>

    <!--[if mso]>
        <style>
            * {{
                font-family: sans-serif !important; /* Default fallback for Outlook */
            }}
            .font-medieval-email {{
                font-family: 'Times New Roman', Times, serif !important; /* MedievalSharp fallback */
            }}
            .font-cinzel-email {{
                 font-family: 'Georgia', serif !important; /* Cinzel fallback */
            }}
        </style>
    <![endif]-->

    <style>
        @import url('https://fonts.googleapis.com/css2?family=MedievalSharp&family=Cinzel:wght@400;700&family=Inter:wght@400;600;700&display=swap');

        html,
        body {{
            margin: 0 auto !important;
            padding: 0 !important;
            height: 100% !important;
            width: 100% !important;
            background-color: #1a1a2e; /* Dark blue-purple background */
        }}

        * {{
            -ms-text-size-adjust: 100%;
            -webkit-text-size-adjust: 100%;
        }}

        div[style*=""margin: 16px 0""] {{
            margin: 0 !important;
        }}

        table,
        td {{
            mso-table-lspace: 0pt !important;
            mso-table-rspace: 0pt !important;
        }}

        table {{
            border-spacing: 0 !important;
            border-collapse: collapse !important;
            table-layout: fixed !important;
            margin: 0 auto !important;
        }}

        img {{
            -ms-interpolation-mode:bicubic;
            border: 0; /* Ensure images don't have borders unless specified */
        }}

        a {{
            text-decoration: underline;
            color: #e0ac69; /* Goldenrod for links */
        }}
        a:hover {{
            text-decoration: none !important;
        }}

        .font-medieval-email {{
            font-family: 'MedievalSharp', cursive, 'Times New Roman', Times, serif;
        }}
        .font-cinzel-email {{
            font-family: 'Cinzel', serif, 'Georgia', serif;
        }}
        .font-inter-email {{
            font-family: 'Inter', sans-serif;
        }}

        .email-container {{
            width: 100%;
            max-width: 680px;
            margin: 0 auto;
            background-color: #1f1f38; /* Slightly lighter than body for main content area */
        }}

        .content-cell {{
            padding: 20px;
            font-family: 'Inter', sans-serif;
            font-size: 16px;
            line-height: 1.6;
            color: #e0e0e0; /* Light grey text */
        }}
        
        .button-td {{
            background-color: #e0ac69; /* Goldenrod */
            border-radius: 8px;
        }}
        .button-a {{
            background-color: #e0ac69; /* Goldenrod */
            border: 1px solid #e0ac69;
            font-family: 'Cinzel', serif;
            font-size: 18px;
            line-height: 1.1; /* Adjusted for better button text fit */
            text-align: center;
            text-decoration: none;
            display: block;
            border-radius: 8px;
            font-weight: bold;
            padding: 12px 25px;
            color: #1a1a2e; /* Dark text on button */
        }}
        .button-td:hover,
        .button-a:hover {{ /* Hover for clients that support it */
            background-color: #c99854 !important;
            border-color: #c99854 !important;
        }}

        .footer-text {{
            font-size: 12px;
            color: #a0a0a0;
            line-height: 1.5;
        }}
        .footer-link {{
            color: #c0c0c0;
            text-decoration: underline;
        }}

        /* Responsive Styles */
        @media screen and (max-width: 680px) {{
            .email-container {{
                width: 100% !important;
                margin: auto !important;
            }}
            .content-cell {{
                padding: 15px !important;
            }}
            .avatar-column {{
                width: 60px !important; /* Adjust avatar column width */
            }}
            .avatar-column img {{
                width: 50px !important;
                height: 50px !important;
            }}
            .innkeeper-name {{
                font-size: 16px !important;
            }}
            .main-text-column p {{
                font-size: 15px !important;
            }}
        }}

    </style>
</head>
<body width=""100%"" style=""margin: 0; padding: 0 !important; mso-line-height-rule: exactly; background-color: #1a1a2e;"">
    <center style=""width: 100%; background-color: #1a1a2e;"">
    <!--[if mso | IE]>
    <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""background-color: #1a1a2e;"">
    <tr>
    <td>
    <![endif]-->

        <div style=""display: none; font-size: 1px; line-height: 1px; max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden; mso-hide: all; font-family: sans-serif;"">
            The Innkeeper of Eldoria has a message for you, {Context.Username}...
        </div>

        <table align=""center"" role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" class=""email-container"" style=""margin: auto;"">
            <tr>
                <td style=""padding: 20px 0; text-align: center; background-color: #121220;"">
                    <a href=""#"" target=""_blank"" style=""text-decoration: none;"">
                        <h1 class=""font-cinzel-email"" style=""margin: 0; font-size: 36px; color: #ffffff; font-weight: bold;"">Eldoria</h1>
                    </a>
                </td>
            </tr>

            <tr>
                <td class=""content-cell"" style=""padding: 30px 20px;"">
                    <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"">
                        <tr>
                            <td width=""80"" valign=""top"" style=""padding-right: 15px; width: 80px;"" class=""avatar-column"">
                                <img src=""{cid}"" alt=""Innkeeper"" width=""60"" height=""60"" style=""border-radius: 50%; margin-bottom: 10px; border: 2px solid #e0ac69; display: block;"">
                                <p class=""font-cinzel-email innkeeper-name"" style=""color: #e0ac69; font-size: 18px; line-height: 1.2; margin: 0; font-weight: bold; word-break: break-word;"">
                                    {Context.CompanionName}
                                </p>
                            </td>

                            <td width=""2"" style=""width: 2px; background-color: #e0ac69; padding:0; margin:0;"">
                                <div style=""font-size: 0px; line-height: 0px; width: 2px; height: 100%; background-color: #e0ac69; display:block;"">&nbsp;</div>
                            </td>
                            <!--[if mso]>
                            <td width=""2"" bgcolor=""#e0ac69"" style=""width: 2px; background-color: #e0ac69; padding:0; margin:0;"">
                                <div style=""font-size:0px; line-height:1px; mso-line-height-rule:exactly; width:2px; background-color:#e0ac69;"">&nbsp;</div>
                            </td>
                            <![endif]-->

                            <td valign=""top"" style=""padding-left: 20px;"" class=""main-text-column"">
                                <p style=""margin: 0 0 15px 0; font-family: 'Inter', sans-serif; font-size: 16px; line-height: 1.6; color: #e0e0e0;"">
                                {email.Body.Paragraph1}
                                </p>
                                <p style=""margin: 0 0 15px 0; font-family: 'Inter', sans-serif; font-size: 16px; line-height: 1.6; color: #e0e0e0;"">
                                    {email.Body.Paragraph2}
                                </p>
                                <p style=""margin: 0 0 15px 0; font-family: 'Inter', sans-serif; font-size: 16px; line-height: 1.6; color: #e0e0e0;"">
                                    {email.Body.Paragraph3}
                                </p>
                                <p style=""margin: 0 0 25px 0; font-family: 'Inter', sans-serif; font-size: 16px; line-height: 1.6; color: #e0e0e0;"">
                                    {email.Body.Paragraph4}
                                </p>

                                <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" align=""left"" style=""margin-top: 10px;"">
                                    <tr>
                                        <td class=""button-td"" style=""border-radius: 8px;"">
                                            <a href=""#"" target=""_blank"" class=""button-a"">
                                                Keep Pushing!</a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

            <tr>
                <td style=""padding: 20px; text-align: center; background-color: #121220;"">
                    <p class=""footer-text"" style=""margin: 0 0 10px 0; color: #a0a0a0;"">
                        &copy; 2024 Whispers of Eldoria. All Rights Reserved.<br>
                        Your Game Company Name, 123 Fantasy Lane, Mythic City, EL 45678
                    </p>
                    <p class=""footer-text"" style=""margin: 0; color: #a0a0a0;"">
                        <a href=""YOUR_UNSUBSCRIBE_LINK"" target=""_blank"" class=""footer-link"" style=""color: #c0c0c0;"">Unsubscribe</a> | 
                        <a href=""YOUR_PREFERENCES_LINK"" target=""_blank"" class=""footer-link"" style=""color: #c0c0c0;"">Manage Preferences</a> | 
                        <a href=""YOUR_PRIVACY_POLICY_LINK"" target=""_blank"" class=""footer-link"" style=""color: #c0c0c0;"">Privacy Policy</a>
                    </p>
                </td>
            </tr>
        </table>

    <!--[if mso | IE]>
    </td>
    </tr>
    </table>
    <![endif]-->
    </center>
</body>
</html>
";
    }
}