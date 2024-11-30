using E_CommerceWeb.Repository.Interface;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using E_CommerceWeb.ViewModels.Email;
using System.Text;
using E_CommerceWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using E_CommerceWeb.Data;

namespace E_CommerceWeb.Repository
{
    public class EmailRepo : IEmailRepo
    {

        private readonly EmailConfigureModel _emailconfig;
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmailRepo(IOptions<EmailConfigureModel> emailconfig,
                         AppDbContext context,
                         UserManager<ApplicationUser> userManager)
        {
            _emailconfig = emailconfig.Value;
            _context = context;
            _userManager = userManager;
        }

        //Full Path to Templates Body 
        private const string BodyPath = @"EmailTemplateBody/{0}.html";



        //Get the Specific Body By Name
        private string Body(string TempName)
        {
            var body = File.ReadAllText(string.Format(BodyPath, TempName));
            return body;
        }



        //Get The Body With PlaceHolders
        private string UpdatePlaceHolders(string body, List<KeyValuePair<string, string>> placeHolders)
        {
            if (placeHolders != null && !string.IsNullOrEmpty(body))
            {
                foreach (var placeHolder in placeHolders)
                {
                    if (body.Contains(placeHolder.Key))
                    {
                        body = body.Replace(placeHolder.Key, placeHolder.Value);
                    }
                }
            }
            return body;
        }



        //Send Email Settings
        private async Task SendEmail(UserEmailOptions emailOptions)
        {
            //First Step
            MailMessage message = new MailMessage
            {
                Subject = emailOptions.Subject,
                Body = emailOptions.Body,
                From = new MailAddress(_emailconfig.SendingEmail, _emailconfig.DisplayName),
                IsBodyHtml = _emailconfig.IsBodyHTML,
                BodyEncoding = Encoding.Default
            };

            //If There Any Attachment
            if (emailOptions.Attachment != null)
            {
                message.Attachments.Add(new Attachment(emailOptions.Attachment.OpenReadStream(), emailOptions.Attachment.FileName));
            }

            //Send to Receive Emails 
            foreach (var User in emailOptions.RecieveEmails)
            {
                message.To.Add(User);
            }


            //Second Step
            SmtpClient client = new SmtpClient(_emailconfig.Host,_emailconfig.Port);
            try
            {
                client.Port = _emailconfig.Port;
                client.Host = _emailconfig.Host;
                client.UseDefaultCredentials = _emailconfig.UseDefaultCredentials;
                client.Credentials = new NetworkCredential(_emailconfig.UserName, _emailconfig.PassWord);
                client.EnableSsl = _emailconfig.EnableSSL;

                await client.SendMailAsync(message);
            }
            catch
            {
                client.Dispose();
            }

        }



        //Email Confirmation Method
        public async Task SendEmailConfirmation(UserEmailOptions emailOptions)
        {
            emailOptions.Subject = UpdatePlaceHolders("hi {{username}} , Email Confirmation :)", emailOptions.PlaceHolders);
            emailOptions.Body = UpdatePlaceHolders(Body("ConfirmEmail"), emailOptions.PlaceHolders);
            await SendEmail(emailOptions);
        }



        //Forget Password Confirmation Method
        public async Task SendForgetPasswordConfirmation(UserEmailOptions emailOptions)
        {
            emailOptions.Subject = UpdatePlaceHolders("hi {{username}} , ForgetPassword Confirmation :)", emailOptions.PlaceHolders);
            emailOptions.Body = UpdatePlaceHolders(Body("Forgetpassword"), emailOptions.PlaceHolders);
            await SendEmail(emailOptions);
        }







        //Send Code On the Email of User ==> Check Out Page
        public async Task SendDiscountCodeOnEmailAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            DiscountCode code = new DiscountCode
            {
                Code = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                DiscountPercentage = 10,
            };

            await _context.DiscountCodes.AddAsync(code);
            await _context.SaveChangesAsync();

            user.DisCodeId = code.Id;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();


            UserEmailOptions emailOption = new UserEmailOptions
            {
                RecieveEmails = new List<string> { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string> ( "{{username}}", user.FirstName ),
                    new KeyValuePair<string, string> ( "{{code}}", code.Code ),
                }
            };

            await SendDiscountCodeOnEmailAsync(emailOption);

        }

        //Private method for preparing Sending code to Users
        private async Task SendDiscountCodeOnEmailAsync(UserEmailOptions emailOption)
        {
            emailOption.Subject = UpdatePlaceHolders("Hi {{username}} , DisCount Code", emailOption.PlaceHolders);
            emailOption.Body = UpdatePlaceHolders(Body("DiscountEmail"), emailOption.PlaceHolders);
            await SendEmail(emailOption);
        }


    }
}
